using MISA.CRM.API.Middlewares;
using MISA.CRM.CORE.Interfaces.Repositories;
using MISA.CRM.CORE.Interfaces.Services;
using MISA.CRM.CORE.Services;
using MISA.CRM.Infrastructure.Connection;
using MISA.CRM.Infrastructure.Repositories;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký MySQL connection factory
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<MySqlConnectionFactory>(
    new MySqlConnectionFactory(connectionString)
);

// Đăng ký Repository
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Đăng ký Service
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ValidateExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();