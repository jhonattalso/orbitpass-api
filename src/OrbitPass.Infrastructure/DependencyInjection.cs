using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrbitPass.Application.UseCases.Ingressos.CancelarIngresso;
using OrbitPass.Application.UseCases.Ingressos.ComprarIngresso;
using OrbitPass.Application.UseCases.Ingressos.ListarIngressos;
using OrbitPass.Application.UseCases.Pagamentos.ProcessarPagamento;
using OrbitPass.Domain.Interfaces;
using OrbitPass.Infrastructure.Persistence.Context;
using OrbitPass.Infrastructure.Persistence.Repositories;

namespace OrbitPass.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString) {
        services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(connectionString));

        services.AddScoped<IIngressoRepository, IngressoRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();

        services.AddScoped<ComprarIngressoHandler>();
        services.AddScoped<CancelarIngressoHandler>();
        services.AddScoped<ListarIngressosHandler>();
        services.AddScoped<ProcessarPagamentoHandler>();

        return services;
    }
}