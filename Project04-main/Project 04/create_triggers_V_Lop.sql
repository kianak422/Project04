USE [master];
GO

CREATE TRIGGER trg_V_Lop_InsteadOfInsert
ON V_Lop
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO CSDLPT_Site1.dbo.Lop (MaLop, TenLop, Khoa)
    SELECT MaLop, TenLop, Khoa
    FROM INSERTED
    WHERE Site = 'Site1';

    INSERT INTO CSDLPT_Site2.dbo.Lop (MaLop, TenLop, Khoa)
    SELECT MaLop, TenLop, Khoa
    FROM INSERTED
    WHERE Site = 'Site2';

    INSERT INTO CSDLPT_Site3.dbo.Lop (MaLop, TenLop, Khoa)
    SELECT MaLop, TenLop, Khoa
    FROM INSERTED
    WHERE Site = 'Site3';
END;
GO

CREATE TRIGGER trg_V_Lop_InsteadOfDelete
ON V_Lop
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DELETE CSDLPT_Site1.dbo.Lop
    FROM DELETED d
    WHERE CSDLPT_Site1.dbo.Lop.MaLop = d.MaLop AND d.Site = 'Site1';

    DELETE CSDLPT_Site2.dbo.Lop
    FROM DELETED d
    WHERE CSDLPT_Site2.dbo.Lop.MaLop = d.MaLop AND d.Site = 'Site2';

    DELETE CSDLPT_Site3.dbo.Lop
    FROM DELETED d
    WHERE CSDLPT_Site3.dbo.Lop.MaLop = d.MaLop AND d.Site = 'Site3';
END;
GO

CREATE TRIGGER trg_V_Lop_InsteadOfUpdate
ON V_Lop
INSTEAD OF UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE CSDLPT_Site1.dbo.Lop
    SET TenLop = i.TenLop, Khoa = i.Khoa
    FROM INSERTED i
    WHERE CSDLPT_Site1.dbo.Lop.MaLop = i.MaLop AND i.Site = 'Site1';

    UPDATE CSDLPT_Site2.dbo.Lop
    SET TenLop = i.TenLop, Khoa = i.Khoa
    FROM INSERTED i
    WHERE CSDLPT_Site2.dbo.Lop.MaLop = i.MaLop AND i.Site = 'Site2';

    UPDATE CSDLPT_Site3.dbo.Lop
    SET TenLop = i.TenLop, Khoa = i.Khoa
    FROM INSERTED i
    WHERE CSDLPT_Site3.dbo.Lop.MaLop = i.MaLop AND i.Site = 'Site3';
END;
GO