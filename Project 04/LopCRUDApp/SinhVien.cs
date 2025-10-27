namespace LopCRUDApp
{
    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string Phai { get; set; }
        public DateTime NgaySinh { get; set; }
        public string MaLop { get; set; }
        public decimal HocBong { get; set; }
        public string Site { get; set; }

        // Thuộc tính bổ sung cho truy vấn
        public string Khoa { get; set; }
    }
}