﻿using Capstone.Application.Common.Cloudinaries;
using Capstone.Application.Common.Jwt;
using Capstone.Domain.Entities;
using Capstone.Infrastructure.DbContexts;
using Capstone.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Capstone.Api.Common.ConfigureService
{
    public static class DataExtention
    {
        public static void AddDataService(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("value");
            services.AddDbContext<SeCapstoneContext>(options => options.UseNpgsql(configuration.GetConnectionString("value")));
            services.AddTransient<IRepository<Job>, Repository<Job>>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Role>, Repository<Role>>();
            services.AddSingleton<CloudinaryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
