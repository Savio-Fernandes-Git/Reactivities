using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using API.Extensions;
using API.Middleware;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// configure the HTTP request pipeline (middleware)
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (builder.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

//since we aren't using https we can comment the next line
// app.UseHttpsRedirection();

//routing middleware responsible routing to or endpoints (controllers)
//it is important to note the order of calling these functions
// we dont need UseRouting for .net 7
// app.UseRouting();

app.UseCors("CorsPolicy");

//we need authentication to go before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//endpoint for signalR
app.MapHub<ChatHub>("/chat");

//using dependency injection
//keyword using here will dispose var scope
//scope is where we will be storing our services so it needs to be disposed after use
//there are things we need to dispose after we finish using it
using var scope = app.Services.CreateScope();

//service fetcher
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    //will create database if it doesn't already exist
    await context.Database.MigrateAsync();
    await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    //ILogger is a service, ILogger takes a type <Program> is the class we're logging from
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}
// we need to make sure we run the application (CreateHostBuilder)
app.Run();