using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using WorkerService.Data.Entities;
using WorkerService.Data.Repository;
using WorkerService.DTOs;
using WorkerService.Enums;
using Xunit;

namespace WorkerService.Handlers.Tests
{
    public class PedidoHandlerTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly IPedidoHandler _handler;

        public PedidoHandlerTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _handler = new PedidoHandler(_pedidoRepositoryMock.Object);
        }

        [Fact]
        public async Task DeveProcessarPedidoComSucesso()
        {
            // Arrange
            var mensagem = new MensagemPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                StatusPedido = (int)StatusPedido.Pendente,
                DataModificacao = DateTime.Now
            };

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(mensagem.PedidoId))
                .ReturnsAsync(new Pedido
                {
                    Id = mensagem.PedidoId,
                    DataCriacao = DateTime.Now,
                    UltimaAtualizacao = DateTime.Now - TimeSpan.FromSeconds(1),
                    Status = StatusPedido.Pendente
                });

            // Act
            await _handler.ProcessarPedidoAsync(mensagem);

            // Assert
            _pedidoRepositoryMock.Verify(r => r.GetByIdAsync(mensagem.PedidoId), Times.Once);
            _pedidoRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task DeveTratarQuandoPedidoNaoEncontrado()
        {
            // Arrange
            var mensagem = new MensagemPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                StatusPedido = (int)StatusPedido.Pendente,
                DataModificacao = DateTime.Now
            };

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(mensagem.PedidoId))
                .ReturnsAsync((Pedido)null);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.ProcessarPedidoAsync(mensagem));

            // Assert
            Assert.Equal($"Pedido with ID {mensagem.PedidoId} not found.", exception.Message);
        }

        [Fact]
        public async Task DeveTratarQuandoDataModificacaoEhAntiga()
        {
            // Arrange
            var mensagem = new MensagemPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                StatusPedido = (int)StatusPedido.Pendente,
                DataModificacao = DateTime.Now.AddHours(-1)
            };

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(mensagem.PedidoId))
                .ReturnsAsync(new Pedido
                {
                    Id = mensagem.PedidoId,
                    DataCriacao = DateTime.Now,
                    UltimaAtualizacao = DateTime.Now,
                    Status = StatusPedido.Pendente
                });

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.ProcessarPedidoAsync(mensagem));

            // Assert
            Assert.Equal(
                $"DataModificacao in the message is older than UltimaAtualizacao for Pedido with ID {mensagem.PedidoId}.",
                exception.Message);
        }

        [Fact]
        public async Task DeveTratarQuandoStatusPedidoEhInvalido()
        {
            // Arrange
            var mensagem = new MensagemPedidoDto
            {
                PedidoId = Guid.NewGuid(),
                StatusPedido = 99,
                DataModificacao = DateTime.Now
            };

            _pedidoRepositoryMock.Setup(r => r.GetByIdAsync(mensagem.PedidoId))
                .ReturnsAsync(new Pedido
                {
                    Id = mensagem.PedidoId,
                    DataCriacao = DateTime.Now,
                    UltimaAtualizacao = DateTime.Now - TimeSpan.FromSeconds(1),
                    Status = StatusPedido.Pendente
                });

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.ProcessarPedidoAsync(mensagem));

            // Assert
            Assert.Equal(
                $"StatusPedido in the message is invalid for Pedido with ID {mensagem.PedidoId}.",
                exception.Message);
        }
    }
}