using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicShare.Web.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Matricula",
                table: "AspNetUsers",
                newName: "University");

            migrationBuilder.RenameColumn(
                name: "Faculdade",
                table: "AspNetUsers",
                newName: "Registration");

            migrationBuilder.RenameColumn(
                name: "Curso",
                table: "AspNetUsers",
                newName: "Course");

            migrationBuilder.AlterColumn<string>(
                name: "Teacher",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "University",
                table: "AspNetUsers",
                newName: "Matricula");

            migrationBuilder.RenameColumn(
                name: "Registration",
                table: "AspNetUsers",
                newName: "Faculdade");

            migrationBuilder.RenameColumn(
                name: "Course",
                table: "AspNetUsers",
                newName: "Curso");

            migrationBuilder.AlterColumn<string>(
                name: "Teacher",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
