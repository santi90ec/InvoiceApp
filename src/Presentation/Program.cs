using InvoiceApp.Application.Interfaces;
using InvoiceApp.Application.UseCase;
using InvoiceApp.Infrastracture.Classification;
using InvoiceApp.Infrastracture.Storage;
using InvoiceApp.Infrastracture.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<IInvoiceReader, InvoiceXmlReader>();
builder.Services.AddScoped<IInvoiceClassifier, BasicInvoiceClassifier>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ProcessInvoiceUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
