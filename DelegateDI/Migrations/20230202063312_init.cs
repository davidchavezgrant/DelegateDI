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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ImageUri = table.Column<string>(type: "text", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Permission = table.Column<int>(type: "integer", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsBylinesUser = table.Column<bool>(type: "boolean", nullable: false),
                    WalletAddress = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: true),
                    Permissions = table.Column<int>(type: "integer", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    DisplayName = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeSentNodaTicks = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Flagged = table.Column<bool>(type: "boolean", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    WalletAddress = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumberUnreadMessages = table.Column<int>(type: "integer", nullable: false),
                    PermissionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationFrequency = table.Column<int>(type: "integer", nullable: false),
                    AbstractChannelId = table.Column<Guid>(type: "uuid", nullable: false)
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
