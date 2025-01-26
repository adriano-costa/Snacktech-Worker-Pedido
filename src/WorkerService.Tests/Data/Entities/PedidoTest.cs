using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using WorkerService.Enums;
using Xunit;

namespace WorkerService.Data.Entities;

public class PedidoTests
{
    [Fact]
    public void DeveRetornarIdInformado()
    {
        // Arrange
        var pedido = new Pedido();
        var id = Guid.NewGuid();

        // Act
        pedido.Id = id;

        // Assert
        Assert.Equal<Guid>(id, pedido.Id);
    }

    [Fact]
    public void DeveRetornarDataCriacaoInformada()
    {
        // Arrange
        var pedido = new Pedido();
        var dataCriacao = DateTime.Now;

        // Act
        pedido.DataCriacao = dataCriacao;

        // Assert
        Assert.Equal<DateTime>(dataCriacao, pedido.DataCriacao);
    }

    [Fact]
    public void DeveRetornarDataAtualizacaoInformada()
    {
        // Arrange
        var pedido = new Pedido();
        var dataAtualizacao = DateTime.Now;

        // Act
        pedido.UltimaAtualizacao = dataAtualizacao;

        // Assert
        Assert.Equal<DateTime>(pedido.UltimaAtualizacao, dataAtualizacao);
    }

    [Fact]
    public void DeveRetornarStatusInformado()
    {
        // Arrange
        var pedido = new Pedido();
        var status = StatusPedido.Pendente;

        // Act
        pedido.Status = status;

        // Assert
        Assert.Equal<StatusPedido>(status, pedido.Status);
    }
}