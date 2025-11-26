using System;

namespace LopCRUDApp
{
    public class Lop
    {
        public required string MaLop { get; set; }
        public required string TenLop { get; set; }
        public required string Khoa { get; set; }
        // Đã sửa thành string để khớp với SQL view
        public required string Site { get; set; }
    }
}