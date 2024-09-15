using Domain.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Abstractions;
using Services.Abstractions;
using Services.Implementations;
using Services.MappingProfiles;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using TimeSheet.Middleware;
using TimeSheet.Swagger;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

        builder.Services.AddScoped<IUnitOfWork,  UnitOfWork>();


        builder.Services.AddTransient<ICategoryService, CategoryService>();

        builder.Services.AddTransient<ITeamMemberService, TeamMemberService>();

        builder.Services.AddTransient<IClientService, ClientService>();

        builder.Services.AddTransient<IProjectService, ProjectService>();

        builder.Services.AddTransient<IActivityService, ActivityService>();

        builder.Services.AddTransient<IReportService, ReportService>();

        builder.Services.AddTransient<IAccountService, AccountService>();

        builder.Services.AddTransient<ITokenService, TokenService>();

        builder.Services.AddTransient<IPdfService, PdfService>();
        builder.Services.AddTransient<IExcelService, ExcelService>();

        builder.Services.AddDbContext<RepositoryDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "MyAllowSpecificOrigins",
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader()
                                      .WithExposedHeaders("pagination");
                              });
        });


        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();
        app.UseCors("MyAllowSpecificOrigins");

        app.Run();
    }
}