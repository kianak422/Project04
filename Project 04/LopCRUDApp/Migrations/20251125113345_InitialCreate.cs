using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LopCRUDApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DangKys",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MaMon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Diem1 = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Diem2 = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Diem3 = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Site = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKys", x => new { x.MaSV, x.MaMon });
                });

            migrationBuilder.CreateTable(
                name: "Lops",
                columns: table => new
                {
                    MaLop = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenLop = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Site = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lops", x => x.MaLop);
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSV = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phai = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaLop = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HocBong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Site = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Khoa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSV);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DangKys");

            migrationBuilder.DropTable(
                name: "Lops");

            migrationBuilder.DropTable(
                name: "SinhViens");
        }
    }
}
