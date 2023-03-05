using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SleekFlow.Todo.Infrastructure.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TodoApp");

            migrationBuilder.CreateTable(
                name: "TodoLists",
                schema: "TodoApp",
                columns: table => new
                {
                    Id = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    CreatedBy = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoLists", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "TodoApp",
                columns: table => new
                {
                    Id = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: false),
                    UserName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    CreatedBy = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                schema: "TodoApp",
                columns: table => new
                {
                    Id = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                    DueDate = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ListId = table.Column<string>(type: "VARCHAR(36)", nullable: false),
                    CreatedBy = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: false),
                    UpdatedBy = table.Column<string>(type: "VARCHAR(36)", maxLength: 36, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "DATETIMEOFFSET", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_TodoItem_TodoList",
                        column: x => x.ListId,
                        principalSchema: "TodoApp",
                        principalTable: "TodoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ListId",
                schema: "TodoApp",
                table: "TodoItems",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "UC_AppUsers_UserName",
                schema: "TodoApp",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems",
                schema: "TodoApp");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "TodoApp");

            migrationBuilder.DropTable(
                name: "TodoLists",
                schema: "TodoApp");
        }
    }
}
