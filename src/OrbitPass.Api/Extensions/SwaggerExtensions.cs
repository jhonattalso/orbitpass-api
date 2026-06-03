using Microsoft.OpenApi.Models;

namespace OrbitPass.Api.Extensions;

public static class SwaggerExtensions {
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services) {
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo {
                Title = "OrbitPass API",
                Version = "v1",
                Description = "Módulo de Ingressos e Pagamentos — Global Solution FIAP 2026/1"
            });

            var scheme = new OpenApiSecurityScheme {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Informe: Bearer {seu_token}"
            };

            c.AddSecurityDefinition("Bearer", scheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}