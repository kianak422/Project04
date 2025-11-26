using Microsoft.EntityFrameworkCore;

namespace LopCRUDApp
{
    public class SinhVienRepository
    {
        private readonly ApplicationDbContext _context;

        public SinhVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        public void AddSinhVien(SinhVien sv)
        {
            try
            {
                _context.SinhViens.Add(sv);
                _context.SaveChanges();
                Console.WriteLine($"Đã thêm sinh viên {sv.HoTen} vào Site {sv.Site}.");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine($"Lỗi khi thêm sinh viên: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw; // Re-throw the exception so the API endpoint can catch it
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định khi thêm sinh viên: {ex.Message}");
                throw;
            }
        }

        // UPDATE
        public void UpdateSinhVien(SinhVien sv)
        {
            var existingSinhVien = _context.SinhViens.FirstOrDefault(s => s.MaSV == sv.MaSV && s.Site == sv.Site);
            if (existingSinhVien != null)
            {
                try
                {
                    existingSinhVien.HoTen = sv.HoTen;
                    existingSinhVien.Phai = sv.Phai;
                    existingSinhVien.NgaySinh = sv.NgaySinh;
                    existingSinhVien.MaLop = sv.MaLop;
                    existingSinhVien.HocBong = sv.HocBong;
                    existingSinhVien.Khoa = sv.Khoa;
                    _context.SaveChanges();
                    Console.WriteLine($"Đã cập nhật sinh viên {sv.MaSV} trên Site {sv.Site}.");
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    Console.WriteLine($"Lỗi khi cập nhật sinh viên: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    throw; // Re-throw the exception
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi không xác định khi cập nhật sinh viên: {ex.Message}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi để cập nhật.");
            }
        }

        // DELETE
        public void DeleteSinhVien(string maSV, string site)
        {
            var sinhVien = _context.SinhViens.FirstOrDefault(s => s.MaSV == maSV && s.Site == site);
            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
                _context.SaveChanges();
                Console.WriteLine($"Đã xóa sinh viên {maSV} trên Site {site}.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi để xóa.");
            }
        }

        // READ ALL
        public List<SinhVien> GetAllSinhViens()
        {
            return _context.SinhViens.ToList();
        }

        // READ by MaSV + Site (hàm gốc của bạn)
        public SinhVien? GetSinhVienByMaSVAndSite(string maSV, string site)
        {
            return _context.SinhViens.FirstOrDefault(s => s.MaSV == maSV && s.Site == site);
        }

        // 🔥 HÀM MỚI THÊM VÀO — PHÙ HỢP VỚI Program.cs
        public SinhVien? GetSinhVienById(string maSV, string site)
        {
            return _context.SinhViens.FirstOrDefault(s => s.MaSV == maSV && s.Site == site);
        }

        public void DeleteAllSinhViens()
        {
            _context.SinhViens.RemoveRange(_context.SinhViens);
            _context.SaveChanges();
            Console.WriteLine("Đã xóa tất cả các Sinh Viên.");
        }
    }
}

