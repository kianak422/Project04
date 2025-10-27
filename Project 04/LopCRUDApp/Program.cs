using System;
using System.Collections.Generic;

namespace LopCRUDApp
{
    class Program
    {
        // Cập nhật chuỗi kết nối của bạn tại đây
        private static readonly string ConnectionString = "Server=.;Database=master;Integrated Security=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            LopRepository lopRepo = new LopRepository(ConnectionString);
            DangKyRepository dkRepo = new DangKyRepository(ConnectionString);
            QueryRepository queryRepo = new QueryRepository(ConnectionString);

            while (true)
            {
                Console.WriteLine("\n--- HỆ THỐNG QUẢN LÝ SINH VIÊN PHÂN TÁN ---");
                Console.WriteLine("--- Module Lớp (Huy) ---");
                Console.WriteLine("1. Thêm Lớp");
                Console.WriteLine("2. Cập nhật Lớp");
                Console.WriteLine("3. Xóa Lớp");
                Console.WriteLine("4. Xem tất cả Lớp");
                Console.WriteLine("5. Tìm Lớp theo Mã Lớp và Site");
                Console.WriteLine("--- Module Đăng Ký (Trung) ---");
                Console.WriteLine("6. Thêm Đăng Ký");
                Console.WriteLine("7. Cập nhật Điểm");
                Console.WriteLine("8. Xóa Đăng Ký");
                Console.WriteLine("--- Module Truy Vấn (Trung) ---");
                Console.WriteLine("9. Form 1: Tìm Khoa của Sinh viên");
                Console.WriteLine("10. Form 2: Xem Bảng điểm Sinh viên");
                Console.WriteLine("11. Form 3: Điểm TB cao nhất mỗi Khoa");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddLop(lopRepo);
                        break;
                    case "2":
                        UpdateLop(lopRepo);
                        break;
                    case "3":
                        DeleteLop(lopRepo);
                        break;
                    case "4":
                        ViewAllLops(lopRepo);
                        break;
                    case "5":
                        GetLopByMaLopAndSite(lopRepo);
                        break;
                    case "6":
                        AddDangKy(dkRepo);
                        break;
                    case "7":
                        UpdateDiem(dkRepo);
                        break;
                    case "8":
                        DeleteDangKy(dkRepo);
                        break;
                    case "9":
                        GetKhoaForSinhVien(queryRepo);
                        break;
                    case "10":
                        GetDiemForAllMonHoc(queryRepo);
                        break;
                    case "11":
                        queryRepo.GetDiemTrungBinhCaoNhatKhoa();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            }
        }

        #region Chức năng của Trung
        static void AddDangKy(DangKyRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên: ");
            string maSV = Console.ReadLine();
            Console.Write("Nhập Mã Môn Học: ");
            string maMon = Console.ReadLine();
            Console.Write("Nhập Site ('Site1', 'Site2', 'Site3'): ");
            string site = Console.ReadLine(); // Cần site để trigger định tuyến

            DangKy newDK = new DangKy { MaSV = maSV, MaMon = maMon, Site = site };
            repository.AddDangKy(newDK);
        }

        static void UpdateDiem(DangKyRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên: ");
            string maSV = Console.ReadLine();
            Console.Write("Nhập Mã Môn Học: ");
            string maMon = Console.ReadLine();
            Console.Write("Nhập Site ('Site1', 'Site2', 'Site3'): ");
            string site = Console.ReadLine();

            Console.Write("Nhập Điểm 1 (bỏ trống nếu không đổi): ");
            string d1Str = Console.ReadLine();
            decimal? d1 = string.IsNullOrEmpty(d1Str) ? (decimal?)null : decimal.Parse(d1Str);

            Console.Write("Nhập Điểm 2 (bỏ trống nếu không đổi): ");
            string d2Str = Console.ReadLine();
            decimal? d2 = string.IsNullOrEmpty(d2Str) ? (decimal?)null : decimal.Parse(d2Str);

            Console.Write("Nhập Điểm 3 (bỏ trống nếu không đổi): ");
            string d3Str = Console.ReadLine();
            decimal? d3 = string.IsNullOrEmpty(d3Str) ? (decimal?)null : decimal.Parse(d3Str);

            repository.UpdateDiem(maSV, maMon, site, d1, d2, d3);
        }

        static void DeleteDangKy(DangKyRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần xóa: ");
            string maSV = Console.ReadLine();
            Console.Write("Nhập Mã Môn Học cần xóa: ");
            string maMon = Console.ReadLine();
            Console.Write("Nhập Site của bản ghi: ");
            string site = Console.ReadLine();

            repository.DeleteDangKy(maSV, maMon, site);
        }

        static void GetKhoaForSinhVien(QueryRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần tìm Khoa: ");
            string maSV = Console.ReadLine();
            repository.GetKhoaForSinhVien(maSV);
        }

        static void GetDiemForAllMonHoc(QueryRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần xem điểm: ");
            string maSV = Console.ReadLine();
            repository.GetDiemForAllMonHoc(maSV);
        }

        #endregion

        #region Chức năng của Huy (đã có)
        static void AddLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Tên Lớp: ");
            string tenLop = Console.ReadLine();
            Console.Write("Nhập Khoa: ");
            string khoa = Console.ReadLine();
            Console.Write("Nhập Site ('Site1', 'Site2', hoặc 'Site3'): ");
            string site = Console.ReadLine(); // Thay đổi từ int sang string

            Lop newLop = new Lop { MaLop = maLop, TenLop = tenLop, Khoa = khoa, Site = site };
            repository.AddLop(newLop);
        }

        static void UpdateLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần cập nhật: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Site của Lớp cần cập nhật: ");
            string site = Console.ReadLine();

            Lop existingLop = repository.GetLopByMaLopAndSite(maLop, site);
            if (existingLop == null)
            {
                Console.WriteLine($"Không tìm thấy Lớp với Mã Lớp {maLop} và Site {site}.");
                return;
            }

            Console.WriteLine($"Đang cập nhật Lớp: {existingLop.TenLop} (Khoa: {existingLop.Khoa})");
            Console.Write("Nhập Tên Lớp mới (để trống nếu không đổi): ");
            string tenLop = Console.ReadLine();
            if (!string.IsNullOrEmpty(tenLop))
            {
                existingLop.TenLop = tenLop;
            }
            Console.Write("Nhập Khoa mới (để trống nếu không đổi): ");
            string khoa = Console.ReadLine();
            if (!string.IsNullOrEmpty(khoa))
            {
                existingLop.Khoa = khoa;
            }

            repository.UpdateLop(existingLop);
        }

        static void DeleteLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần xóa: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Site của Lớp cần xóa: ");
            string site = Console.ReadLine();

            repository.DeleteLop(maLop, site);
        }

        static void ViewAllLops(LopRepository repository)
        {
            List<Lop> lops = repository.GetAllLops();
            if (lops.Count == 0)
            {
                Console.WriteLine("Không có lớp nào trong hệ thống.");
                return;
            }

            Console.WriteLine("\n--- Danh sách Lớp ---");
            foreach (var lop in lops)
            {
                Console.WriteLine($"Mã Lớp: {lop.MaLop}, Tên Lớp: {lop.TenLop}, Khoa: {lop.Khoa}, Site: {lop.Site}");
            }
        }

        static void GetLopByMaLopAndSite(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần tìm: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Site của Lớp cần tìm: ");
            string site = Console.ReadLine();

            Lop lop = repository.GetLopByMaLopAndSite(maLop, site);
            if (lop != null)
            {
                Console.WriteLine($"\n--- Thông tin Lớp ---");
                Console.WriteLine($"Mã Lớp: {lop.MaLop}, Tên Lớp: {lop.TenLop}, Khoa: {lop.Khoa}, Site: {lop.Site}");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy Lớp với Mã Lớp {maLop} và Site {site}.");
            }
        }
        #endregion

        // Cần cập nhật Lop.cs và LopRepository.cs để dùng Site là string thay vì int
    }
}