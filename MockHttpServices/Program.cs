using Microsoft.OpenApi.Models;
using MockHttpServices.Repositories;
using MockHttpServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskQueue, TaskQueue>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Register Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mock Http Services API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mock Http Services API v1");
        c.RoutePrefix = string.Empty; 
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
//app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
