using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Adicione este using
using Microsoft.Extensions.Logging.Abstractions; // Adicione este using


namespace ApiCatalogoxUnitTests.UnitTests;

public class ProdutosUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString =
        "Server=Bebeto\\SQLSERVER;Database=CatalogoDB;Trusted_Connection=True;TrustServerCertificate=True;";

    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    public ProdutosUnitTestController()
    {
        var loggerFactory = new NullLoggerFactory();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProdutoDTOMappingProfile());
        }, loggerFactory);

        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }
}
