namespace WorkerService.Domain;

public class Pedido
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime UltimaAtualizacao { get; set; }
    public StatusPedido Status { get; set; }
}

public enum StatusPedido
{
    Pendente,
    EmProcessamento,
    Concluido,
    Cancelado
}
