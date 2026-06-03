using FluentAssertions;
using Moq;
using OrbitPass.Application.UseCases.Ingressos.CancelarIngresso;
using OrbitPass.Domain.Entities;
using OrbitPass.Domain.Enums;
using OrbitPass.Domain.Exceptions;
using OrbitPass.Domain.Interfaces;

namespace OrbitPass.Tests.UseCases;

public class CancelarIngressoHandlerTests {
    private readonly Mock<IIngressoRepository> _repoMock = new();

    private CancelarIngressoHandler CriarHandler() => new(_repoMock.Object);

    [Fact]
    public async Task Handle_DeveCancelarIngresso_QuandoUsuarioCorreto() {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var ingresso = Ingresso.Criar(usuarioId, Guid.NewGuid(), 1000m);
        ingresso.Confirmar();

        _repoMock.Setup(r => r.ObterPorIdAsync(ingresso.Id, default))
                 .ReturnsAsync(ingresso);
        _repoMock.Setup(r => r.AtualizarAsync(ingresso, default))
                 .Returns(Task.CompletedTask);

        // Act
        await CriarHandler().Handle(new CancelarIngressoCommand(ingresso.Id, usuarioId));

        // Assert
        ingresso.Status.Should().Be(StatusIngresso.Cancelado);
    }

    [Fact]
    public async Task Handle_DeveLancarDomainException_QuandoUsuarioDiferente() {
        // Arrange
        var ingresso = Ingresso.Criar(Guid.NewGuid(), Guid.NewGuid(), 1000m);

        _repoMock.Setup(r => r.ObterPorIdAsync(ingresso.Id, default))
                 .ReturnsAsync(ingresso);

        var outroUsuario = Guid.NewGuid();

        // Act
        var act = () => CriarHandler()
            .Handle(new CancelarIngressoCommand(ingresso.Id, outroUsuario));

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*permissão*");
    }

    [Fact]
    public async Task Handle_DeveLancarKeyNotFound_QuandoIngressoNaoExiste() {
        // Arrange
        _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), default))
                 .ReturnsAsync((Ingresso?)null);

        // Act
        var act = () => CriarHandler()
            .Handle(new CancelarIngressoCommand(Guid.NewGuid(), Guid.NewGuid()));

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}