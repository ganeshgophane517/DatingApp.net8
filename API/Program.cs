using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:4200","https://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod());


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

 using var scope =  app.Services.CreateScope();

var  services =  scope.ServiceProvider;
try{
   var context = services.GetRequiredService<DataContext>();
   await context.Database.MigrateAsync();
   await Seed.SeedUsers(context);
}
catch(Exception ex){
   var logger = services.GetRequiredService<ILogger<Program>>();
   logger.LogError(ex,"An error occured during migration");
}


// try
// {

//     var context = services.GetRequiredService<DataContext>();
//     await context.Database.MigrateAsync();
//     await Seed.SeedUsers(context, builder.Configuration); // Pass configuration
// }
// catch (Exception ex)
// {
//     var logger = services.GetRequiredService<ILogger<Program>>();
//     logger.LogError(ex, "An error occurred during migration or seeding: {Message}", ex.Message);
// }

app.Run();
