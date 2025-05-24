using System;
using IdentityNetCore.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityNetCore;

public class Startup
{
	private IConfiguration Configuration { get; set; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		var connString = Configuration["ConnectionStrings:Default"];

		services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connString));
		services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

		// Configure identity options
		services.Configure<IdentityOptions>(options =>
		{
			options.Password.RequiredLength = 8;
			options.Password.RequireDigit = true;
			options.Password.RequireNonAlphanumeric = true;

			options.Lockout.MaxFailedAccessAttempts = 4;
			options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

			options.SignIn.RequireConfirmedEmail = true;
		});
		services.ConfigureApplicationCookie
		(options =>
		{
			options.LoginPath = "/Home/SignIn";
			options.AccessDeniedPath = "/Home/AccessDenied";
			options.ExpireTimeSpan = TimeSpan.FromHours(1);
		});

		services.AddRazorPages();
		services.AddControllersWithViews();
	}
	
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
		});
	}
}