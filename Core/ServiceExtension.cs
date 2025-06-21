using Core.Interfaces;
using Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceExtension
    {
        public static void AddCustomService(this IServiceCollection service)
        {
            service.AddScoped<IAccountService, AccountService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<IEmailService, EmailService>();
            service.AddScoped<IBankService, BankService>();
            service.AddScoped<IRegionService, RegionService>();
        }
        public static void AddValidator(this IServiceCollection service)
        {
            service.AddFluentValidationAutoValidation();

            service.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        }
        public static void AddAutoMapper(this IServiceCollection service)
        {
            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}