using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LivrariaApi.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaLivros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Titulo = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    Autor = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Preco = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: false),
                    AnoPublicacao = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Livros");
        }
    }
}
