using OrbitPass.Domain.Enums;

namespace OrbitPass.Application.UseCases.Pagamentos.ProcessarPagamento;

public record ProcessarPagamentoCommand(
    Guid IngressoId,
    MetodoPagamento Metodo,
    decimal Valor
);