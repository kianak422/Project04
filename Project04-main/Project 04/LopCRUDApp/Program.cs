using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LopCRUDApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=CSDLPT;Integrated Security=True;TrustServerCertificate=True;");

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                context.Database.Migrate(); // Apply any pending migrations

                // --- KHỞI TẠO CÁC REPOSITORY ---
                LopRepository lopRepo = new LopRepository(context);
                DangKyRepository dkRepo = new DangKyRepository(context);
                QueryRepository queryRepo = new QueryRepository(context);
                SinhVienRepository svRepo = new SinhVienRepository(context); 

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
                    Console.WriteLine("--- Module Sinh Viên (Anh) ---");
                    Console.WriteLine("12. Thêm Sinh Viên");
                    Console.WriteLine("13. Cập nhật Sinh Viên");
                    Console.WriteLine("14. Xóa Sinh Viên");
                    Console.WriteLine("15. Xem tất cả Sinh Viên");
                    Console.WriteLine("16. Tìm Sinh Viên theo Mã & Site");
                    Console.WriteLine("17. Thêm dữ liệu mẫu");
                    Console.WriteLine("0. Thoát");
                    Console.Write("Chọn chức năng: ");

                    string choice = Console.ReadLine() ?? "";

                    if (choice == "17")
                    {
                        AddSampleData(lopRepo, svRepo, dkRepo);
                        return; // Thoát ứng dụng sau khi thêm dữ liệu mẫu
                    }

                    switch (choice)
                    {
                        case "1": AddLop(lopRepo); break;
                        case "2": UpdateLop(lopRepo); break;
                        case "3": DeleteLop(lopRepo); break;
                        case "4": ViewAllLops(lopRepo); break;
                        case "5": GetLopByMaLopAndSite(lopRepo); break;
                        
                        case "6": AddDangKy(dkRepo); break;
                        case "7": UpdateDiem(dkRepo); break;
                        case "8": DeleteDangKy(dkRepo); break;
                        
                        case "9": GetKhoaByMaSV(queryRepo); break;
                        case "10": GetDiemByMaSV(queryRepo); break;
                        case "11": GetDiemMaxKhoa(queryRepo); break;
                        
                        // Các case này sẽ chạy OK vì svRepo đã được khai báo ở trên
                        case "12": AddSinhVien(svRepo); break;
                        case "13": UpdateSinhVien(svRepo); break;
                        case "14": DeleteSinhVien(svRepo); break;
                        case "15": ViewAllSinhVien(svRepo); break;
                        case "16": GetSinhVienByMaSVAndSite(svRepo); break;
                        
                        case "0": return;
                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                            break;
                    }
                }
            }
        }

        #region Chức năng của Trung
        static void AddDangKy(DangKyRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Mã Môn Học: ");
            string maMon = Console.ReadLine() ?? "";
            Console.Write("Nhập Site ('Site1', 'Site2', 'Site3'): ");
            string site = Console.ReadLine() ?? ""; 

            DangKy newDK = new DangKy { MaSV = maSV, MaMon = maMon, Site = site };
            repository.AddDangKy(newDK);
        }

        static void UpdateDiem(DangKyRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Mã Môn Học: ");
            string maMon = Console.ReadLine() ?? "";
            Console.Write("Nhập Site ('Site1', 'Site2', 'Site3'): ");
            string site = Console.ReadLine() ?? "";

            Console.Write("Nhập Điểm 1 (bỏ trống nếu không đổi): ");
            string d1Str = Console.ReadLine() ?? "";
            decimal? d1 = string.IsNullOrEmpty(d1Str) ? (decimal?)null : decimal.Parse(d1Str);

            Console.Write("Nhập Điểm 2 (bỏ trống nếu không đổi): ");
            string d2Str = Console.ReadLine() ?? "";
            decimal? d2 = string.IsNullOrEmpty(d2Str) ? (decimal?)null : decimal.Parse(d2Str);

            Console.Write("Nhập Điểm 3 (bỏ trống nếu không đổi): ");
            string d3Str = Console.ReadLine() ?? "";
            decimal? d3 = string.IsNullOrEmpty(d3Str) ? (decimal?)null : decimal.Parse(d3Str);

            repository.UpdateDiem(maSV, maMon, site, d1, d2, d3);
        }

        static void DeleteDangKy(DangKyRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần xóa: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Mã Môn Học cần xóa: ");
            string maMon = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của bản ghi: ");
            string site = Console.ReadLine() ?? "";

            repository.DeleteDangKy(maSV, maMon, site);
        }

        static void GetKhoaByMaSV(QueryRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần tìm Khoa: ");
            string maSV = Console.ReadLine() ?? "";
            repository.GetKhoaByMaSV(maSV);
        }

        static void GetDiemByMaSV(QueryRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần xem điểm: ");
            string maSV = Console.ReadLine() ?? "";
            repository.GetDiemByMaSV(maSV);
        }

        static void GetDiemMaxKhoa(QueryRepository repository)
        {
            repository.GetDiemMaxKhoa();
        }

        #endregion

        #region Chức năng của Huy (đã có)
        static void AddLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp: ");
            string maLop = Console.ReadLine() ?? "";
            Console.Write("Nhập Tên Lớp: ");
            string tenLop = Console.ReadLine() ?? "";
            Console.Write("Nhập Khoa: ");
            string khoa = Console.ReadLine() ?? "";
            Console.Write("Nhập Site ('Site1', 'Site2', hoặc 'Site3'): ");
            string site = Console.ReadLine() ?? ""; 

            Lop newLop = new Lop { MaLop = maLop, TenLop = tenLop, Khoa = khoa, Site = site };
            repository.AddLop(newLop);
        }

        static void UpdateLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần cập nhật: ");
            string maLop = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của Lớp cần cập nhật: ");
            string site = Console.ReadLine() ?? "";

            Lop? existingLop = repository.GetLopByMaLopAndSite(maLop, site);
            if (existingLop == null)
            {
                Console.WriteLine($"Không tìm thấy Lớp với Mã Lớp {maLop} và Site {site}.");
                return;
            }

            Console.WriteLine($"Đang cập nhật Lớp: {existingLop.TenLop} (Khoa: {existingLop.Khoa})");
            Console.Write("Nhập Tên Lớp mới (để trống nếu không đổi): ");
            string tenLop = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(tenLop))
            {
                existingLop.TenLop = tenLop;
            }
            Console.Write("Nhập Khoa mới (để trống nếu không đổi): ");
            string khoa = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(khoa))
            {
                existingLop.Khoa = khoa;
            }

            repository.UpdateLop(existingLop);
        }

        static void DeleteLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần xóa: ");
            string maLop = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của Lớp cần xóa: ");
            string site = Console.ReadLine() ?? "";

            repository.DeleteLop(maLop, site);
        }

        static void ViewAllLops(LopRepository repository)
        {
            Console.WriteLine("\n--- Danh sách tất cả Lớp ---");
            var lops = repository.GetAllLops();
            if (!lops.Any())
            {
                Console.WriteLine("Không có lớp nào trong hệ thống.");
                return;
            }

            Console.WriteLine($"| {"Mã Lớp",-10} | {"Tên Lớp",-25} | {"Khoa",-20} | {"Site",-10} |");
            Console.WriteLine(new string('-', 70));
            foreach (var lop in lops)
            {
                Console.WriteLine($"| {lop.MaLop,-10} | {lop.TenLop,-25} | {lop.Khoa,-20} | {lop.Site,-10} |");
            }
        }

        static void GetLopByMaLopAndSite(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần tìm: ");
            string maLop = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của Lớp cần tìm: ");
            string site = Console.ReadLine() ?? "";

            Lop? lop = repository.GetLopByMaLopAndSite(maLop, site);
            if (lop != null)
            {
                Console.WriteLine($"\n--- Thông tin Lớp ---");
                Console.WriteLine($"Mã Lớp: {lop.MaLop}");
                Console.WriteLine($"Tên Lớp: {lop.TenLop}");
                Console.WriteLine($"Khoa: {lop.Khoa}");
                Console.WriteLine($"Site: {lop.Site}");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy Lớp với Mã Lớp {maLop} và Site {site}.");
            }
        }
        #endregion

        #region Chức năng của Anh
        static void AddSinhVien(SinhVienRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Họ Tên: ");
            string hoTen = Console.ReadLine() ?? "";
            Console.Write("Nhập Ngày Sinh (yyyy-MM-dd): ");
            DateTime ngaySinh = DateTime.Parse(Console.ReadLine() ?? "");

            Console.Write("Nhập Mã Lớp: ");
            string maLop = Console.ReadLine() ?? "";
            Console.Write("Nhập Phái (Nam/Nu): ");
            string phai = Console.ReadLine() ?? "";
            Console.Write("Nhập Khoa: ");
            string khoa = Console.ReadLine() ?? "";
            Console.Write("Nhập Site ('Site1', 'Site2', 'Site3'): ");
            string site = Console.ReadLine() ?? "";

            SinhVien newSV = new SinhVien { MaSV = maSV, HoTen = hoTen, NgaySinh = ngaySinh, MaLop = maLop, Phai = phai, Khoa = khoa, Site = site };
            repository.AddSinhVien(newSV);
        }

        static void UpdateSinhVien(SinhVienRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần cập nhật: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của Sinh Viên cần cập nhật: ");
            string site = Console.ReadLine() ?? "";

            SinhVien? existingSV = repository.GetSinhVienByMaSVAndSite(maSV, site);
            if (existingSV == null)
            {
                Console.WriteLine($"Không tìm thấy Sinh Viên với Mã SV {maSV} và Site {site}.");
                return;
            }

            Console.WriteLine($"Đang cập nhật Sinh Viên: {existingSV.HoTen}");
            Console.Write("Nhập Họ Tên mới (để trống nếu không đổi): ");
            string hoTen = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(hoTen))
            {
                existingSV.HoTen = hoTen;
            }
            Console.Write("Nhập Ngày Sinh mới (yyyy-MM-dd, để trống nếu không đổi): ");
            string ngaySinhStr = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(ngaySinhStr))
            {
                existingSV.NgaySinh = DateTime.Parse(ngaySinhStr);
            }

            Console.Write("Nhập Mã Lớp mới (để trống nếu không đổi): ");
            string maLop = Console.ReadLine() ?? "";
            if (!string.IsNullOrEmpty(maLop))
            {
                existingSV.MaLop = maLop;
            }

            repository.UpdateSinhVien(existingSV);
        }

        static void DeleteSinhVien(SinhVienRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần xóa: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của Sinh Viên cần xóa: ");
            string site = Console.ReadLine() ?? "";

            repository.DeleteSinhVien(maSV, site);
        }

        static void ViewAllSinhVien(SinhVienRepository repository)
        {
            Console.WriteLine("\n--- Danh sách tất cả Sinh Viên ---");
            var sinhViens = repository.GetAllSinhViens();
            if (!sinhViens.Any())
            {
                Console.WriteLine("Không có sinh viên nào trong hệ thống.");
                return;
            }

            Console.WriteLine($"| {"Mã SV",-10} | {"Họ Tên",-25} | {"Ngày Sinh",-12} | {"Mã Lớp",-10} | {"Site",-10} |");
            Console.WriteLine(new string('-', 80));
            foreach (var sv in sinhViens)
            {
                Console.WriteLine($"| {sv.MaSV,-10} | {sv.HoTen,-25} | {sv.NgaySinh.ToShortDateString(),-12} | {sv.MaLop,-10} | {sv.Site,-10} |");
            }
        }

        static void GetSinhVienByMaSVAndSite(SinhVienRepository repository)
        {
            Console.Write("Nhập Mã Sinh Viên cần tìm: ");
            string maSV = Console.ReadLine() ?? "";
            Console.Write("Nhập Site của Sinh Viên cần tìm: ");
            string site = Console.ReadLine() ?? "";

            SinhVien? sv = repository.GetSinhVienByMaSVAndSite(maSV, site);
            if (sv != null)
            {
                Console.WriteLine($"\n--- Thông tin Sinh Viên ---");
                Console.WriteLine($"Mã SV: {sv.MaSV}");
                Console.WriteLine($"Họ Tên: {sv.HoTen}");
                Console.WriteLine($"Ngày Sinh: {sv.NgaySinh.ToShortDateString()}");

                Console.WriteLine($"Mã Lớp: {sv.MaLop}");
                Console.WriteLine($"Site: {sv.Site}");
            }
            else
            {
                Console.WriteLine($"Không tìm thấy Sinh Viên với Mã SV {maSV} và Site {site}.");
            }
        }
        #endregion

        static void AddSampleData(LopRepository lopRepo, SinhVienRepository svRepo, DangKyRepository dkRepo)
        {
            Console.WriteLine("\n--- Thêm dữ liệu mẫu ---");

            // Xóa dữ liệu cũ trước khi thêm mới
            dkRepo.DeleteAllDangKys();
            svRepo.DeleteAllSinhViens();
            lopRepo.DeleteAllLops();

            // Thêm dữ liệu mẫu cho Lop
            Console.WriteLine("Thêm dữ liệu mẫu cho Lớp...");
            lopRepo.AddLop(new Lop { MaLop = "L001", TenLop = "Cong Nghe Phan Mem", Khoa = "CNTT", Site = "Site1" });
            lopRepo.AddLop(new Lop { MaLop = "L002", TenLop = "He Thong Thong Tin", Khoa = "CNTT", Site = "Site2" });
            lopRepo.AddLop(new Lop { MaLop = "L003", TenLop = "Khoa Hoc May Tinh", Khoa = "CNTT", Site = "Site3" });
            lopRepo.AddLop(new Lop { MaLop = "L004", TenLop = "An Toan Thong Tin", Khoa = "CNTT", Site = "Site1" });
            lopRepo.AddLop(new Lop { MaLop = "L005", TenLop = "Ky Thuat Phan Mem", Khoa = "CNTT", Site = "Site2" });
            Console.WriteLine("Đã thêm 5 Lớp mẫu.");

            // Thêm dữ liệu mẫu cho SinhVien
            Console.WriteLine("Thêm dữ liệu mẫu cho Sinh Viên...");
            svRepo.AddSinhVien(new SinhVien { MaSV = "SV001", HoTen = "Nguyen Van A", NgaySinh = new DateTime(2002, 1, 1), Phai = "Nam", MaLop = "L001", Khoa = "CNTT", Site = "Site1" });
            svRepo.AddSinhVien(new SinhVien { MaSV = "SV002", HoTen = "Tran Thi B", NgaySinh = new DateTime(2001, 5, 10), Phai = "Nu", MaLop = "L002", Khoa = "CNTT", Site = "Site2" });
            svRepo.AddSinhVien(new SinhVien { MaSV = "SV003", HoTen = "Le Van C", NgaySinh = new DateTime(2003, 9, 20), Phai = "Nam", MaLop = "L003", Khoa = "CNTT", Site = "Site3" });
            svRepo.AddSinhVien(new SinhVien { MaSV = "SV004", HoTen = "Pham Thi D", NgaySinh = new DateTime(2002, 3, 15), Phai = "Nu", MaLop = "L001", Khoa = "CNTT", Site = "Site1" });
            svRepo.AddSinhVien(new SinhVien { MaSV = "SV005", HoTen = "Hoang Van E", NgaySinh = new DateTime(2001, 7, 25), Phai = "Nam", MaLop = "L002", Khoa = "CNTT", Site = "Site2" });
            Console.WriteLine("Đã thêm 5 Sinh Viên mẫu.");

            // Thêm dữ liệu mẫu cho DangKy
            Console.WriteLine("Thêm dữ liệu mẫu cho Đăng Ký...");
            dkRepo.AddDangKy(new DangKy { MaSV = "SV001", MaMon = "MON01", Site = "Site1", Diem1 = 7.5m, Diem2 = 8.0m, Diem3 = 7.8m });
            dkRepo.AddDangKy(new DangKy { MaSV = "SV002", MaMon = "MON02", Site = "Site2", Diem1 = 6.0m, Diem2 = 7.0m, Diem3 = 6.5m });
            dkRepo.AddDangKy(new DangKy { MaSV = "SV003", MaMon = "MON03", Site = "Site3", Diem1 = 8.0m, Diem2 = 8.5m, Diem3 = 8.2m });
            dkRepo.AddDangKy(new DangKy { MaSV = "SV004", MaMon = "MON01", Site = "Site1", Diem1 = 9.0m, Diem2 = 8.5m, Diem3 = 8.8m });
            dkRepo.AddDangKy(new DangKy { MaSV = "SV005", MaMon = "MON02", Site = "Site2", Diem1 = 7.0m, Diem2 = 7.5m, Diem3 = 7.2m });
            Console.WriteLine("Đã thêm 5 Đăng Ký mẫu.");

            Console.WriteLine("Đã thêm dữ liệu mẫu thành công!");
        }
    }
}