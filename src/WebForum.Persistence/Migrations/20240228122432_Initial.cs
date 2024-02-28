using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebForum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.PermissionId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.RoleId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.RoomId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Username = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Email = table.Column<string>(type: "varchar(192)", maxLength: 192, nullable: true),
                    PasswordHash = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    LockoutEnd = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Content = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DatePosted = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    RoomId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_comment_room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "registration_request",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SubmitDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registration_request", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_registration_request_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_login",
                columns: table => new
                {
                    LoginProvider = table.Column<int>(type: "int", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_user_login_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_permission",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RoomId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permission", x => new { x.UserId, x.RoomId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_user_permission_permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_permission_room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "room",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_permission_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_token",
                columns: table => new
                {
                    TokenId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_token", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_user_token_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "PermissionId", "Name" },
                values: new object[,]
                {
                    { 1, "CreateComment" },
                    { 2, "EditComment" },
                    { 3, "RemoveComment" },
                    { 4, "PostComment" },
                    { 5, "BlockComment" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "RoleId", "Name" },
                values: new object[,]
                {
                    { 1, "Regular" },
                    { 2, "Moderator" },
                    { 3, "Admin" },
                    { 4, "RootAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_comment_CommentId",
                table: "comment",
                column: "CommentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comment_RoomId",
                table: "comment",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_comment_UserId",
                table: "comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_permission_Name",
                table: "permission",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permission_PermissionId",
                table: "permission",
                column: "PermissionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_registration_request_RequestId",
                table: "registration_request",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_registration_request_UserId",
                table: "registration_request",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_Name",
                table: "role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_RoleId",
                table: "role",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_Name",
                table: "room",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_room_RoomId",
                table: "room",
                column: "RoomId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_UserId",
                table: "user",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_Username",
                table: "user",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_login_LoginProvider_ProviderKey",
                table: "user_login",
                columns: new[] { "LoginProvider", "ProviderKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_login_UserId",
                table: "user_login",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_PermissionId",
                table: "user_permission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_RoomId",
                table: "user_permission",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_user_permission_UserId_RoomId_PermissionId",
                table: "user_permission",
                columns: new[] { "UserId", "RoomId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_token_TokenId",
                table: "user_token",
                column: "TokenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_token_UserId",
                table: "user_token",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "registration_request");

            migrationBuilder.DropTable(
                name: "user_login");

            migrationBuilder.DropTable(
                name: "user_permission");

            migrationBuilder.DropTable(
                name: "user_token");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "role");
        }
    }
}
