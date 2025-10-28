using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutorNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaisOrigem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.AutorId);
                });

            migrationBuilder.CreateTable(
                name: "Editoras",
                columns: table => new
                {
                    EditoraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EditoraNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editoras", x => x.EditoraId);
                });

            migrationBuilder.CreateTable(
                name: "Leitores",
                columns: table => new
                {
                    LeitorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeitorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<int>(type: "int", nullable: false),
                    CEP = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leitores", x => x.LeitorId);
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    LivroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LivroNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnoLancamento = table.Column<int>(type: "int", nullable: false),
                    Disponivel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.LivroId);
                });

            migrationBuilder.CreateTable(
                name: "AutoresLivros",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    LivroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLivros", x => new { x.AutorId, x.LivroId });
                    table.ForeignKey(
                        name: "FK_AutoresLivros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "AutorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoresLivros_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "LivroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publicacoes",
                columns: table => new
                {
                    LivroId = table.Column<int>(type: "int", nullable: false),
                    EditoraId = table.Column<int>(type: "int", nullable: false),
                    DataPublicacao = table.Column<int>(type: "int", nullable: false),
                    Edicao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacoes", x => new { x.EditoraId, x.LivroId, x.DataPublicacao, x.Edicao });
                    table.ForeignKey(
                        name: "FK_Publicacoes_Editoras_EditoraId",
                        column: x => x.EditoraId,
                        principalTable: "Editoras",
                        principalColumn: "EditoraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Publicacoes_Livros_LivroId",
                        column: x => x.LivroId,
                        principalTable: "Livros",
                        principalColumn: "LivroId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLivros_LivroId",
                table: "AutoresLivros",
                column: "LivroId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacoes_LivroId",
                table: "Publicacoes",
                column: "LivroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoresLivros");

            migrationBuilder.DropTable(
                name: "Leitores");

            migrationBuilder.DropTable(
                name: "Publicacoes");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Editoras");

            migrationBuilder.DropTable(
                name: "Livros");
        }
    }
}
