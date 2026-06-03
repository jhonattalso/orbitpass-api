using FluentAssertions;
using Moq;
using OrbitPass.Application.UseCases.Ingressos.ComprarIngresso;
using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Enums;
using OrbitPass.Domain.Exceptions;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Tests.UseCases;

public class ComprarIngressoHandlerTests {
    private readonly Mock<IIngressoRepository> _ingressoRepoMock = new();
    private readonly Mock<IPagamentoRepository> _pagamentoRepoMock = new();

    private ComprarIngressoHandler CriarHandler() => new(
        _ingressoRepoMock.Object,
        _pagamentoRepoMock.Object);

    [Fact]
    public async Task Handle_DeveCriarIngresso_QuandoDadosValidos() {
        // Arrange
        var command = new ComprarIngressoCommand(
            Guid.NewGuid(), Guid.NewGuid(), 1500.00m, MetodoPagamento.Pix);

        _ingressoRepoMock
            .Setup(r => r.AdicionarAsync(It.IsAny<Ingresso>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _pagamentoRepoMock
            .Setup(r => r.AdicionarAsync(It.IsAny<Pagamento>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _ingressoRepoMock
            .Setup(r => r.AtualizarAsync(It.IsAny<Ingresso>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await CriarHandler().Handle(command);

        // Assert
        resultado.Should().NotBeNull();
        resultado.CodigoUnico.Should().StartWith("OP-");
        resultado.StatusIngresso.Should().Be(StatusIngresso.Confirmado);
        resultado.StatusPagamento.Should().Be(StatusPagamento.Aprovado);
    }

    [Fact]
    public async Task Handle_DeveLancarDomainException_QuandoValorZero() {
        // Arrange
        var command = new ComprarIngressoCommand(
            Guid.NewGuid(), Guid.NewGuid(), 0m, MetodoPagamento.Pix);

        // Act
        var act = () => CriarHandler().Handle(command);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*valor*");
    }

    [Fact]
    public async Task Handle_DeveGerarCodigoUnico_ParaCadaIngresso() {
        // Arrange
        var command = new ComprarIngressoCommand(
            Guid.NewGuid(), Guid.NewGuid(), 2000m, MetodoPagamento.CartaoCredito);

        _ingressoRepoMock
            .Setup(r => r.AdicionarAsync(It.IsAny<Ingresso>(), default))
            .Returns(Task.CompletedTask);
        _pagamentoRepoMock
            .Setup(r => r.AdicionarAsync(It.IsAny<Pagamento>(), default))
            .Returns(Task.CompletedTask);
        _ingressoRepoMock
            .Setup(r => r.AtualizarAsync(It.IsAny<Ingresso>(), default))
            .Returns(Task.CompletedTask);

        // Act
        var r1 = await CriarHandler().Handle(command);
        var r2 = await CriarHandler().Handle(command);

        // Assert
        r1.CodigoUnico.Should().NotBe(r2.CodigoUnico);
    }
}