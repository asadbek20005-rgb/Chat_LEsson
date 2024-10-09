using Chat.Api.Context;
using Chat.Api.Helper;
using Chat.Api.Jwt;
using Chat.Api.Managers;
using Chat.Api.MemoryCache;
using Chat.Api.UnitOfWork.Classes;
using Chat.Api.UnitOfWork.Implementations;
using Chat.Api.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Chat.Api.DIContainer
{
    public static class RegisterationOfService
    {
        public static void AddToService(this WebApplicationBuilder builder)
        {
            var conntectionString = builder.Configuration.GetConnectionString("Connection");
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork.Classes.UnitOfWork>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<UserManager>();
            builder.Services.AddScoped<ChatManager>();
            builder.Services.AddScoped<MessageManager>();
            builder.Services.AddScoped<JwtManager>();
            builder.Services.AddScoped<UserHelper>();
            builder.Services.AddScoped<MessageHelper>();
            builder.Services.AddScoped<JwtSettings>();
            builder.Services.AddScoped<MemoryCacheManager>();


            builder.Services.AddCors();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(conntectionString);
            });

            builder.Services.AddMemoryCache();
            builder.Services.AddSignalR();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = securityKey,
                    ClockSkew = TimeSpan.FromDays(1),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = false,

                };
                options.SaveToken = true;

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Token;

                        if (string.IsNullOrEmpty(token))
                        {
                            token = context.Request.Query["token"];

                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }

                        }

                        return Task.CompletedTask;
                    }
                };
            });

        }
    }
}
