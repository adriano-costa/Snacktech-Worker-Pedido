using WorkerService.Domain;

namespace WorkerService.Data.Repository
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(Guid pedidoId);
        Task UpdateAsync(Pedido pedido);
    }
}