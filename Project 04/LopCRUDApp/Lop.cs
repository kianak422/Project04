using System;

namespace LopCRUDApp
{
    public class Lop
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string Khoa { get; set; }
        // Đã sửa thành string để khớp với SQL view
        public string Site { get; set; }
    }
}