using IdentityNetCore;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }
// app.UseHttpsRedirection();
// app.UseStaticFiles();
//
// app.UseAuthorization();
// app.UseAuthentication();
//
// app.MapDefaultControllerRoute();
// app.MapRazorPages();

app.Run();