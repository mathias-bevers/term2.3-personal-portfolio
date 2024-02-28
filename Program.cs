namespace PersonalPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.WebHost.UseStaticWebAssets();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".PersonalPortfolio.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10); //TODO: remove when done testing...
                // options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.Cookie.IsEssential = true;
            });

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            // NOTE: middleware order from: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0#order
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseRouting();
            //app.UseRateLimiter();
            //app.UseRequestLocalization();
            //app.UseCors();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            //app.UseResponseCompression();
            //app.UseResponseCaching();

            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            
            app.Run();
        }
    }
}