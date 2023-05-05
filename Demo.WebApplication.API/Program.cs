using Demo.WebApplication.API;
using Demo.WebApplication.BL.BaseBL;
using Demo.WebApplication.BL.DepartmentBL;
using Demo.WebApplication.BL.EmployeeBL;
using Demo.WebApplication.BL.OverTimeBL;
using Demo.WebApplication.BL.OverTimeDetailBL;
using Demo.WebApplication.DL;
using Demo.WebApplication.DL.BaseDL;
using Demo.WebApplication.DL.DepartmentDL;
using Demo.WebApplication.DL.EmployeeDL;
using Demo.WebApplication.DL.OverTimeDetailDL;
using Demo.WebApplication.DL.OverTimeDL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new DateTimeHandler());
}
        );
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//Dependency injection
builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();
builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
builder.Services.AddScoped<IDepartmentDL, DepartmentDL>();
builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
builder.Services.AddScoped<IOverTimeDL, OverTimeDL>();
builder.Services.AddScoped<IOverTimeBL, OverTimeBL>();
builder.Services.AddScoped<IOverTimeDetailDL, OverTimeDetailDL>();
builder.Services.AddScoped<IOverTimeDetailBL, OverTimeDetailBL>();
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));

DatabaseContext.ConnectionString  = builder.Configuration.GetConnectionString("MySQL");


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
