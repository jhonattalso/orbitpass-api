namespace OrbitPass.Application.UseCases.Ingressos.CancelarIngresso;

public record CancelarIngressoCommand(Guid IngressoId, Guid UsuarioId);