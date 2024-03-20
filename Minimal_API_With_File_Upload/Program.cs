using Microsoft.AspNetCore.Mvc;
using Minimal_API_With_File_Upload.Fileupload;
using Minimal_API_With_File_Upload.FileuploadService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//cors configuration:
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
//Dependency Concepts:
builder.Services.AddScoped<IFileUpladService, FileUpladService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

///minimal_API:
app.MapGet("/", () => "Hello World!");

app.MapPost("/FileUpload", async (HttpContext context, string location,IFileUpladService fileUpladService) =>
{
    var form = await context.Request.ReadFormAsync();
    var files = form.Files;
    var file = files.Any() && files.Count > 0 ? files[0] : null;
    var response = await fileUpladService.FileUpload(file, location, context);
    return Results.Created("FileUrl",response);
});
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
