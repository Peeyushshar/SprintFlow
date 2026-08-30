using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SprintFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeletableUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AppApplicationUsers",
                type: "datetime2",
                nullable: true
            );

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "AppApplicationUsers",
                type: "uniqueidentifier",
                nullable: true
            );

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppApplicationUsers",
                type: "bit",
                nullable: false,
                defaultValue: false
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DeletedAt", table: "AppApplicationUsers");

            migrationBuilder.DropColumn(name: "DeletedBy", table: "AppApplicationUsers");

            migrationBuilder.DropColumn(name: "IsDeleted", table: "AppApplicationUsers");
        }
    }
}
