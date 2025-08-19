using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests;

public class PutProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutosUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        //Arrange
        var prodId = 14;

        var updateProdutoDto = new ProdutoDTO
        {
            ProdutoId = prodId,
            Nome = "Produto Atualizado",
            Descricao = "Descrição do Produto Atualizado",
            ImagemUrl = "imagematualizada.jpg",
            CategoriaId = 2
        };

        //Act
        var result = await _controller.Put(prodId, updateProdutoDto) as ActionResult<ProdutoDTO>;

        //Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        //Arrange
        var prodId = 999;
        var updateProdutoDto = new ProdutoDTO
        {
            ProdutoId = 14,
            Nome = "Produto Inexistente",
            Descricao = "Descrição do Produto Inexistente",
            ImagemUrl = "imageminexistente.jpg",
            CategoriaId = 2
        };

        //Act
        var data = await _controller.Put(prodId, updateProdutoDto);

        //Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(400);
    }
}
