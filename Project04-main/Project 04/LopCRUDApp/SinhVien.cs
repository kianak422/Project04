namespace LopCRUDApp
{
    public class SinhVien
    {
        public required string MaSV { get; set; }
        public required string HoTen { get; set; }
        public required string Phai { get; set; }
        public DateTime NgaySinh { get; set; }
        public required string MaLop { get; set; }
        public decimal HocBong { get; set; }
        public required string Site { get; set; }

        // Thuộc tính bổ sung cho truy vấn
        public required string Khoa { get; set; }
    }
}