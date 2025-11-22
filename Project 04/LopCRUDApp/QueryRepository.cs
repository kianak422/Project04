using Microsoft.Data.SqlClient;
using System.Data;

namespace LopCRUDApp
{
    public class QueryRepository
    {
        private readonly string _connectionString;

        public QueryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // Form 1: Khoa của một sinh viên
        public void GetKhoaForSinhVien(string maSV)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                // Dùng View toàn cục V_SinhVien và V_Lop
                string query = @"SELECT sv.MaSV, sv.HoTen, l.Khoa 
                                 FROM V_SinhVien sv 
                                 JOIN V_Lop l ON sv.MaLop = l.MaLop AND sv.Site = l.Site
                                 WHERE sv.MaSV = @MaSV";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine($"--- Form 1: Khoa của Sinh viên ---");
                            Console.WriteLine($"Mã SV: {reader["MaSV"]}");
                            Console.WriteLine($"Họ Tên: {reader["HoTen"]}");
                            Console.WriteLine($"Khoa: {reader["Khoa"]}");
                        }
                        else
                        {
                            Console.WriteLine($"Không tìm thấy sinh viên với mã {maSV}.");
                        }
                    }
                }
            }
        }

        // Form 2: Điểm của sinh viên trong tất cả các môn học
        public void GetDiemForAllMonHoc(string maSV)
        {
            Console.WriteLine($"--- Form 2: Bảng điểm của SV {maSV} ---");
            using (var connection = GetConnection())
            {
                connection.Open();
                // Dùng View toàn cục V_DangKy
                string query = @"SELECT MaMon, Diem1, Diem2, Diem3, 
                                        (ISNULL(Diem1,0) + ISNULL(Diem2,0) + ISNULL(Diem3,0)) / 
                                        (CASE WHEN Diem1 IS NULL THEN 0 ELSE 1 END + 
                                         CASE WHEN Diem2 IS NULL THEN 0 ELSE 1 END + 
                                         CASE WHEN Diem3 IS NULL THEN 0 ELSE 1 END) AS DiemTB
                                 FROM V_DangKy 
                                 WHERE MaSV = @MaSV";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaSV", maSV);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine($"Sinh viên {maSV} chưa đăng ký môn nào.");
                            return;
                        }
                        Console.WriteLine($"| {"MaMon",-10} | {"Diem1",-6} | {"Diem2",-6} | {"Diem3",-6} | {"DiemTB",-6} |");
                        Console.WriteLine(new string('-', 44));
                        while (reader.Read())
                        {
                            Console.WriteLine($"| {reader["MaMon"],-10} | {reader["Diem1"],-6} | {reader["Diem2"],-6} | {reader["Diem3"],-6} | {reader["DiemTB"],-6:F2} |");
                        }
                    }
                }
            }
        }

        // Form 3: Trung bình điểm cao nhất của mỗi khoa
        public void GetDiemTrungBinhCaoNhatKhoa()
        {
            Console.WriteLine($"--- Form 3: Điểm TB cao nhất mỗi Khoa ---");
            using (var connection = GetConnection())
            {
                connection.Open();
                // Truy vấn này phức tạp, cần JOIN cả 3 View
                string query = @"
                    ;WITH DiemTB_CTE AS (
                        -- Tính điểm TB từng môn của mỗi SV
                        SELECT MaSV, MaMon, 
                               (ISNULL(Diem1,0) + ISNULL(Diem2,0) + ISNULL(Diem3,0)) / 
                               (NULLIF(CASE WHEN Diem1 IS NULL THEN 0 ELSE 1 END + 
                                        CASE WHEN Diem2 IS NULL THEN 0 ELSE 1 END + 
                                        CASE WHEN Diem3 IS NULL THEN 0 ELSE 1 END, 0)) AS DiemTBMon
                        FROM V_DangKy
                    ),
                    DiemTB_SV_CTE AS (
                        -- Tính điểm TB chung của mỗi SV
                        SELECT MaSV, AVG(DiemTBMon) AS DiemTBSV
                        FROM DiemTB_CTE
                        WHERE DiemTBMon IS NOT NULL
                        GROUP BY MaSV
                    ),
                    SV_Khoa_CTE AS (
                        -- Lấy Khoa của mỗi SV
                        SELECT sv.MaSV, l.Khoa
                        FROM V_SinhVien sv
                        JOIN V_Lop l ON sv.MaLop = l.MaLop AND sv.Site = l.Site
                    )
                    -- Lấy MAX(Điểm TB) của mỗi Khoa
                    SELECT k.Khoa, MAX(dtb.DiemTBSV) AS DiemTBMax
                    FROM DiemTB_SV_CTE dtb
                    JOIN SV_Khoa_CTE k ON dtb.MaSV = k.MaSV
                    GROUP BY k.Khoa
                    ORDER BY Khoa";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine($"| {"Khoa",-30} | {"DiemTB Cao Nhat",-20} |");
                        Console.WriteLine(new string('-', 54));
                        while (reader.Read())
                        {
                            Console.WriteLine($"| {reader["Khoa"],-30} | {reader["DiemTBMax"],-20:F2} |");
                        }
                    }
                }
            }
        }
    }
}