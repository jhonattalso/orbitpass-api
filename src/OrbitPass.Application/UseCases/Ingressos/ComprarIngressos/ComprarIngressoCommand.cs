using OrbitPass.Domain.Enums;

namespace OrbitPass.Application.UseCases.Ingressos.ComprarIngresso;

public record ComprarIngressoCommand(
    Guid UsuarioId,
    Guid DataTourId,
    decimal ValorPago,
    MetodoPagamento Metodo
);