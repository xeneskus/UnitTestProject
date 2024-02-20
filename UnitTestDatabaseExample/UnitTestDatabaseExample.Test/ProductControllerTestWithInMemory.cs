using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UnitTestDatabaseExample.SQLITE.Controllers;
using UnitTestDatabaseExample.SQLITE.Models;

namespace UnitTestDatabaseExample.Test;

public class ProductControllerTestWithInMemory : ProductControllerTest
{
    public ProductControllerTestWithInMemory()
    {
        ContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("ornek db").Options;
    }

    [Fact]
    public async Task PostProduct_ValidProduct_ReturnCreated()
    {
        Product newProduct = new Product() { Name = "kalem1", Price = 100, Stock = 200, CategoryId = 1 };
        using (var context = new AppDbContext(ContextOptions))
        {
            var controller = new ProductsController(context);

            var responseResult = await controller.PostProduct(newProduct);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(responseResult.Result);

            Assert.Equal("GetProduct", createdAtActionResult.ActionName);
        }

        using (var context = new AppDbContext(ContextOptions))
        {
            var hasProduct = context.Products.Any(p => p.Name == newProduct.Name);

            Assert.True(hasProduct);
        }
    }
}