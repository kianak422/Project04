using Microsoft.EntityFrameworkCore;

namespace LopCRUDApp
{
    public class LopRepository
    {
        private readonly ApplicationDbContext _context;

        public LopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddLop(Lop lop)
        {
            _context.Lops.Add(lop);
            _context.SaveChanges();
            Console.WriteLine($"Đã thêm lớp {lop.TenLop} vào Site {lop.Site}.");
        }

        public void UpdateLop(Lop lop)
        {
            var existingLop = _context.Lops.FirstOrDefault(l => l.MaLop == lop.MaLop && l.Site == lop.Site);
            if (existingLop != null)
            {
                existingLop.TenLop = lop.TenLop;
                existingLop.Khoa = lop.Khoa;
                _context.SaveChanges();
                Console.WriteLine($"Đã cập nhật lớp {lop.MaLop} trên Site {lop.Site}.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi để cập nhật.");
            }
        }

        // Đã sửa int site thành string site
        public void DeleteLop(string maLop, string site)
        {
            var lop = _context.Lops.FirstOrDefault(l => l.MaLop == maLop && l.Site == site);
            if (lop != null)
            {
                _context.Lops.Remove(lop);
                _context.SaveChanges();
                Console.WriteLine($"Đã xóa lớp {maLop} trên Site {site}.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy bản ghi để xóa.");
            }
        }

        public List<Lop> GetAllLops()
        {
            return _context.Lops.ToList();
        }

        // Đã sửa int site thành string site
        public Lop GetLopByMaLopAndSite(string maLop, string site)
        {
            return _context.Lops.FirstOrDefault(l => l.MaLop == maLop && l.Site == site);
        }

        public void DeleteAllLops()
        {
            _context.Lops.RemoveRange(_context.Lops);
            _context.SaveChanges();
            Console.WriteLine("Đã xóa tất cả các Lớp.");
        }
    }
}