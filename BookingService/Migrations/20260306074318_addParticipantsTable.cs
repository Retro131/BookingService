using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingService.Migrations
{
    /// <inheritdoc />
    public partial class addParticipantsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId_StartDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bookings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:booking_participant_status", "accepted,declined,pending")
                .Annotation("Npgsql:Enum:booking_role", "organizer,participant")
                .Annotation("Npgsql:Enum:booking_status", "active,cancelled,completed")
                .OldAnnotation("Npgsql:Enum:booking_status", "active,cancelled,completed");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Bookings",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tittle",
                table: "Bookings",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BookingParticipants",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingParticipants", x => new { x.UserId, x.BookingId });
                    table.ForeignKey(
                        name: "FK_BookingParticipants_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingParticipants_BookingId",
                table: "BookingParticipants",
                column: "BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingParticipants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Tittle",
                table: "Bookings");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:booking_status", "active,cancelled,completed")
                .OldAnnotation("Npgsql:Enum:booking_participant_status", "accepted,declined,pending")
                .OldAnnotation("Npgsql:Enum:booking_role", "organizer,participant")
                .OldAnnotation("Npgsql:Enum:booking_status", "active,cancelled,completed");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Bookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_StartDate",
                table: "Bookings",
                columns: new[] { "UserId", "StartDate" });
        }
    }
}
