using Moq;
using Xunit;
using WorkerService.Data.Entities;
using WorkerService.Data.Repository;
using WorkerService.Data;
using Microsoft.EntityFrameworkCore;

namespace WorkerService.Tests.Data.Repository
{
    public class PedidoRepositoryTests
    {
        private readonly Mock<PedidoContext> _mockContext;
        private readonly IPedidoRepository _repository;

        public PedidoRepositoryTests()
        {
            var options = new DbContextOptions<PedidoContext>();
            _mockContext = new Mock<PedidoContext>(options);
            _repository = new PedidoRepository(_mockContext.Object);
        }

        [Fact]
        public async Task Get_PedidoByIdAsync_ReturnsCorrectPedido()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedPedido = new Pedido
            {
                Id = expectedId,
                // Add other necessary properties for your test case
            };

            _mockContext.Setup(context => context.Pedidos.FindAsync(expectedId))
                        .ReturnsAsync(expectedPedido);

            // Act
            var result = await _repository.GetByIdAsync(expectedId);

            // Assert
            Assert.Equal(expectedId, result.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_PedidoByIdAsync_ReturnsNullWhenPedidoNotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            _mockContext.Setup(context => context.Pedidos.FindAsync(nonExistentId))
                        .ReturnsAsync((Pedido)null);

            // Act
            var result = await _repository.GetByIdAsync(nonExistentId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_PedidoAsync_UpdatesAndSavesChanges()
        {
            // Arrange
            var existingPedido = new Pedido
            {
                Id = Guid.NewGuid(),
                UltimaAtualizacao = DateTime.Now
            };

            _mockContext.Setup(context => context.Pedidos.FindAsync(existingPedido.Id))
                        .ReturnsAsync(existingPedido);
            var pedidoToUpdate = await _repository.GetByIdAsync(existingPedido.Id);
            pedidoToUpdate!.UltimaAtualizacao = DateTime.Now.AddMinutes(1);

            // Act
            await _repository.UpdateAsync(pedidoToUpdate);

            // Assert
            _mockContext.Verify(context => context.Pedidos.Update(pedidoToUpdate), Times.Once);
            _mockContext.Verify(context => context.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}