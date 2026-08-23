using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SprintFlow.Application.Common.Interfaces.Authentication;
using SprintFlow.Application.Common.Interfaces.Persistence;
using SprintFlow.Application.Common.MultiTenancy;
using SprintFlow.Domain.Common;
using SprintFlow.Domain.Entities;

namespace SprintFlow.Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>,
        IApplicationDbContext
{
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUserService _currentUserService;

    public Guid? CurrentTenantId => _currentTenant.Id;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenant currentTenant,
        ICurrentUserService currentUserService
    )
        : base(options)
    {
        _currentTenant = currentTenant;
        _currentUserService = currentUserService;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ApplyGlobalFilters(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInformation()
    {
        var userId = _currentUserService.UserId;
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
                entry.Entity.CreatedBy = userId;

                entry.Entity.UpdatedAt = null;
                entry.Entity.UpdatedBy = null;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = utcNow;
                entry.Entity.UpdatedBy = userId;

                entry.Property(x => x.CreatedAt).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;
            }
        }
    }

    private void ApplyGlobalFilters(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            LambdaExpression? filter = null;

            if (typeof(ITenantEntity).IsAssignableFrom(clrType))
            {
                filter = BuildTenantFilter(clrType);
            }

            if (typeof(BaseEntity).IsAssignableFrom(clrType))
            {
                var softDeleteFilter = BuildSoftDeleteFilter(clrType);

                filter = filter is null
                    ? softDeleteFilter
                    : CombineFilters(filter, softDeleteFilter);
            }

            if (filter is not null)
            {
                builder.Entity(clrType).HasQueryFilter(filter);
            }
        }
    }

    private LambdaExpression BuildTenantFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");

        var tenantId = Expression.Property(parameter, nameof(ITenantEntity.TenantId));

        var currentTenantId = Expression.Property(
            Expression.Constant(this),
            nameof(CurrentTenantId)
        );

        Expression tenantIdNullable =
            tenantId.Type == typeof(Guid) ? Expression.Convert(tenantId, typeof(Guid?)) : tenantId;

        var hasTenant = Expression.Property(currentTenantId, nameof(Nullable<Guid>.HasValue));

        var tenantMatches = Expression.Equal(tenantIdNullable, currentTenantId);

        var body = Expression.AndAlso(hasTenant, tenantMatches);

        return Expression.Lambda(body, parameter);
    }

    private LambdaExpression BuildSoftDeleteFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");

        var isDeleted = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));

        var body = Expression.Equal(isDeleted, Expression.Constant(false));

        return Expression.Lambda(body, parameter);
    }

    private LambdaExpression CombineFilters(LambdaExpression first, LambdaExpression second)
    {
        var parameter = first.Parameters[0];

        var secondBody = new ReplaceParameterVisitor(second.Parameters[0], parameter).Visit(
            second.Body
        )!;

        var body = Expression.AndAlso(first.Body, secondBody);

        return Expression.Lambda(body, parameter);
    }

    private sealed class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter;
        private readonly ParameterExpression _newParameter;

        public ReplaceParameterVisitor(
            ParameterExpression oldParameter,
            ParameterExpression newParameter
        )
        {
            _oldParameter = oldParameter;
            _newParameter = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == _oldParameter ? _newParameter : base.VisitParameter(node);
        }
    }
}
