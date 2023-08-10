using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperations",
                table: "UserOperations");

            migrationBuilder.RenameTable(
                name: "UserOperations",
                newName: "UserOperationsClaims");

            migrationBuilder.RenameIndex(
                name: "IX_UserOperations_Id",
                table: "UserOperationsClaims",
                newName: "IX_UserOperationsClaims_Id");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperationsClaims",
                table: "UserOperationsClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DoctorRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<int>(type: "integer", nullable: false),
                    RatingFromPatient = table.Column<int>(type: "integer", nullable: false),
                    CommentFromPatient = table.Column<string>(type: "text", nullable: false),
                    ConfirmedRating = table.Column<bool>(type: "boolean", nullable: false),
                    ConfirmedComment = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorRatings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Raiting = table.Column<double>(type: "double precision", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorRatings");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "UserTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOperationsClaims",
                table: "UserOperationsClaims");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "UserOperationsClaims",
                newName: "UserOperations");

            migrationBuilder.RenameIndex(
                name: "IX_UserOperationsClaims_Id",
                table: "UserOperations",
                newName: "IX_UserOperations_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOperations",
                table: "UserOperations",
                column: "Id");
        }
    }
}
