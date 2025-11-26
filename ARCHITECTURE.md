# ?? KI?N TRÚC D? ÁN - CLEAN ARCHITECTURE (.NET 8)

## ?? T?ng quan ki?n trúc

D? án này s? d?ng **Clean Architecture** (Ki?n trúc s?ch), t??ng t? nh? cách t? ch?c d? án Spring Boot theo các layer. ?ây là so sánh cho ng??i chuy?n t? Java Spring Boot sang .NET:

| .NET Clean Architecture | Java Spring Boot |
|------------------------|------------------|
| **Domain** (Core) | Domain / Entities |
| **Application** | Service / UseCase Layer |
| **Infrastructure** | Repository / DAO / External Services |
| **Api** | Controller / REST API |

---

## ??? C?u trúc Solution

```
clean-architecture/
??? Domain/                    # Core project - Domain Layer
??? Application/               # Application Layer - Business Logic
??? Infrastructure/            # Infrastructure Layer - Database, External Services
??? Api/                      # Presentation Layer - REST API
```

---

## ?? 1. DOMAIN LAYER (Core Project)

**T??ng ???ng:** Entity/Domain trong Spring Boot

**Ch?c n?ng:** Ch?a logic nghi?p v? c?t lõi, không ph? thu?c vào b?t k? layer nào

### ?? C?u trúc th? m?c
```
Domain/
??? Entities/          # Entity classes (nh? @Entity trong Spring)
?   ??? Product.cs
??? Enums/             # Enum types
?   ??? ProductStatus.cs
??? Constants/         # Constants
    ??? MessageConstant.cs
```

### ?? Dependencies
```xml
<ItemGroup>
  <PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
</ItemGroup>
```

**?? Không ph? thu?c vào project nào** (hoàn toàn ??c l?p)

### ?? Ví d? Entity

**.NET:**
```csharp
public class Product
{
    public int Id { get; set; }          // Nh? @Id trong JPA
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
}
```

**So sánh v?i Spring Boot:**
```java
@Entity
@Table(name = "products")
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    private String name;
    private BigDecimal price;
    private ProductStatus status;
}
```

---

## ?? 2. APPLICATION LAYER

**T??ng ???ng:** Service Layer + Interface trong Spring Boot

**Ch?c n?ng:** Ch?a business logic, ??nh ngh?a interface cho repository và service

### ?? C?u trúc th? m?c
```
Application/
??? IRepositories/         # Repository interfaces (nh? Repository interface trong Spring)
?   ??? IProductRepository.cs
??? IServices/             # Service interfaces
?   ??? IProductService.cs
?   ??? ExternalServices/
?       ??? IService1.cs
??? Services/              # Service implementations (nh? @Service trong Spring)
?   ??? ProductService.cs
??? DTOs/                  # Data Transfer Objects (nh? DTO trong Spring)
    ??? Common/
    ?   ??? BaseRequestDTO.cs
    ?   ??? BaseResponseDTO.cs
    ?   ??? BaseQueryDTO.cs
    ??? Config/
        ??? ExternalServiceDTO.cs
```

### ?? Dependencies
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.2" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\Domain\Core.csproj" />
</ItemGroup>
```

**Dependencies:** Ch? ph? thu?c vào `Domain`

### ?? Ví d? Repository Interface

**.NET:**
```csharp
namespace Application.IRepository
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task AddAsync(Product product);
    }
}
```

**So sánh v?i Spring Boot:**
```java
public interface IProductRepository extends JpaRepository<Product, Integer> {
    Optional<Product> findById(Integer id);
    List<Product> findAll();
    Product save(Product product);
}
```

### ?? Ví d? Service

**.NET:**
```csharp
// Interface
public interface IProductService
{
    Task<Product?> GetProductAsync(int id);
    Task<List<Product>> GetAllProductsAsync();
    Task AddProductAsync(string name, decimal price);
}

// Implementation
public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IProductRepository _repository;
    private readonly IService1 _service1;
    
    // Constructor Injection (nh? @Autowired trong Spring)
    public ProductService(
        ILogger<ProductService> logger, 
        IProductRepository repository,
        IService1 service1)
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
    }
    
    public async Task AddProductAsync(string name, decimal price)
    {
        var product = new Product
        {
            Name = name,
            Price = price,
            Status = ProductStatus.Active
        };
        await _repository.AddAsync(product);
    }
}
```

**So sánh v?i Spring Boot:**
```java
@Service
public class ProductService {
    @Autowired
    private IProductRepository repository;
    
    @Autowired
    private Logger logger;
    
    public Product getProductById(Integer id) {
        logger.error("GetProductAsync id: " + id);
        
        return repository.findById(id)
            .orElseThrow(() -> new NotFoundException("Not found"));
    }
    
    public List<Product> getAllProducts() {
        return repository.findAll();
    }
    
    public void addProduct(String name, BigDecimal price) {
        Product product = new Product();
        product.setName(name);
        product.setPrice(price);
        product.setStatus(ProductStatus.ACTIVE);
        repository.save(product);
    }
}
```

---

## ?? 3. INFRASTRUCTURE LAYER

**T??ng ???ng:** Repository Implementation + JPA Configuration + External Services trong Spring Boot

**Ch?c n?ng:** Implement các interface t? Application layer, x? lý database, external API

### ?? C?u trúc th? m?c
```
Infrastructure/
??? Repositories/          # Repository implementations
?   ??? ProductRepository.cs
??? Persistence/           # Database context (nh? JPA Configuration)
?   ??? AppDbContext.cs   # DbContext (nh? EntityManager)
?   ??? Configurations/   # Entity configurations (nh? @EntityListeners)
?       ??? ProductConfiguration.cs
??? ExternalServices/      # External API implementations
?   ??? Service1.cs
??? Migrations/            # Database migrations (nh? Flyway/Liquibase)
?   ??? 20251021065111_InitDB.cs
?   ??? AppDbContextModelSnapshot.cs
??? DependencyInjection.cs # DI configuration
??? AppSettings.cs         # Application settings
```

### ?? Dependencies
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.20" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.20" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.20" />
  <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
  <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="8.0.2" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
  <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.11" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\Application\Application.csproj" />
  <ProjectReference Include="..\Domain\Core.csproj" />
</ItemGroup>
```

**Dependencies:** Ph? thu?c vào `Application` và `Domain`

### ?? Ví d? Repository Implementation

**.NET:**
```csharp
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context; // Nh? EntityManager
    
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product?> GetByIdAsync(int id) =>
        await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    
    public async Task<List<Product>> GetAllAsync() =>
        await _context.Products.ToListAsync();
    
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync(); // Nh? entityManager.persist()
    }
}
```

**So sánh v?i Spring Boot:**
```java
@Repository
public class ProductRepositoryImpl implements IProductRepository {
    @PersistenceContext
    private EntityManager entityManager;
    
    public Optional<Product> findById(Integer id) {
        return Optional.ofNullable(entityManager.find(Product.class, id));
    }
    
    public List<Product> findAll() {
        return entityManager.createQuery("FROM Product", Product.class)
            .getResultList();
    }
    
    public Product save(Product product) {
        entityManager.persist(product);
        return product;
    }
}
```

### ?? DbContext (Entity Framework)

**.NET:**
```csharp
public class AppDbContext : DbContext  // Nh? @Configuration + EntityManager
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }
    
    public DbSet<Product> Products => Set<Product>(); // Nh? Repository<Product>
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply configurations (nh? @EntityListeners)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
```

**So sánh v?i Spring Boot:**
```java
@Configuration
@EnableJpaRepositories
public class JpaConfig {
    @Bean
    public LocalContainerEntityManagerFactoryBean entityManagerFactory(
            DataSource dataSource) {
        LocalContainerEntityManagerFactoryBean em = 
            new LocalContainerEntityManagerFactoryBean();
        em.setDataSource(dataSource);
        em.setPackagesToScan("com.example.domain");
        return em;
    }
}
```

### ?? Dependency Injection Configuration

**.NET:**
```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        string connectionString)
    {
        // Register DbContext (nh? @Bean DataSource)
        services.AddDbContext<AppDbContext>(opt => 
            opt.UseNpgsql(connectionString));
        
        // Register repositories (nh? @Component scan)
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IService1, Service1>();
        
        return services;
    }
}
```

**So sánh v?i Spring Boot:**
```java
@Configuration
public class AppConfig {
    @Bean
    public DataSource dataSource() {
        // Configure datasource
    }
    
    // Spring t? ??ng scan @Repository, @Service
}
```

---

## ?? 4. API LAYER (Presentation)

**T??ng ???ng:** Controller trong Spring Boot

**Ch?c n?ng:** X? lý HTTP requests, routing, validation

### ?? C?u trúc th? m?c
```
Api/
??? Controllers/           # Controllers (nh? @RestController)
?   ??? BaseController.cs
?   ??? ProductsController.cs
?   ??? InternalController.cs
??? Attributes/            # Custom attributes (nh? @Annotation)
?   ??? InternalAuthorize.cs
??? Program.cs            # Application entry point (nh? Application.java)
```

### ?? Dependencies
```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.20" />
  <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.20.1" />
  <PackageReference Include="Newtonsoft.Json" Version="13.0.4" />
  <PackageReference Include="Serilog.AspNetCore" Version="8.0.3" />
  <PackageReference Include="Serilog.Settings.Configuration" Version="9.0.1-dev-02317" />
  <PackageReference Include="Serilog.Sinks.Console" Version="6.0.1-dev-00953" />
  <PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
  <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
</ItemGroup>

<ItemGroup>
  <ProjectReference Include="..\Infrastructure\Infrastructure.csproj" />
</ItemGroup>
```

**Dependencies:** Ph? thu?c vào `Infrastructure`

### ?? Ví d? Controller

**.NET:**
```csharp
[ApiController]  // Nh? @RestController
[Route("api/[controller]")]  // Nh? @RequestMapping
public class ProductsController : BaseController
{
    private readonly IProductService _service;
    
    public ProductsController(IProductService service)  // Nh? @Autowired
    {
        _service = service;
    }
    
    [HttpGet]  // Nh? @GetMapping
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);  // Nh? ResponseEntity.ok()
    }
    
    [HttpGet("{id}")]  // Nh? @GetMapping("/{id}")
    public async Task<IActionResult> Get(int id)
    {
        var product = await _service.GetProductAsync(id);
        return Ok(product);
    }
    
    [HttpPost]  // Nh? @PostMapping
    public async Task<IActionResult> Create([FromBody] CreateProductDTO model)
    {
        await _service.AddProductAsync(model.Name, model.Price);
        return Ok();
    }
}
```

**So sánh v?i Spring Boot:**
```java
@RestController
@RequestMapping("/api/products")
public class ProductsController {
    @Autowired
    private IProductService service;
    
    @GetMapping
    public ResponseEntity<List<Product>> getAll() {
        List<Product> products = service.getAllProducts();
        return ResponseEntity.ok(products);
    }
    
    @GetMapping("/{id}")
    public ResponseEntity<Product> getById(@PathVariable Integer id) {
        Product product = service.getProductById(id);
        return ResponseEntity.ok(product);
    }
    
    @PostMapping
    public ResponseEntity<Void> create(@RequestBody CreateProductDTO model) {
        service.addProduct(model.getName(), model.getPrice());
        return ResponseEntity.ok().build();
    }
}
```

### ?? Program.cs (Startup Configuration)

**.NET:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog (nh? logback/slf4j trong Spring)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// G?n Serilog vào host
builder.Host.UseSerilog();

// Add Infrastructure layer (nh? @ComponentScan)
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection"));

// Register services (Dependency Injection)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // Nh? Springdoc OpenAPI

// Initialize AppSettings
AppSettings.Initialize(builder.Configuration);

var app = builder.Build();

// Initialize database
await Init(app);

// Configure middleware pipeline (nh? Filter/Interceptor chain)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Log.Logger.Error($"Service Started");

app.Run();  // Start application


async Task Init(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Auto migration (nh? Flyway)
        // if (db.Database.GetPendingMigrations().Any())
        // {
        //     db.Database.Migrate();
        // }
    }
}
```

**So sánh v?i Spring Boot:**
```java
@SpringBootApplication
public class Application {
    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
    
    @Bean
    public CommandLineRunner init(AppDbContext db) {
        return args -> {
            // Initialize database
        };
    }
}
```

---

## ?? SO SÁNH T?NG QUAN

### ?? Dependency Direction (H??ng ph? thu?c)
```
Api ? Infrastructure ? Application ? Domain
                     ?
```

**Nguyên t?c:** Các layer ngoài ph? thu?c vào layer trong, layer trong KHÔNG bi?t gì v? layer ngoài

### ?? Dependency Injection

| .NET | Spring Boot |
|------|-------------|
| `services.AddScoped<I, Impl>()` | `@Service` / `@Repository` |
| Constructor Injection | `@Autowired` |
| `IServiceCollection` | `ApplicationContext` |
| `AddScoped` (per request) | `@RequestScope` |
| `AddSingleton` (single instance) | `@Singleton` |
| `AddTransient` (per usage) | `@Prototype` |

### ??? ORM Comparison

| .NET (Entity Framework) | Java (JPA/Hibernate) |
|------------------------|----------------------|
| `DbContext` | `EntityManager` |
| `DbSet<T>` | `Repository<T>` |
| `FirstOrDefaultAsync()` | `findById()` |
| `ToListAsync()` | `findAll()` |
| `AddAsync()` + `SaveChangesAsync()` | `save()` |
| `Include()` | `@JoinColumn` / `fetch` |
| Migrations | Flyway/Liquibase |
| Code-First | Schema generation |

### ? Async Programming

| .NET | Java |
|------|------|
| `async Task<T>` | `CompletableFuture<T>` ho?c Project Reactor |
| `await` | `.get()` ho?c `.block()` |
| Built-in async/await | Virtual Threads (Java 21+) |

### ?? Logging

| .NET | Java |
|------|------|
| Serilog | Logback / Log4j2 |
| `ILogger<T>` | `Logger` / `@Slf4j` |
| `appsettings.json` | `logback.xml` / `log4j2.xml` |

### ?? Configuration

| .NET | Java |
|------|------|
| `appsettings.json` | `application.properties` / `application.yml` |
| `IConfiguration` | `@Value` / `@ConfigurationProperties` |
| User Secrets | Environment variables |

---

## ? ?I?M KHÁC BI?T QUAN TR?NG

### 1. **Async/Await**
- ? .NET: Async/await built-in, performance cao
- ?? Spring Boot: CompletableFuture ho?c Virtual Threads (Java 21)

### 2. **Dependency Injection**
- ? .NET: Manual registration trong `Program.cs`, ki?m soát t?t h?n
- ? Spring: Auto-scan v?i annotations, ti?n l?i h?n

### 3. **Entity Framework vs JPA**
- ? EF: Code-first approach m?c ??nh, Migrations tích h?p
- ? JPA: Database-first ho?c code-first, Flyway/Liquibase riêng bi?t

### 4. **Project Structure**
- ? .NET: Solution ? Multiple Projects (physical separation)
- ? Spring: Single Project ? Multiple Packages (logical separation)

### 5. **Naming Convention**
- ? .NET: PascalCase cho methods, properties
- ? Java: camelCase cho methods, properties

### 6. **Nullable Reference Types**
- ? .NET 8: `<Nullable>enable</Nullable>` - compile-time null safety
- ?? Java: `@Nullable` / `@NonNull` annotations (runtime)

### 7. **Extension Methods**
- ? .NET: `AddInfrastructure(this IServiceCollection services)`
- ? Java: Không có extension methods (ph?i dùng static methods)

---

## ?? NGUYÊN T?C THI?T K? (SOLID)

### ? Single Responsibility Principle
M?i class ch? có m?t lý do ?? thay ??i:
- `ProductRepository` - ch? lo database
- `ProductService` - ch? lo business logic
- `ProductsController` - ch? lo HTTP handling

### ? Open/Closed Principle
M? r?ng thông qua interface, không s?a code c?:
- Interface: `IProductRepository`, `IProductService`
- Implementation: `ProductRepository`, `ProductService`

### ? Liskov Substitution Principle
Có th? thay th? implementation mà không ?nh h??ng:
```csharp
IProductRepository repo = new ProductRepository(); // Database
// ho?c
IProductRepository repo = new InMemoryProductRepository(); // Test
```

### ? Interface Segregation Principle
Interface nh?, t?p trung:
- `IProductRepository` - ch? CRUD operations
- `IProductService` - ch? business operations

### ? Dependency Inversion Principle
Ph? thu?c vào abstraction, không ph? thu?c vào concrete:
```csharp
// ? ?ÚNG
private readonly IProductRepository _repository;

// ? SAI
private readonly ProductRepository _repository;
```

---

## ?? WORKFLOW TH?C T?

### 1?? T?o Entity m?i
```
Domain/Entities/Order.cs
```

### 2?? T?o Repository Interface
```
Application/IRepositories/IOrderRepository.cs
```

### 3?? T?o Repository Implementation
```
Infrastructure/Repositories/OrderRepository.cs
```

### 4?? C?u hình Entity trong DbContext
```
Infrastructure/Persistence/Configurations/OrderConfiguration.cs
```

### 5?? T?o Migration
```bash
dotnet ef migrations add AddOrderTable --project Infrastructure --startup-project Api
```

### 6?? T?o Service Interface & Implementation
```
Application/IServices/IOrderService.cs
Application/Services/OrderService.cs
```

### 7?? Register DI
```csharp
// Infrastructure/DependencyInjection.cs
services.AddScoped<IOrderRepository, OrderRepository>();

// Api/Program.cs
services.AddScoped<IOrderService, OrderService>();
```

### 8?? T?o Controller
```
Api/Controllers/OrdersController.cs
```

---

## ?? TÀI LI?U THAM KH?O

### Official Documentation
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Patterns & Practices
- Dependency Injection
- Repository Pattern
- Service Pattern
- DTO Pattern
- CQRS (n?u c?n m? r?ng)

---

## ?? K?T LU?N

Ki?n trúc này tuân th? **SOLID principles** và **Clean Architecture**, gi?ng nh? cách Spring Boot ???c thi?t k?. ?i?m m?nh:

? **Separation of Concerns** - Tách bi?t rõ ràng các layer  
? **Testability** - D? test t?ng layer ??c l?p  
? **Maintainability** - D? b?o trì và m? r?ng  
? **Independence** - Domain không ph? thu?c vào framework  
? **Scalability** - D? dàng scale và thay ??i implementation  

### ?? L?i khuyên cho ng??i t? Spring Boot
1. **T? duy gi?ng nhau:** Concepts c? b?n gi?ng Spring Boot
2. **Async/Await:** H?c async/await ?? t?n d?ng s?c m?nh c?a .NET
3. **Dependency Injection:** Manual registration t?t h?n magic annotations
4. **Entity Framework:** Code-first approach r?t m?nh m?
5. **Clean Architecture:** Gi? nguyên t?c, không ph? thu?c vào framework

---

## ?? H? TR?

N?u có th?c m?c v? ki?n trúc ho?c c?n h? tr?:
1. ??c k? documentation c?a t?ng layer
2. Tham kh?o code examples trong project
3. Follow SOLID principles
4. Test t?ng layer ??c l?p

**Happy Coding! ??**
