using OrbitPass.Domain.Exceptions;

namespace OrbitPass.Domain.Entities;

public class DataTour {
    public Guid Id { get; private set; }
    public string Destino { get; private set; } = string.Empty;
    public DateTime DataPartida { get; private set; }
    public decimal PrecoBase { get; private set; }

    // Propriedade de navegação: 1 Tour para N Ingressos
    public ICollection<Ingresso> Ingressos { get; private set; } = new List<Ingresso>();

    protected DataTour() { }

    public static DataTour Criar(string destino, DateTime dataPartida, decimal precoBase) {
        if (string.IsNullOrWhiteSpace(destino))
            throw new DomainException("O destino do tour não pode ser vazio.");

        if (precoBase <= 0)
            throw new DomainException("O preço base do tour deve ser positivo.");

        return new DataTour {
            Id = Guid.NewGuid(),
            Destino = destino,
            DataPartida = dataPartida,
            PrecoBase = precoBase
        };
    }
}