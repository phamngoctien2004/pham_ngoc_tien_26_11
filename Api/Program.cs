using Api.Extentions;
using Api.Middlewares;
using Application.IRepository;
using Application.IServices;
using Application.Mappers;
using Application.Services;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Đọc cấu hình từ appsettings.json
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

// Gắn Serilog vào host
builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

// Dependency Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUploadService, UploadService>();


builder.Services.AddAutoMapper(cfg =>
{
	cfg.AddProfile<ProductMapper>();
    cfg.AddProfile<CategoryMapper>();
    cfg.AddProfile<UserMapper>();

}); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfig();

AppSettings.Initialize(builder.Configuration);

var a = AppSettings.InternalToken;
var a1 = AppSettings.Service1;

builder.Services.AddExceptionHandler<BussinessExceptionHandler>(); // Đăng ký handler global
builder.Services.AddProblemDetails();
builder.Services.AddIdentityService(builder.Configuration); // Đăng ký dịch vụ xác thực và ủy quyền
var app = builder.Build();

await Init(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

Log.Logger.Information($"Service Started");

app.Run();


async Task Init(WebApplication app)
{
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	}
}