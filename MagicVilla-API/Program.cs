using MagicVilla_API.Logging;
using MagicVilla_API.Repositories.Implementations;
using MagicVilla_API.Repositories.Interfaces;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Mapper;
using MagicVilla_VillaAPI.Repositories.Villa_Number_Repositories.Implementation;
using MagicVilla_VillaAPI.Repositories.Villa_Number_Repositories.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(option =>
{
    //  option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddSingleton<ILogging, Logging>();
builder.Services.AddScoped<IVillaRepo, VillaRepo>();
builder.Services.AddScoped<IVillaNumberRepo, VillaNumberRepo>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
