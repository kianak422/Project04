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
            LopRepository repository = new LopRepository(ConnectionString);

            while (true)
            {
                Console.WriteLine("\n--- Quản lý Lớp ---");
                Console.WriteLine("1. Thêm Lớp");
                Console.WriteLine("2. Cập nhật Lớp");
                Console.WriteLine("3. Xóa Lớp");
                Console.WriteLine("4. Xem tất cả Lớp");
                Console.WriteLine("5. Tìm Lớp theo Mã Lớp và Site");
                Console.WriteLine("0. Thoát");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddLop(repository);
                        break;
                    case "2":
                        UpdateLop(repository);
                        break;
                    case "3":
                        DeleteLop(repository);
                        break;
                    case "4":
                        ViewAllLops(repository);
                        break;
                    case "5":
                        GetLopByMaLopAndSite(repository);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                        break;
                }
            }
        }

        static void AddLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Tên Lớp: ");
            string tenLop = Console.ReadLine();
            Console.Write("Nhập Khoa: ");
            string khoa = Console.ReadLine();
            Console.Write("Nhập Site (1, 2, hoặc 3): ");
            int site = int.Parse(Console.ReadLine());

            Lop newLop = new Lop { MaLop = maLop, TenLop = tenLop, Khoa = khoa, Site = site };
            repository.AddLop(newLop);
        }

        static void UpdateLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần cập nhật: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Site của Lớp cần cập nhật: ");
            int site = int.Parse(Console.ReadLine());

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
            // Site không được phép thay đổi khi cập nhật

            repository.UpdateLop(existingLop);
        }

        static void DeleteLop(LopRepository repository)
        {
            Console.Write("Nhập Mã Lớp cần xóa: ");
            string maLop = Console.ReadLine();
            Console.Write("Nhập Site của Lớp cần xóa: ");
            int site = int.Parse(Console.ReadLine());

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
            int site = int.Parse(Console.ReadLine());

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
    }
}
