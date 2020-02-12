using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UploadFile.WebApi.Jwt;
using UploadFile.WebApi.Context;
using UploadFile.WebApi.Repositories.Interfaces;
using UploadFile.WebApi.Repositories;
using UploadFile.WebApi.Services.Interfaces;
using UploadFile.WebApi.Services;

namespace UploadFile.WebApi.Extensions
{
    public static class ServiceExtensions
    {
        // Configuração de Cors
        public static void ConfigureCors (this IServiceCollection services) 
        {
            services.AddCors();
        }

        // Configuração de conexão com o banco
        public static void ConfigurePostgresContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddEntityFrameworkNpgsql()
               .AddDbContext<UploadFileDbContext>(
                   options => options.UseNpgsql(
                   config["UploadFileBd"]));

            // services.AddScoped<UtilMigrate> ();
        }

        // Configuração de injeção de dependencia dos Repositorios
        public static void ConfigureRepositories (this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
        }

        // Configuração de injeção de dependencia dos Services
        public static void ConfigureServices (this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IUserService, UserService>();
        }

        // Configuração de autenticação
        public static void ConfigureAuthentication (this IServiceCollection services, IConfiguration config)
        {
            var signingConfigurations = new SigningConfigurations ();
            
            // configura a injeção de dependencia da classe siginConfigurations para singleton
            services.AddSingleton (signingConfigurations);

            var tokenConfigurations = new TokenConfigurations ();

            // pega as configurações de token no appsettings
            new ConfigureFromConfigurationOptions<TokenConfigurations> (
                    config.GetSection ("TokenConfigurations"))
                .Configure (tokenConfigurations);

            // adiciona ela como singleton
            services.AddSingleton (tokenConfigurations);

            // configura o metodo de auth
            services.AddAuthentication (authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer (bearerOptions =>
                {
                    var paramsValidation = bearerOptions.TokenValidationParameters;

                    // paramsValidation.IssuerSigningKey = signingConfigurations.Key; descomentar depois essa linha

                    paramsValidation.IssuerSigningKey = new SymmetricSecurityKey (
                        Encoding.UTF8.GetBytes (tokenConfigurations.Key));

                    paramsValidation.ValidAudience = tokenConfigurations.Audience;

                    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization (auth =>
            {
                auth.AddPolicy ("Bearer", new AuthorizationPolicyBuilder ()
                    .AddAuthenticationSchemes (JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser ().Build ());
            });
        }
    }
}