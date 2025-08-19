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

public class GetProdutoUnitTest : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutoUnitTest(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProdutosById()
    {
        // Arrange 
        var prodId = 2;

        // Act
        var data = await _controller.Get(prodId);

        // Assert (xUnit)
        //var result = Assert.IsType<ActionResult<IEnumerable<ProdutoDTO>>>(data.Result);
        //Assert.Equal(200, (result.Result as OkObjectResult)?.StatusCode);

        //Assert (FluentAssertions)
        data.Result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutosByIdNotFound()
    {
        // Arrange 
        var prodId = 1000;
        // Act
        var data = await _controller.Get(prodId);

        // Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>().Which.StatusCode.Should().Be(404);
    }
    [Fact]
    public async Task GetProdutos_Return_ListOfProdutosDTO()
    {
        // Act
        var data = await _controller.Get();

        // Assert
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
            .And.NotBeNull();
    }
    [Fact]
    public async Task GetProdutos_Return_BadRequestResult()
    {
        //Act
        var data = await _controller.Get();

        // Assert
        data.Result.Should().BeOfType<BadRequestResult>();
    }
}
