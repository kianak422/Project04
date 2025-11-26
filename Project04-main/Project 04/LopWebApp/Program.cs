using LopCRUDApp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Read connection string from configuration (appsettings.json or environment)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DEFAULT_CONNECTION")
    ?? "Server=.;Database=master;Integrated Security=True;TrustServerCertificate=True;";

builder.Services.Configure<JsonOptions>(opts => {
    opts.SerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<LopRepository>();
builder.Services.AddScoped<SinhVienRepository>();
builder.Services.AddScoped<DangKyRepository>();
builder.Services.AddScoped<QueryRepository>();

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

// CRUD for LOP
app.MapGet("/api/lops", (LopRepository lopRepo) =>
{
    try
    {
        return Results.Ok(lopRepo.GetAllLops());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/lops/{maLop}/{site}", (string maLop, string site, LopRepository lopRepo) =>
{
    try
    {
        var lop = lopRepo.GetLopByMaLopAndSite(maLop, site);
        return lop is null ? Results.NotFound() : Results.Ok(lop);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/lops", (Lop lop, LopRepository lopRepo) =>
{
    try
    {
        lopRepo.AddLop(lop);
        return Results.Created($"/api/lops/{lop.MaLop}/{lop.Site}", lop);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPut("/api/lops/{maLop}/{site}", (string maLop, string site, Lop updated, LopRepository lopRepo) =>
{
    try
    {
        updated.MaLop = maLop;
        updated.Site = site;
        lopRepo.UpdateLop(updated);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/api/lops/{maLop}/{site}", (string maLop, string site, LopRepository lopRepo) =>
{
    try
    {
        lopRepo.DeleteLop(maLop, site);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// CRUD for SINHVIEN
app.MapGet("/api/sinhviens", (SinhVienRepository svRepo) =>
{
    try
    {
        return Results.Ok(svRepo.GetAllSinhViens());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/sinhviens/{maSV}/{site}", (string maSV, string site, SinhVienRepository svRepo) =>
{
    try
    {
        var sv = svRepo.GetSinhVienByMaSVAndSite(maSV, site);
        return sv is null ? Results.NotFound() : Results.Ok(sv);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/sinhviens", (SinhVien sv, SinhVienRepository svRepo) =>
{
    try
    {
        svRepo.AddSinhVien(sv);
        return Results.Created($"/api/sinhviens/{sv.MaSV}/{sv.Site}", sv);
    }
    catch (Exception ex)
    {
        var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        return Results.Problem(errorMessage);
    }
});

app.MapPut("/api/sinhviens/{maSV}/{site}", (string maSV, string site, SinhVien updated, SinhVienRepository svRepo) =>
{
    try
    {
        updated.MaSV = maSV;
        updated.Site = site;
        svRepo.UpdateSinhVien(updated);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        return Results.Problem(errorMessage);
    }
});

app.MapDelete("/api/sinhviens/{maSV}/{site}", (string maSV, string site, SinhVienRepository svRepo) =>
{
    try
    {
        svRepo.DeleteSinhVien(maSV, site);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// Đăng ký endpoints
app.MapPost("/api/dangky", (DangKy dk, DangKyRepository dkRepo) =>
{
    try
    {
        dkRepo.AddDangKy(dk);
        return Results.Created($"/api/dangky/{dk.MaSV}/{dk.MaMon}/{dk.Site}", dk);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPut("/api/dangky/{maSV}/{maMon}/{site}", (string maSV, string maMon, string site, DangKy body, DangKyRepository dkRepo) =>
{
    dkRepo.UpdateDiem(maSV, maMon, site, body.Diem1, body.Diem2, body.Diem3);
    return Results.NoContent();
});

app.MapDelete("/api/dangky/{maSV}/{maMon}/{site}", (string maSV, string maMon, string site, DangKyRepository dkRepo) =>
{
    try
    {
        dkRepo.DeleteDangKy(maSV, maMon, site);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// GET all dang ky
app.MapGet("/api/dangky", (DangKyRepository dkRepo) =>
{
    try
    {
        return Results.Ok(dkRepo.GetAllDangKys());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// GET single dang ky
app.MapGet("/api/dangky/{maSV}/{maMon}/{site}", (string maSV, string maMon, string site, DangKyRepository dkRepo) =>
{
    try
    {
        var dangKy = dkRepo.GetDangKyByMaSVMaMonAndSite(maSV, maMon, site);
        return dangKy is null ? Results.NotFound() : Results.Ok(dangKy);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// Truy vấn endpoints (trả JSON)
app.MapGet("/api/query/khoa/{maSV}", (string maSV, QueryRepository queryRepo) =>
{
    try
    {
        var result = queryRepo.GetKhoaByMaSV(maSV);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/query/diem/{maSV}", (string maSV, QueryRepository queryRepo) =>
{
    try
    {
        return Results.Ok(queryRepo.GetDiemByMaSV(maSV));
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/query/diemmaxkhoa", (QueryRepository queryRepo) =>
{
    try
    {
        return Results.Ok(queryRepo.GetDiemMaxKhoa());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

// Simple health
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

// DB check endpoint - returns counts for key views/tables (or error message)
app.MapGet("/api/check", (ApplicationDbContext dbContext) =>
{
    try
    {
        var result = new Dictionary<string, object>();

        result["Lop_count"] = dbContext.Lops.Count();
        result["SinhVien_count"] = dbContext.SinhViens.Count();
        result["DangKy_count"] = dbContext.DangKys.Count();

        return Results.Ok(new { connectionStringSet = !string.IsNullOrEmpty(connectionString), details = result });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
