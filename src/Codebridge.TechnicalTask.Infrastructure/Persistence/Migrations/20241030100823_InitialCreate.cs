using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codebridge.TechnicalTask.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dogs",
                columns: table => new
                {
                    name = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: false),
                    color = table.Column<string>(type: "nvarchar(127)", maxLength: 127, nullable: false),
                    tail_length = table.Column<double>(type: "float", nullable: false),
                    weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dogs", x => x.name);
                    table.CheckConstraint("ck_dog_tail_length", "tail_length >= 0");
                    table.CheckConstraint("ck_dog_weight", "weight > 0");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dogs");
        }
    }
}
