using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<DataContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options => {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                         ValidateIssuer = false,
                         ValidateAudience = false,
                     };
                 });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireHRManagerRoles", policy => policy.RequireRole("Admin", "Manager", "HR"));
                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
            });

            //services.Configure<IdentityOptions>(opt =>
            //{
            //    opt.SignIn.RequireConfirmedEmail = true;
            //    opt.SignIn.RequireConfirmedAccount = true;
            //});

            

            return services;
        }
    }
}
