using EInvoice.Business.Services.External.Implements;
using EInvoice.Business.Services.External.Interfaces;
using EInvoice.Business.Services.Internal.Implements;
using EInvoice.Business.Services.Internal.Interfaces;
using EInvoice.DAL.Data;
using EInvoice.DAL.Repositories.Implements;
using EInvoice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder (args);

builder.Services.AddDbContext<AppDbContext>(opt =>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IGoodRepository, GoodRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceFieldDefinitionRepository, InvoiceFieldDefinitionRepository>();
builder.Services.AddScoped<IInvoiceFieldValueRepository, InvoiceFieldValueRepository>();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IInvoiceFieldDefinitionService, InvoiceFieldDefinitionService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IGoodService,GoodService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();