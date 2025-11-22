CREATE VIEW V_DangKy AS
SELECT MaSV, MaMon, Diem1, Diem2, Diem3, 'Site1' AS Site
FROM CSDLPT_Site1.dbo.DangKy
UNION ALL
SELECT MaSV, MaMon, Diem1, Diem2, Diem3, 'Site2' AS Site
FROM CSDLPT_Site2.dbo.DangKy
UNION ALL
SELECT MaSV, MaMon, Diem1, Diem2, Diem3, 'Site3' AS Site
FROM CSDLPT_Site3.dbo.DangKy;
GO

CREATE VIEW V_SinhVien AS
SELECT MaSV, HoTen, Phai, NgaySinh, MaLop, HocBong, 'Site1' AS Site
FROM CSDLPT_Site1.dbo.SinhVien
UNION ALL
SELECT MaSV, HoTen, Phai, NgaySinh, MaLop, HocBong, 'Site2' AS Site
FROM CSDLPT_Site2.dbo.SinhVien
UNION ALL
SELECT MaSV, HoTen, Phai, NgaySinh, MaLop, HocBong, 'Site3' AS Site
FROM CSDLPT_Site3.dbo.SinhVien;
GO

CREATE VIEW V_Lop AS
SELECT MaLop, TenLop, Khoa, 'Site1' AS Site
FROM CSDLPT_Site1.dbo.Lop
UNION ALL
SELECT MaLop, TenLop, Khoa, 'Site2' AS Site
FROM CSDLPT_Site2.dbo.Lop
UNION ALL
SELECT MaLop, TenLop, Khoa, 'Site3' AS Site
FROM CSDLPT_Site3.dbo.Lop;
GO