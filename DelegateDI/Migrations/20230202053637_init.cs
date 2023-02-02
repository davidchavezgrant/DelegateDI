using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DelegateDI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "chat");

            migrationBuilder.CreateTable(
                name: "AbstractChannel",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUri = table.Column<string>(type: "TEXT", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelPermission",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Permission = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsBylinesUser = table.Column<bool>(type: "INTEGER", nullable: false),
                    WalletAddress = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Permissions = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessage",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeSentNodaTicks = table.Column<long>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SenderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Flagged = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessage_AbstractChannel_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "chat",
                        principalTable: "AbstractChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ContactProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nickname = table.Column<string>(type: "TEXT", nullable: false),
                    WalletAddress = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_Profiles_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "chat",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChannelConfigurations",
                schema: "chat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NumberUnreadMessages = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissionsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NotificationFrequency = table.Column<int>(type: "INTEGER", nullable: false),
                    AbstractChannelId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChannelConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChannelConfigurations_AbstractChannel_AbstractChannelId",
                        column: x => x.AbstractChannelId,
                        principalSchema: "chat",
                        principalTable: "AbstractChannel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserChannelConfigurations_ChannelPermission_PermissionsId",
                        column: x => x.PermissionsId,
                        principalSchema: "chat",
                        principalTable: "ChannelPermission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChannelConfigurations_Profiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalSchema: "chat",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_ChannelId",
                schema: "chat",
                table: "ChatMessage",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_OwnerId",
                schema: "chat",
                table: "Contact",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_WalletAddress_ApplicationId",
                schema: "chat",
                table: "Profiles",
                columns: new[] { "WalletAddress", "ApplicationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChannelConfigurations_AbstractChannelId",
                schema: "chat",
                table: "UserChannelConfigurations",
                column: "AbstractChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChannelConfigurations_PermissionsId",
                schema: "chat",
                table: "UserChannelConfigurations",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChannelConfigurations_UserProfileId",
                schema: "chat",
                table: "UserChannelConfigurations",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessage",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Contact",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "UserChannelConfigurations",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "AbstractChannel",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "ChannelPermission",
                schema: "chat");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "chat");
        }
    }
}
