IF OBJECT_ID('dbo.V_SinhVien', 'V') IS NOT NULL
    DROP VIEW dbo.V_SinhVien;
GO

CREATE VIEW dbo.V_SinhVien AS
SELECT MaSV, HoTen, Phai, MaLop, HocBong FROM CSDLPT_Site1.dbo.SinhVien
UNION ALL
SELECT MaSV, HoTen, Phai, MaLop, HocBong FROM CSDLPT_Site2.dbo.SinhVien
UNION ALL
SELECT MaSV, HoTen, Phai, MaLop, HocBong FROM CSDLPT_Site3.dbo.SinhVien;
GO

IF OBJECT_ID('dbo.TRG_V_SinhVien_InsteadOf', 'TR') IS NOT NULL
    DROP TRIGGER dbo.TRG_V_SinhVien_InsteadOf;
GO

CREATE TRIGGER dbo.TRG_V_SinhVien_InsteadOf
ON dbo.V_SinhVien
INSTEAD OF INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @sql NVARCHAR(MAX);

    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        DECLARE ins_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT MaSV, HoTen, Phai, MaLop, HocBong FROM inserted;

        DECLARE @MaSV CHAR(10), @HoTen NVARCHAR(150), @Phai NVARCHAR(10), @MaLop CHAR(8), @HocBong DECIMAL(10,2);

        OPEN ins_cursor;
        FETCH NEXT FROM ins_cursor INTO @MaSV, @HoTen, @Phai, @MaLop, @HocBong;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @target NVARCHAR(128) =
                CASE WHEN LEFT(ISNULL(@MaLop,''),4) = 'CNTT' THEN 'CSDLPT_Site1'
                     WHEN LEFT(ISNULL(@MaLop,''),2) = 'KT'   THEN 'CSDLPT_Site2'
                     WHEN LEFT(ISNULL(@MaLop,''),4) = 'QTKD' THEN 'CSDLPT_Site3'
                     ELSE 'CSDLPT_Site1' END;

            SET @sql = N'
                INSERT INTO ' + QUOTENAME(@target) + N'.dbo.SinhVien (MaSV, HoTen, Phai, MaLop, HocBong)
                VALUES (@MaSV, @HoTen, @Phai, @MaLop, @HocBong);';

            EXEC sp_executesql @sql,
                N'@MaSV CHAR(10), @HoTen NVARCHAR(150), @Phai NVARCHAR(10), @MaLop CHAR(8), @HocBong DECIMAL(10,2)',
                @MaSV=@MaSV, @HoTen=@HoTen, @Phai=@Phai, @MaLop=@MaLop, @HocBong=@HocBong;

            FETCH NEXT FROM ins_cursor INTO @MaSV, @HoTen, @Phai, @MaLop, @HocBong;
        END
        CLOSE ins_cursor;
        DEALLOCATE ins_cursor;
    END

    IF EXISTS (SELECT 1 FROM inserted INNER JOIN deleted ON inserted.MaSV = deleted.MaSV)
    BEGIN
        SET @sql = N'
            UPDATE S SET S.HoTen = I.HoTen, S.Phai = I.Phai, S.MaLop = I.MaLop, S.HocBong = I.HocBong
            FROM CSDLPT_Site1.dbo.SinhVien S JOIN inserted I ON S.MaSV = I.MaSV;
        ';
        EXEC(@sql);

        SET @sql = N'
            UPDATE S SET S.HoTen = I.HoTen, S.Phai = I.Phai, S.MaLop = I.MaLop, S.HocBong = I.HocBong
            FROM CSDLPT_Site2.dbo.SinhVien S JOIN inserted I ON S.MaSV = I.MaSV;
        ';
        EXEC(@sql);

        SET @sql = N'
            UPDATE S SET S.HoTen = I.HoTen, S.Phai = I.Phai, S.MaLop = I.MaLop, S.HocBong = I.HocBong
            FROM CSDLPT_Site3.dbo.SinhVien S JOIN inserted I ON S.MaSV = I.MaSV;
        ';
        EXEC(@sql);
    END

    IF EXISTS (SELECT 1 FROM deleted)
    BEGIN
        DECLARE del_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT MaSV FROM deleted;

        DECLARE @delMaSV CHAR(10);
        OPEN del_cursor;
        FETCH NEXT FROM del_cursor INTO @delMaSV;
        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @sql = N'DELETE FROM CSDLPT_Site1.dbo.SinhVien WHERE MaSV = @MaSV;';
            EXEC sp_executesql @sql, N'@MaSV CHAR(10)', @MaSV=@delMaSV;

            SET @sql = N'DELETE FROM CSDLPT_Site2.dbo.SinhVien WHERE MaSV = @MaSV;';
            EXEC sp_executesql @sql, N'@MaSV CHAR(10)', @MA SV=@delMaSV;

            SET @sql = N'DELETE FROM CSDLPT_Site3.dbo.SinhVien WHERE MaSV = @MaSV;';
            EXEC sp_executesql @sql, N'@MaSV CHAR(10)', @MaSV=@delMaSV;

            FETCH NEXT FROM del_cursor INTO @delMaSV;
        END
        CLOSE del_cursor;
        DEALLOCATE del_cursor;
    END

END
GO