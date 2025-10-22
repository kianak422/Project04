using System;

namespace LopCRUDApp
{
    public class Lop
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string Khoa { get; set; }
        public int Site { get; set; } // Thêm trường Site để xác định site của lớp
    }
}