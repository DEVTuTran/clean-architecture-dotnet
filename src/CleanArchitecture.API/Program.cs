using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application.Mappings;
using CleanArchitecture.Application.Handlers;
using CleanArchitecture.Application.Queries;
using CleanArchitecture.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services
builder.Services.AddScoped<IProductService, ProductService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetAllProductsQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();