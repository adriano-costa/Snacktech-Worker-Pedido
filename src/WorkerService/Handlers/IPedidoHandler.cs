namespace WorkerService.Handlers;

using WorkerService.DTOs;

public interface IPedidoHandler
{
    Task ProcessarPedidoAsync(MensagemPedidoDto mensagem);
}