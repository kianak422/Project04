using Microsoft.Data.SqlClient;
using System.Data;

namespace LopCRUDApp
{
    public class DangKyRepository
    {
        private readonly string _connectionString;

        public DangKyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void AddDangKy(DangKy dk)
        {
            // Giả định trigger V_DangKy (phần của Anh) đã được tạo
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO V_DangKy (MaSV, MaMon, Diem1, Diem2, Diem3, Site) VALUES (@MaSV, @MaMon, @Diem1, @Diem2, @Diem3, @Site)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", dk.MaSV);
                    command.Parameters.AddWithValue("@MaMon", dk.MaMon);
                    command.Parameters.AddWithValue("@Diem1", (object)dk.Diem1 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Diem2", (object)dk.Diem2 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Diem3", (object)dk.Diem3 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Site", dk.Site); // Cần Site để trigger định tuyến
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Đã thêm đăng ký cho SV {dk.MaSV} - Môn {dk.MaMon} vào {dk.Site}.");
        }

        public void UpdateDiem(string maSV, string maMon, string site, decimal? d1, decimal? d2, decimal? d3)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = @"UPDATE V_DangKy 
                                 SET Diem1 = @Diem1, Diem2 = @Diem2, Diem3 = @Diem3 
                                 WHERE MaSV = @MaSV AND MaMon = @MaMon AND Site = @Site";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    command.Parameters.AddWithValue("@MaMon", maMon);
                    command.Parameters.AddWithValue("@Site", site);
                    command.Parameters.AddWithValue("@Diem1", (object)d1 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Diem2", (object)d2 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Diem3", (object)d3 ?? DBNull.Value);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                        Console.WriteLine($"Đã cập nhật điểm cho SV {maSV} - Môn {maMon} tại {site}.");
                    else
                        Console.WriteLine("Không tìm thấy bản ghi để cập nhật.");
                }
            }
        }

        public void DeleteDangKy(string maSV, string maMon, string site)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "DELETE FROM V_DangKy WHERE MaSV = @MaSV AND MaMon = @MaMon AND Site = @Site";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    command.Parameters.AddWithValue("@MaMon", maMon);
                    command.Parameters.AddWithValue("@Site", site);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                        Console.WriteLine($"Đã xóa đăng ký cho SV {maSV} - Môn {maMon} tại {site}.");
                    else
                        Console.WriteLine("Không tìm thấy bản ghi để xóa.");
                }
            }
        }
    }
}