using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace SinhVienCRUDApp
{
    public class SinhVienRepository
    {
        private readonly string _connectionString;

        public SinhVienRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // CREATE
        public void AddSinhVien(SinhVien sv)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    @"INSERT INTO V_SinhVien (MaSV, HoTen, Phai, NgaySinh, MaLop, HocBong, Site)
                      VALUES (@MaSV, @HoTen, @Phai, @NgaySinh, @MaLop, @HocBong, @Site)", connection))
                {
                    command.Parameters.AddWithValue("@MaSV", sv.MaSV);
                    command.Parameters.AddWithValue("@HoTen", sv.HoTen);
                    command.Parameters.AddWithValue("@Phai", sv.Phai);
                    command.Parameters.AddWithValue("@NgaySinh", sv.NgaySinh);
                    command.Parameters.AddWithValue("@MaLop", sv.MaLop);
                    command.Parameters.AddWithValue("@HocBong", sv.HocBong);
                    command.Parameters.AddWithValue("@Site", sv.Site);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine($"Đã thêm sinh viên {sv.HoTen} vào Site {sv.Site}.");
        }

        // UPDATE
        public void UpdateSinhVien(SinhVien sv)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    @"UPDATE V_SinhVien 
                      SET HoTen = @HoTen, Phai = @Phai, NgaySinh = @NgaySinh, 
                          MaLop = @MaLop, HocBong = @HocBong
                      WHERE MaSV = @MaSV AND Site = @Site", connection))
                {
                    command.Parameters.AddWithValue("@MaSV", sv.MaSV);
                    command.Parameters.AddWithValue("@HoTen", sv.HoTen);
                    command.Parameters.AddWithValue("@Phai", sv.Phai);
                    command.Parameters.AddWithValue("@NgaySinh", sv.NgaySinh);
                    command.Parameters.AddWithValue("@MaLop", sv.MaLop);
                    command.Parameters.AddWithValue("@HocBong", sv.HocBong);
                    command.Parameters.AddWithValue("@Site", sv.Site);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine($"Đã cập nhật sinh viên {sv.MaSV} trên Site {sv.Site}.");
        }

        // DELETE
        public void DeleteSinhVien(string maSV, string site)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    @"DELETE FROM V_SinhVien WHERE MaSV = @MaSV AND Site = @Site",
                    connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    command.Parameters.AddWithValue("@Site", site);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine($"Đã xóa sinh viên {maSV} trên Site {site}.");
        }

        // READ ALL
        public List<SinhVien> GetAllSinhVien()
        {
            var list = new List<SinhVien>();

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    @"SELECT MaSV, HoTen, Phai, NgaySinh, MaLop, HocBong, Site 
                      FROM V_SinhVien", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new SinhVien
                            {
                                MaSV = reader["MaSV"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                Phai = reader["Phai"].ToString(),
                                NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                MaLop = reader["MaLop"].ToString(),
                                HocBong = Convert.ToDecimal(reader["HocBong"]),
                                Site = reader["Site"].ToString()
                            });
                        }
                    }
                }
            }

            return list;
        }

        // READ by MaSV + Site (hàm gốc của bạn)
        public SinhVien GetSinhVienByMaSVAndSite(string maSV, string site)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                using (var command = new SqlCommand(
                    @"SELECT MaSV, HoTen, Phai, NgaySinh, MaLop, HocBong, Site
                      FROM V_SinhVien
                      WHERE MaSV = @MaSV AND Site = @Site", connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    command.Parameters.AddWithValue("@Site", site);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SinhVien
                            {
                                MaSV = reader["MaSV"].ToString(),
                                HoTen = reader["HoTen"].ToString(),
                                Phai = reader["Phai"].ToString(),
                                NgaySinh = Convert.ToDateTime(reader["NgaySinh"]),
                                MaLop = reader["MaLop"].ToString(),
                                HocBong = Convert.ToDecimal(reader["HocBong"]),
                                Site = reader["Site"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        // 🔥 HÀM MỚI THÊM VÀO — PHÙ HỢP VỚI Program.cs
        public SinhVien GetSinhVienById(string maSV, string site)
        {
            return GetSinhVienByMaSVAndSite(maSV, site);
        }
    }
}
