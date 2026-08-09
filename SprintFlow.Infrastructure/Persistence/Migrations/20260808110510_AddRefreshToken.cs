using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SprintFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens"
            );

            migrationBuilder.DropPrimaryKey(name: "PK_Tenants", table: "Tenants");

            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers");

            migrationBuilder.RenameTable(name: "Tenants", newName: "AppTenants");

            migrationBuilder.RenameTable(name: "AspNetUsers", newName: "AppApplicationUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Tenants_Slug",
                table: "AppTenants",
                newName: "IX_AppTenants_Slug"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AppApplicationUsers",
                newName: "IX_AppApplicationUsers_TenantId"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTenants",
                table: "AppTenants",
                column: "Id"
            );

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppApplicationUsers",
                table: "AppApplicationUsers",
                column: "Id"
            );

            migrationBuilder.CreateTable(
                name: "AppRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenHash = table.Column<string>(
                        type: "nvarchar(128)",
                        maxLength: 128,
                        nullable: false
                    ),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplicationUserId = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: true
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRefreshTokens_AppApplicationUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AppApplicationUsers",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_AppRefreshTokens_AppApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_ApplicationUserId",
                table: "AppRefreshTokens",
                column: "ApplicationUserId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_TokenHash",
                table: "AppRefreshTokens",
                column: "TokenHash",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_AppRefreshTokens_UserId",
                table: "AppRefreshTokens",
                column: "UserId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AppApplicationUsers_AppTenants_TenantId",
                table: "AppApplicationUsers",
                column: "TenantId",
                principalTable: "AppTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AppApplicationUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AppApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AppApplicationUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AppApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AppApplicationUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AppApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AppApplicationUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AppApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppApplicationUsers_AppTenants_TenantId",
                table: "AppApplicationUsers"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AppApplicationUsers_UserId",
                table: "AspNetUserClaims"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AppApplicationUsers_UserId",
                table: "AspNetUserLogins"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AppApplicationUsers_UserId",
                table: "AspNetUserRoles"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AppApplicationUsers_UserId",
                table: "AspNetUserTokens"
            );

            migrationBuilder.DropTable(name: "AppRefreshTokens");

            migrationBuilder.DropPrimaryKey(name: "PK_AppTenants", table: "AppTenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppApplicationUsers",
                table: "AppApplicationUsers"
            );

            migrationBuilder.RenameTable(name: "AppTenants", newName: "Tenants");

            migrationBuilder.RenameTable(name: "AppApplicationUsers", newName: "AspNetUsers");

            migrationBuilder.RenameIndex(
                name: "IX_AppTenants_Slug",
                table: "Tenants",
                newName: "IX_Tenants_Slug"
            );

            migrationBuilder.RenameIndex(
                name: "IX_AppApplicationUsers_TenantId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_TenantId"
            );

            migrationBuilder.AddPrimaryKey(name: "PK_Tenants", table: "Tenants", column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade
            );
        }
    }
}
