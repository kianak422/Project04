using Microsoft.EntityFrameworkCore;

namespace LopCRUDApp
{
    public class DangKyRepository
    {
        private readonly ApplicationDbContext _context;

        public DangKyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DangKy> GetAllDangKys()
        {
            return _context.DangKys.ToList();
        }

        public void AddDangKy(DangKy dk)
        {
            _context.DangKys.Add(dk);
            _context.SaveChanges();
            Console.WriteLine($"Đã thêm đăng ký cho SV {dk.MaSV} - Môn {dk.MaMon} vào {dk.Site}.");
        }

        public DangKy? GetDangKyByMaSVMaMonAndSite(string maSV, string maMon, string site)
        {
            return _context.DangKys.FirstOrDefault(dk => dk.MaSV == maSV && dk.MaMon == maMon && dk.Site == site);
        }

        public void UpdateDiem(string maSV, string maMon, string site, decimal? d1, decimal? d2, decimal? d3)
        {
            var dangKy = _context.DangKys.FirstOrDefault(dk => dk.MaSV == maSV && dk.MaMon == maMon && dk.Site == site);
            if (dangKy != null)
            {
                dangKy.Diem1 = d1;
                dangKy.Diem2 = d2;
                dangKy.Diem3 = d3;
                _context.SaveChanges();
                Console.WriteLine($"Đã cập nhật điểm cho SV {maSV} - Môn {maMon} tại {site}.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi để cập nhật.");
            }
        }

        public void DeleteDangKy(string maSV, string maMon, string site)
        {
            var dangKy = _context.DangKys.FirstOrDefault(dk => dk.MaSV == maSV && dk.MaMon == maMon && dk.Site == site);
            if (dangKy != null)
            {
                _context.DangKys.Remove(dangKy);
                _context.SaveChanges();
                Console.WriteLine($"Đã xóa đăng ký cho SV {maSV} - Môn {maMon} tại {site}.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi để xóa.");
            }
        }

        public void DeleteAllDangKys()
        {
            _context.DangKys.RemoveRange(_context.DangKys);
            _context.SaveChanges();
            Console.WriteLine("Đã xóa tất cả các Đăng Ký.");
        }
    }
}