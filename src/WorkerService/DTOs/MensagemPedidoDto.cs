namespace WorkerService.DTOs;

public class MensagemPedidoDto
{
    public Guid PedidoId { get; set; }
    public DateTime DataModificacao { get; set; }
    public int StatusPedido { get; set; }
    public string Mensagem { get; set; }
}