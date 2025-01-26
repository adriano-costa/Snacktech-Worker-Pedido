using WorkerService.Enums;

namespace WorkerService.Data.Entities;

public class Pedido
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime UltimaAtualizacao { get; set; }
    public StatusPedido Status { get; set; }
}