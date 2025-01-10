using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using serverApp.Infrastructure;
using serverApp.Services;
using System.Text;

namespace serverApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<TokenProvider>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "clientPolicy",
                                  policy =>
                                  {
                                      var origin = builder.Configuration.GetSection("origin");
                                      policy.AllowAnyOrigin();//WithOrigins(origin.Value);
                                  });
            });
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IFileOperationsService, FileOperationsService>();

            builder.Services.AddAuthentication(config => {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:secret"])
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    //ValidIssuer = builder.Configuration["JWT:issuer"],
                    //ValidAudience = builder.Configuration["JWT:audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("clientPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
