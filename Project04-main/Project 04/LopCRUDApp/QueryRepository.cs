using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace LopCRUDApp
{
    public class QueryRepository
    {
        private readonly ApplicationDbContext _context;

        public QueryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Form 1: Khoa của một sinh viên
        public object? GetKhoaByMaSV(string maSV)
        {
            var result = (from sv in _context.SinhViens
                          join l in _context.Lops on new { sv.MaLop, sv.Site } equals new { l.MaLop, l.Site }
                          where sv.MaSV == maSV
                          select new
                          {
                              sv.MaSV,
                              sv.HoTen,
                              l.Khoa
                          }).FirstOrDefault();

            return result;
        }

        // Form 2: Điểm của sinh viên trong tất cả các môn học
        public object GetDiemByMaSV(string maSV)
        {
            var results = _context.DangKys
                .Where(dk => dk.MaSV == maSV)
                .Select(dk => new
                {
                    dk.MaMon,
                    dk.Diem1,
                    dk.Diem2,
                    dk.Diem3,
                    DiemTB = (dk.Diem1.GetValueOrDefault(0) + dk.Diem2.GetValueOrDefault(0) + dk.Diem3.GetValueOrDefault(0)) /
                             (new[] { dk.Diem1, dk.Diem2, dk.Diem3 }.Count(d => d.HasValue) == 0 ? 1 : new[] { dk.Diem1, dk.Diem2, dk.Diem3 }.Count(d => d.HasValue))
                }).ToList();

            return results;
        }

        // Form 3: Trung bình điểm cao nhất của mỗi khoa
        public object GetDiemMaxKhoa()
        {
            var result = (from dk in _context.DangKys
                          join sv in _context.SinhViens on dk.MaSV equals sv.MaSV
                          join l in _context.Lops on new { sv.MaLop, sv.Site } equals new { l.MaLop, l.Site }
                          group new { dk, l } by l.Khoa into g
                          select new
                          {
                              Khoa = g.Key,
                              DiemTBMax = g.Average(x => (x.dk.Diem1.GetValueOrDefault(0) + x.dk.Diem2.GetValueOrDefault(0) + x.dk.Diem3.GetValueOrDefault(0)) /
                                                          (new[] { x.dk.Diem1, x.dk.Diem2, x.dk.Diem3 }.Count(d => d.HasValue) == 0 ? 1 : new[] { x.dk.Diem1, x.dk.Diem2, x.dk.Diem3 }.Count(d => d.HasValue)))
                          }).OrderBy(x => x.Khoa).ToList();

            return result;
        }
    }
}