using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LopCRUDApp
{
    public class LopRepository
    {
        private readonly string _connectionString;

        public LopRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void AddLop(Lop lop)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO V_Lop (MaLop, TenLop, Khoa, Site) VALUES (@MaLop, @TenLop, @Khoa, @Site)", connection))
                {
                    command.Parameters.AddWithValue("@MaLop", lop.MaLop);
                    command.Parameters.AddWithValue("@TenLop", lop.TenLop);
                    command.Parameters.AddWithValue("@Khoa", lop.Khoa);
                    command.Parameters.AddWithValue("@Site", lop.Site);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Đã thêm lớp {lop.TenLop} vào Site {lop.Site}.");
        }

        public void UpdateLop(Lop lop)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE V_Lop SET TenLop = @TenLop, Khoa = @Khoa, Site = @Site WHERE MaLop = @MaLop AND Site = @OriginalSite", connection))
                {
                    command.Parameters.AddWithValue("@MaLop", lop.MaLop);
                    command.Parameters.AddWithValue("@TenLop", lop.TenLop);
                    command.Parameters.AddWithValue("@Khoa", lop.Khoa);
                    command.Parameters.AddWithValue("@Site", lop.Site);
                    // Cần một cách để xác định Site ban đầu để trigger hoạt động đúng
                    // Hiện tại, giả định Site không thay đổi khi update MaLop
                    command.Parameters.AddWithValue("@OriginalSite", lop.Site);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Đã cập nhật lớp {lop.MaLop} trên Site {lop.Site}.");
        }

        public void DeleteLop(string maLop, int site)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM V_Lop WHERE MaLop = @MaLop AND Site = @Site", connection))
                {
                    command.Parameters.AddWithValue("@MaLop", maLop);
                    command.Parameters.AddWithValue("@Site", site);
                    command.ExecuteNonQuery();
                }
            }
            Console.WriteLine($"Đã xóa lớp {maLop} trên Site {site}.");
        }

        public List<Lop> GetAllLops()
        {
            var lops = new List<Lop>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT MaLop, TenLop, Khoa, Site FROM V_Lop", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lops.Add(new Lop
                            {
                                MaLop = reader["MaLop"].ToString(),
                                TenLop = reader["TenLop"].ToString(),
                                Khoa = reader["Khoa"].ToString(),
                                Site = Convert.ToInt32(reader["Site"])
                            });
                        }
                    }
                }
            }
            return lops;
        }

        public Lop GetLopByMaLopAndSite(string maLop, int site)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT MaLop, TenLop, Khoa, Site FROM V_Lop WHERE MaLop = @MaLop AND Site = @Site", connection))
                {
                    command.Parameters.AddWithValue("@MaLop", maLop);
                    command.Parameters.AddWithValue("@Site", site);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Lop
                            {
                                MaLop = reader["MaLop"].ToString(),
                                TenLop = reader["TenLop"].ToString(),
                                Khoa = reader["Khoa"].ToString(),
                                Site = Convert.ToInt32(reader["Site"])
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}