# 📚 Hướng Dẫn Chi Tiết Clean Architecture

## 📖 Mục Lục
1. [Tổng Quan Clean Architecture](#1-tổng-quan-clean-architecture)
2. [Domain Layer (Core)](#2-domain-layer-core)
3. [Application Layer](#3-application-layer)
4. [Infrastructure Layer](#4-infrastructure-layer)
5. [API Layer (Presentation)](#5-api-layer-presentation)
6. [Luồng Dữ Liệu](#6-luồng-dữ-liệu)
7. [Dependency Injection](#7-dependency-injection)

---

## 1. Tổng Quan Clean Architecture

### 🎯 Clean Architecture là gì?
Clean Architecture là một kiến trúc phần mềm được đề xuất bởi Robert C. Martin (Uncle Bob), nhằm tạo ra các hệ thống:
- **Độc lập với Framework**: Kiến trúc không phụ thuộc vào framework cụ thể
- **Có thể test được**: Business logic có thể test mà không cần UI, Database, hay bất kỳ external element nào
- **Độc lập với UI**: UI có thể thay đổi dễ dàng mà không ảnh hưởng đến business logic
- **Độc lập với Database**: Business logic không bị ràng buộc với database cụ thể
- **Độc lập với External Services**: Business logic không biết gì về thế giới bên ngoài

### 📊 Sơ Đồ Các Tầng

```
┌─────────────────────────────────────────────────────────────────┐
│                        API Layer                                │
│              (Controllers, Middleware, Filters)                 │
├─────────────────────────────────────────────────────────────────┤
│                    Infrastructure Layer                         │
│        (Database, External Services, Repositories)              │
├─────────────────────────────────────────────────────────────────┤
│                     Application Layer                           │
│           (Services, DTOs, Interfaces, Use Cases)               │
├─────────────────────────────────────────────────────────────────┤
│                       Domain Layer                              │
│              (Entities, Enums, Constants, Value Objects)        │
└─────────────────────────────────────────────────────────────────┘
```

### 📁 Cấu Trúc Thư Mục Dự Án

```
clean-architecture/
├── Api/                    # Presentation Layer
├── Application/            # Application Layer  
├── Domain/                 # Domain Layer (Core)
├── Infrastructure/         # Infrastructure Layer
└── CleanArchitecture.sln   # Solution file
```

---

## 2. Domain Layer (Core)

### 📌 Mô Tả
**Domain Layer** là tầng trung tâm và quan trọng nhất của Clean Architecture. Tầng này chứa:
- **Entities**: Các đối tượng nghiệp vụ chính
- **Enums**: Các kiểu liệt kê
- **Constants**: Các hằng số
- **Value Objects**: Các đối tượng giá trị
- **Domain Events**: Các sự kiện trong domain

> ⚠️ **Nguyên tắc quan trọng**: Domain Layer **KHÔNG** phụ thuộc vào bất kỳ tầng nào khác!

### 📁 Cấu Trúc Thư Mục

```
Domain/
├── Core.csproj
├── Entities/
│   └── Product.cs
├── Enums/
│   └── ProductStatus.cs
└── Constants/
    └── MessageConstant.cs
```

### 📄 Chi Tiết Từng File

#### 2.1 Entities/Product.cs
```csharp
using Domain.Enums;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
    }
}
```

**Giải thích:**
| Thuộc tính | Kiểu dữ liệu | Mô tả |
|------------|--------------|-------|
| `Id` | `int` | Khóa chính, định danh duy nhất |
| `Name` | `string` | Tên sản phẩm |
| `Price` | `decimal` | Giá sản phẩm (dùng decimal cho tiền tệ) |
| `Status` | `ProductStatus` | Trạng thái sản phẩm (enum) |

#### 2.2 Enums/ProductStatus.cs
```csharp
namespace Domain.Enums
{
    public enum ProductStatus
    {
        Active,      // Đang hoạt động
        Inactive,    // Không hoạt động
        OutOfStock   // Hết hàng
    }
}
```

**Giải thích:**
- **Active**: Sản phẩm đang được bán
- **Inactive**: Sản phẩm tạm ngưng
- **OutOfStock**: Sản phẩm hết hàng

#### 2.3 Constants/MessageConstant.cs
```csharp
namespace Domain.Constants
{
    public static class MessageConstant
    {
        public static class CommonMessage
        {
            public const string UNAUTHORIZED = "Common_401";           // Chưa xác thực
            public const string ACCESS_DENIED = "Common_403";          // Không có quyền
            public const string NOT_FOUND = "Common_404";              // Không tìm thấy
            public const string INTERNAL_SERVER_ERROR = "Common_500";  // Lỗi server
            public const string MISSING_PARAM = "Common_501";          // Thiếu tham số
        }
    }
}
```

**Giải thích:**
- Sử dụng `static class` để không cần khởi tạo
- Định nghĩa các mã lỗi theo chuẩn HTTP status code
- Dễ dàng quản lý và tái sử dụng trong toàn bộ ứng dụng

---

## 3. Application Layer

### 📌 Mô Tả
**Application Layer** chứa business logic của ứng dụng. Tầng này:
- Định nghĩa các **Interfaces** cho repositories và services
- Chứa các **Services** xử lý nghiệp vụ
- Định nghĩa các **DTOs** (Data Transfer Objects)
- Phụ thuộc vào **Domain Layer**

### 📁 Cấu Trúc Thư Mục

```
Application/
├── Application.csproj
├── DTOs/
│   ├── Common/
│   │   ├── BaseQueryDTO.cs
│   │   ├── BaseRequestDTO.cs
│   │   └── BaseResponseDTO.cs
│   └── Config/
│       └── ExternalServiceDTO.cs
├── IRepositories/
│   └── IProductRepository.cs
├── IServices/
│   ├── IProductService.cs
│   └── ExternalServices/
│       └── IService1.cs
└── Services/
    └── ProductService.cs
```

### 📄 Chi Tiết Từng File

#### 3.1 DTOs/Common/BaseResponseDTO.cs
```csharp
namespace Application.DTOs.Common
{
    public class BaseResponseDTO<T>
    {
        public int Code { get; set; } = 0;
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }
        public MetaDataDTO? MetaData { get; set; }

        // Factory method cho response thành công
        public static BaseResponseDTO<T> SuccessResponse(T data, MetaDataDTO? meta = null, 
            string? message = null, int code = 200)
            => new() { Data = data, MetaData = meta, Message = message, Code = code, Success = true };

        // Factory method cho response thất bại
        public static BaseResponseDTO<T> FailResponse(string message, int code = 500)
            => new() { Message = message, Code = code, Success = false };
    }

    public class MetaDataDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int Total { get; set; } = 0;
        public int TotalPage => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)Total / PageSize);
    }
}
```

**Giải thích:**
| Thuộc tính | Mô tả |
|------------|-------|
| `Code` | HTTP status code |
| `Success` | Trạng thái thành công/thất bại |
| `Message` | Thông báo trả về |
| `Data` | Dữ liệu trả về (generic type) |
| `MetaData` | Thông tin phân trang |

**Factory Methods:**
- `SuccessResponse()`: Tạo response thành công
- `FailResponse()`: Tạo response thất bại

#### 3.2 DTOs/Common/BaseRequestDTO.cs
```csharp
namespace Application.DTOs.Common
{
    public class BaseRequestDTO
    {
        public int ActionBy { get; set; }      // ID người thực hiện
        public int LanguageKey { get; set; }   // Ngôn ngữ
        public bool IsAdmin { get; set; }      // Có phải admin không
    }

    public class BaseRequestDTO<T> : BaseRequestDTO
    {
        public T? Request { get; set; }        // Dữ liệu request cụ thể
    }
}
```

**Giải thích:**
- `BaseRequestDTO`: Class cơ sở cho mọi request
- `BaseRequestDTO<T>`: Generic version, cho phép gửi kèm dữ liệu cụ thể

#### 3.3 DTOs/Common/BaseQueryDTO.cs
```csharp
using System.Text.Json.Serialization;

namespace Application.DTOs.Common
{
    public class BaseQueryDTO : BaseRequestDTO
    {
        public string? Keyword { get; set; }           // Từ khóa tìm kiếm
        public int Page { get; set; } = 1;             // Trang hiện tại
        public int PageSize { get; set; } = 20;        // Số item/trang

        [JsonIgnore]
        public bool IsGetAll { get; set; } = false;    // Lấy tất cả (không phân trang)
    }

    public class BaseQueryDTO<T> : BaseQueryDTO
    {
        public T? Query { get; set; }                  // Query parameters cụ thể
    }
}
```

**Giải thích:**
- Kế thừa từ `BaseRequestDTO`
- Hỗ trợ phân trang với `Page` và `PageSize`
- `[JsonIgnore]` để ẩn `IsGetAll` khỏi JSON response

#### 3.4 DTOs/Config/ExternalServiceDTO.cs
```csharp
namespace Application.DTOs.Config
{
    public class ExternalServiceDTO
    {
        public string Url { get; set; }    // URL của external service
        public string Token { get; set; }  // Token xác thực
    }
}
```

#### 3.5 IRepositories/IProductRepository.cs
```csharp
using Domain.Entities;

namespace Application.IRepository
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);      // Lấy product theo ID
        Task<List<Product>> GetAllAsync();        // Lấy tất cả products
        Task AddAsync(Product product);           // Thêm product mới
    }
}
```

**Giải thích:**
- **Interface** định nghĩa contract cho repository
- Sử dụng `Task` cho async operations
- `Product?` cho phép trả về null khi không tìm thấy

#### 3.6 IServices/IProductService.cs
```csharp
using Domain.Entities;

namespace Application.Services
{
    public interface IProductService
    {
        Task<Product?> GetProductAsync(int id);
        Task<List<Product>> GetAllProductsAsync();
        Task AddProductAsync(string name, decimal price);
    }
}
```

#### 3.7 IServices/ExternalServices/IService1.cs
```csharp
using Domain.Entities;

namespace Application.IServices.ExternalServices
{
    public interface IService1
    {
        Task<List<Product>> GetAllProducts();
    }
}
```

**Giải thích:**
- Interface cho external service
- Được implement ở Infrastructure Layer

#### 3.8 Services/ProductService.cs
```csharp
using Application.IRepository;
using Application.IServices.ExternalServices;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using static Domain.Constants.MessageConstant;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _repository;
        private readonly IService1 _service1;

        // Constructor Injection
        public ProductService(ILogger<ProductService> logger, 
            IProductRepository repository, IService1 service1)
        {
            _logger = logger;
            _repository = repository;
            _service1 = service1;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            _logger.LogError($"GetProductAsync id: {id}");
            var result = await _repository.GetByIdAsync(id);
            
            if (result == null) 
                throw new KeyNotFoundException(CommonMessage.NOT_FOUND);
            
            return result;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _service1.GetAllProducts();
            // hoặc: return await _repository.GetAllAsync();
        }

        public async Task AddProductAsync(string name, decimal price)
        {
            var product = new Product
            {
                Name = name,
                Price = price,
                Status = Domain.Enums.ProductStatus.Active
            };
            await _repository.AddAsync(product);
        }
    }
}
```

**Giải thích:**
| Thành phần | Mô tả |
|------------|-------|
| `ILogger` | Dependency injection cho logging |
| `IProductRepository` | Repository để truy cập database |
| `IService1` | External service |
| Constructor | Nhận dependencies thông qua DI |

---

## 4. Infrastructure Layer

### 📌 Mô Tả
**Infrastructure Layer** chứa các implementation cụ thể cho:
- **Database Context**: Entity Framework DbContext
- **Repositories**: Triển khai cụ thể của các repository interfaces
- **External Services**: Gọi API bên ngoài
- **Configurations**: Cấu hình Entity Framework
- **Migrations**: Database migrations

### 📁 Cấu Trúc Thư Mục

```
Infrastructure/
├── Infrastructure.csproj
├── AppSettings.cs
├── DependencyInjection.cs
├── Persistence/
│   ├── AppDbContext.cs
│   └── Configurations/
│       └── ProductConfiguration.cs
├── Repositories/
│   └── ProductRepository.cs
├── ExternalServices/
│   └── Service1.cs
└── Migrations/
    ├── 20251021065111_InitDB.cs
    └── AppDbContextModelSnapshot.cs
```

### 📄 Chi Tiết Từng File

#### 4.1 AppSettings.cs
```csharp
using Application.DTOs.Config;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public static class AppSettings
    {
        private static IConfiguration _configuration;
        private static readonly Dictionary<string, object> _cache = new();

        // Khởi tạo configuration
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Đọc InternalToken từ appsettings.json
        public static string InternalToken => 
            _configuration.GetSection("InternalToken").Get<string>();
        
        // Đọc cấu hình External Service
        public static ExternalServiceDTO Service1 => 
            _configuration.GetSection("ExternalServices:Service1").Get<ExternalServiceDTO>();
    }
}
```

**Giải thích:**
- **Static class** để truy cập configuration từ bất kỳ đâu
- `Initialize()`: Được gọi trong `Program.cs` để khởi tạo
- Sử dụng `IConfiguration` để đọc từ `appsettings.json`

#### 4.2 DependencyInjection.cs
```csharp
using Application.IRepository;
using Application.IServices.ExternalServices;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, string connectionString)
        {
            // Đăng ký DbContext với PostgreSQL
            services.AddDbContext<AppDbContext>(opt => 
                opt.UseNpgsql(connectionString));

            // Đăng ký Repository
            services.AddScoped<IProductRepository, ProductRepository>();

            // Đăng ký External Service
            services.AddScoped<IService1, Service1>();

            return services;
        }
    }
}
```

**Giải thích:**
- **Extension method** cho `IServiceCollection`
- Đăng ký tất cả dependencies của Infrastructure layer
- `AddScoped`: Tạo instance mới cho mỗi request

#### 4.3 Persistence/AppDbContext.cs
```csharp
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet cho Product entity
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tự động apply tất cả configurations trong assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
```

**Giải thích:**
| Thành phần | Mô tả |
|------------|-------|
| `DbContext` | Base class của Entity Framework |
| `DbSet<Product>` | Đại diện cho bảng Products trong database |
| `OnModelCreating` | Cấu hình model khi tạo database |
| `ApplyConfigurationsFromAssembly` | Tự động load tất cả configurations |

#### 4.4 Persistence/Configurations/ProductConfiguration.cs
```csharp
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Định nghĩa khóa chính
            builder.HasKey(p => p.Id);

            // Cấu hình thuộc tính Name
            builder.Property(p => p.Name)
                .IsRequired()           // NOT NULL
                .HasMaxLength(200);     // VARCHAR(200)
        }
    }
}
```

**Giải thích:**
- **Fluent API** để cấu hình Entity
- Tách riêng configuration cho từng entity
- Dễ maintain và mở rộng

#### 4.5 Repositories/ProductRepository.cs
```csharp
using Application.IRepository;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lấy product theo ID
        public async Task<Product?> GetByIdAsync(int id) =>
            await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        // Lấy tất cả products
        public async Task<List<Product>> GetAllAsync() =>
            await _context.Products.ToListAsync();

        // Thêm product mới
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
    }
}
```

**Giải thích:**
- **Implement** interface `IProductRepository`
- Sử dụng Entity Framework Core
- Các method đều là **async** để tối ưu performance

#### 4.6 ExternalServices/Service1.cs
```csharp
using Application.DTOs.Common;
using Application.IServices.ExternalServices;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.ExternalServices
{
    public class Service1 : IService1
    {
        private readonly ILogger<Service1> _logger;

        public Service1(ILogger<Service1> logger)
        {
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                // Lấy URL từ AppSettings
                var url = $"{AppSettings.Service1.Url}/product/getall";

                using (var client = new HttpClient())
                {
                    // Thêm token vào header
                    client.DefaultRequestHeaders.Add("Token", AppSettings.Service1.Token);

                    // Gọi API
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Deserialize response
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BaseResponseDTO<List<Product>>>(content);

                    return result?.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
    }
}
```

**Giải thích:**
- Gọi External API sử dụng `HttpClient`
- Đọc cấu hình từ `AppSettings`
- Xử lý exception và logging

---

## 5. API Layer (Presentation)

### 📌 Mô Tả
**API Layer** là tầng trên cùng, chịu trách nhiệm:
- Nhận và xử lý HTTP requests
- Định nghĩa các **Controllers**
- Xử lý **Authentication/Authorization**
- Cấu hình **Middleware**
- Khởi tạo ứng dụng

### 📁 Cấu Trúc Thư Mục

```
Api/
├── Api.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── Dockerfile
├── Controllers/
│   ├── BaseController.cs
│   ├── ProductsController.cs
│   └── InternalController.cs
├── Attributes/
│   └── InternalAuthorize.cs
└── Properties/
    └── launchSettings.json
```

### 📄 Chi Tiết Từng File

#### 5.1 Program.cs
```csharp
using Application.IRepository;
using Application.Services;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// === CẤU HÌNH SERILOG ===
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// === ĐĂNG KÝ INFRASTRUCTURE ===
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection"));

// === DEPENDENCY INJECTION ===
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// === CẤU HÌNH CONTROLLERS & SWAGGER ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// === KHỞI TẠO APPSETTINGS ===
AppSettings.Initialize(builder.Configuration);

var app = builder.Build();

// === KHỞI TẠO DATABASE ===
await Init(app);

// === CONFIGURE PIPELINE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Log.Logger.Error($"Service Started");
app.Run();

// === HELPER METHOD ===
async Task Init(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Uncomment để auto migrate
        // if (db.Database.GetPendingMigrations().Any())
        //     db.Database.Migrate();
    }
}
```

**Giải thích:**
| Phần | Mô tả |
|------|-------|
| Serilog | Cấu hình logging |
| AddInfrastructure | Đăng ký Infrastructure services |
| DI | Đăng ký repositories và services |
| Swagger | Tạo API documentation |
| Init | Khởi tạo database |

#### 5.2 Controllers/BaseController.cs
```csharp
using Application.DTOs.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Domain.Constants.MessageConstant;

namespace Api.Controllers
{
    public class BaseController : ControllerBase
    {
        // Wrapper method xử lý exception
        public async Task<BaseResponseDTO<T>> HandleException<T>(Task<T> task)
        {
            try
            {
                var data = await task;
                return BaseResponseDTO<T>.SuccessResponse(data);
            }
            catch (ApplicationException ex)
            {
                return BaseResponseDTO<T>.FailResponse(ex.Message, 200);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BaseResponseDTO<T>.FailResponse(ex.Message, 401);
            }
            catch (KeyNotFoundException ex)
            {
                return BaseResponseDTO<T>.FailResponse(ex.Message, 404);
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"Failed: {ex.Message}\n{ex.StackTrace}");
                return BaseResponseDTO<T>.FailResponse(CommonMessage.INTERNAL_SERVER_ERROR, 500);
            }
        }
    }
}
```

**Giải thích:**
- **Base class** cho tất cả controllers
- `HandleException<T>`: Generic method xử lý exception
- Map exception types sang HTTP status codes

| Exception | HTTP Code |
|-----------|-----------|
| `ApplicationException` | 200 |
| `UnauthorizedAccessException` | 401 |
| `KeyNotFoundException` | 404 |
| `Exception` | 500 |

#### 5.3 Controllers/ProductsController.cs
```csharp
using Application.DTOs.Common;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _service.GetProductAsync(id);
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BaseResponseDTO model)
        {
            await _service.AddProductAsync(name, price);
            return Ok();
        }
    }
}
```

**Giải thích:**
| Attribute | Mô tả |
|-----------|-------|
| `[ApiController]` | Đánh dấu là API controller |
| `[Route("api/[controller]")]` | Route prefix (api/products) |
| `[HttpGet]` | HTTP GET method |
| `[HttpPost]` | HTTP POST method |
| `[FromBody]` | Bind data từ request body |

#### 5.4 Controllers/InternalController.cs
```csharp
using Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternalController : BaseController
    {
        [HttpGet]
        [InternalAuthorize]  // Custom authorization
        public IActionResult GetSecretData()
        {
            return Ok("Success - valid internal token!");
        }
    }
}
```

**Giải thích:**
- Controller cho internal APIs
- Sử dụng custom `[InternalAuthorize]` attribute

#### 5.5 Attributes/InternalAuthorize.cs
```csharp
using Application.DTOs.Common;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class InternalAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;
            string? incomingToken = null;

            // Đọc token từ header
            if (request.Headers.TryGetValue("Token", out var tokenHeader))
            {
                incomingToken = tokenHeader.FirstOrDefault();
            }

            // Kiểm tra token
            if (string.IsNullOrEmpty(incomingToken))
            {
                throw new ApplicationException("Missing Token!");
            }

            // Validate token
            if (incomingToken != AppSettings.InternalToken)
            {
                throw new ApplicationException("Invalid Token!");
            }
        }
    }
}
```

**Giải thích:**
- **Custom Authorization Attribute**
- Implement `IAuthorizationFilter`
- Kiểm tra token trong request header
- So sánh với token cấu hình trong `AppSettings`

---

## 6. Luồng Dữ Liệu

### 🔄 Request Flow

```
┌──────────┐    ┌────────────┐    ┌─────────────┐    ┌──────────────┐    ┌──────────┐
│  Client  │ -> │ Controller │ -> │   Service   │ -> │  Repository  │ -> │ Database │
└──────────┘    └────────────┘    └─────────────┘    └──────────────┘    └──────────┘
                     API              Application        Infrastructure       
```

### 📝 Ví Dụ: Get Product by ID

```
1. Client gửi GET request đến: /api/products/1

2. ProductsController nhận request
   -> Gọi _service.GetProductAsync(1)

3. ProductService xử lý
   -> Gọi _repository.GetByIdAsync(1)
   -> Validate kết quả
   -> Throw exception nếu không tìm thấy

4. ProductRepository truy vấn database
   -> _context.Products.FirstOrDefaultAsync(p => p.Id == 1)
   -> Trả về Product entity

5. Response được trả về qua các tầng
   -> Repository -> Service -> Controller -> Client
```

---

## 7. Dependency Injection

### 📊 Sơ Đồ DI

```
┌─────────────────────────────────────────────────────────────┐
│                        Program.cs                           │
│                                                             │
│  services.AddScoped<IProductRepository, ProductRepository>  │
│  services.AddScoped<IProductService, ProductService>        │
│  services.AddScoped<IService1, Service1>                    │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    DI Container                             │
│                                                             │
│  ┌─────────────────┐    ┌──────────────────┐               │
│  │ IProductService │ -> │  ProductService  │               │
│  └─────────────────┘    └──────────────────┘               │
│           │                      │                          │
│           │              ┌───────┴───────┐                  │
│           │              ▼               ▼                  │
│  ┌────────────────────┐  ┌─────────────────┐               │
│  │ IProductRepository │  │    IService1    │               │
│  └────────────────────┘  └─────────────────┘               │
│           │                      │                          │
│           ▼                      ▼                          │
│  ┌────────────────────┐  ┌─────────────────┐               │
│  │ ProductRepository  │  │    Service1     │               │
│  └────────────────────┘  └─────────────────┘               │
└─────────────────────────────────────────────────────────────┘
```

### 🔑 Service Lifetimes

| Lifetime | Mô tả | Sử dụng |
|----------|-------|---------|
| `Scoped` | Tạo instance mới cho mỗi request | Repositories, Services |
| `Transient` | Tạo instance mới mỗi khi inject | Lightweight services |
| `Singleton` | Tạo 1 instance duy nhất | Configuration, Caching |

---

## 📚 Tổng Kết

### ✅ Lợi ích của Clean Architecture

1. **Separation of Concerns**: Mỗi tầng có trách nhiệm riêng
2. **Testability**: Dễ dàng viết unit tests
3. **Maintainability**: Dễ bảo trì và mở rộng
4. **Flexibility**: Dễ thay đổi database, framework
5. **Independence**: Các tầng độc lập với nhau

### 📌 Nguyên Tắc Quan Trọng

- **Domain** không phụ thuộc vào bất kỳ tầng nào
- **Application** chỉ phụ thuộc vào Domain
- **Infrastructure** implement các interfaces từ Application
- **API** là entry point, phụ thuộc vào tất cả các tầng

### 🎯 Best Practices

1. Luôn sử dụng **interfaces** cho dependencies
2. Sử dụng **Dependency Injection** 
3. Tách **DTOs** riêng cho từng use case
4. Đặt **business logic** trong Application layer
5. Giữ **Controllers** thin (chỉ xử lý HTTP)
6. Sử dụng **async/await** cho database operations
