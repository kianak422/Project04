namespace LopCRUDApp
{
    public class DangKy
    {
        public required string MaSV { get; set; }
        public required string MaMon { get; set; }
        public decimal? Diem1 { get; set; }
        public decimal? Diem2 { get; set; }
        public decimal? Diem3 { get; set; }
        public required string Site { get; set; } // Dùng chuỗi 'Site1', 'Site2', 'Site3'
    }
}