using FluentAssertions;
using Moq;
using OrbitPass.Application.UseCases.Pagamentos.ProcessarPagamento;
using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Enums;
using OrbitPass.Domain.Exceptions;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Tests.UseCases;

public class ProcessarPagamentoHandlerTests {
    private readonly Mock<IPagamentoRepository> _pagamentoRepoMock = new();
    private readonly Mock<IIngressoRepository> _ingressoRepoMock = new();

    private ProcessarPagamentoHandler CriarHandler() => new(
        _pagamentoRepoMock.Object,
        _ingressoRepoMock.Object);

    [Fact]
    public async Task Handle_DeveProcessarPagamento_QuandoIngressoValido() {
        // Arrange
        var ingresso = Ingresso.Criar(Guid.NewGuid(), Guid.NewGuid(), 3000m);
        var command = new ProcessarPagamentoCommand(
            ingresso.Id, MetodoPagamento.CartaoCredito, 3000m);

        _ingressoRepoMock.Setup(r => r.ObterPorIdAsync(ingresso.Id, default))
                         .ReturnsAsync(ingresso);
        _pagamentoRepoMock.Setup(r => r.ObterPorIngressoIdAsync(ingresso.Id, default))
                          .ReturnsAsync((Pagamento?)null);
        _pagamentoRepoMock.Setup(r => r.AdicionarAsync(It.IsAny<Pagamento>(), default))
                          .Returns(Task.CompletedTask);
        _ingressoRepoMock.Setup(r => r.AtualizarAsync(It.IsAny<Ingresso>(), default))
                         .Returns(Task.CompletedTask);

        // Act
        var resultado = await CriarHandler().Handle(command);

        // Assert
        resultado.Status.Should().Be(StatusPagamento.Aprovado);
        resultado.Valor.Should().Be(3000m);
    }

    [Fact]
    public async Task Handle_DeveLancarDomainException_QuandoPagamentoDuplicado() {
        // Arrange
        var ingresso = Ingresso.Criar(Guid.NewGuid(), Guid.NewGuid(), 3000m);
        var pagamento = Pagamento.Criar(ingresso.Id, MetodoPagamento.Pix, 3000m);
        var command = new ProcessarPagamentoCommand(
            ingresso.Id, MetodoPagamento.Pix, 3000m);

        _ingressoRepoMock.Setup(r => r.ObterPorIdAsync(ingresso.Id, default))
                         .ReturnsAsync(ingresso);
        _pagamentoRepoMock.Setup(r => r.ObterPorIngressoIdAsync(ingresso.Id, default))
                          .ReturnsAsync(pagamento);

        // Act
        var act = () => CriarHandler().Handle(command);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*já possui*");
    }
}