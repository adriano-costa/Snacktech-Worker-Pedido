using WorkerService.Data.Entities;
using WorkerService.Data.Repository;
using WorkerService.DTOs;
using WorkerService.Enums;

namespace WorkerService.Handlers {
    public class PedidoHandler : IPedidoHandler
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoHandler(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task ProcessarPedidoAsync(MensagemPedidoDto mensagem)
        {
            try
            {
                // Fetch the current state of the pedido from the database
                Pedido pedido = await _pedidoRepository.GetByIdAsync(mensagem.PedidoId);

                if (pedido == null)
                {
                    // If the PedidoID does not exist in the database, throw an exception
                    throw new InvalidOperationException($"Pedido with ID {mensagem.PedidoId} not found.");
                }

                // Validate the message and update logic
                if (mensagem.DataModificacao < pedido.UltimaAtualizacao)
                {
                    // If DataModificacao in the message is before UltimaAtualizacao, throw an exception
                    throw new InvalidOperationException($"DataModificacao in the message is older than UltimaAtualizacao for Pedido with ID {mensagem.PedidoId}.");
                }

                if (!Enumerable.Contains((StatusPedido[])Enum.GetValues(typeof(StatusPedido)), (StatusPedido)mensagem.StatusPedido)){
                    // If StatusPedido in the message is not valid, throw an exception
                    throw new InvalidOperationException($"StatusPedido in the message is invalid for Pedido with ID {mensagem.PedidoId}.");
                }

                // Update the pedido if validations pass
                pedido.Status = (StatusPedido)mensagem.StatusPedido;
                pedido.UltimaAtualizacao = mensagem.DataModificacao;

                // Persist the updated pedido back to the database
                await _pedidoRepository.UpdateAsync(pedido);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it
                Console.WriteLine($"Error processing pedido: {ex.Message}");
                throw;
            }
        }
    }
}
