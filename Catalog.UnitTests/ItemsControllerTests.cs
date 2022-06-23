using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    private readonly Mock<IItemsRepository> repositoryStub = new Mock<IItemsRepository>();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new Mock<ILogger<ItemsController>>();
    private readonly Random rand = new();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
        // Arrange
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Item)null);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        // Given
        var expectedItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
        // When
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Then
        Assert.IsType<ItemDto>(result.Value);
        var dto = (result as ActionResult<ItemDto>).Value;
        Assert.Equal(expectedItem.Id, dto.Id);
        Assert.Equal(expectedItem.Name, dto.Name);
        Assert.Equal(expectedItem.Price, dto.Price);
        Assert.Equal(expectedItem.CreatedDate, dto.CreatedDate);
    }

    private Item CreateRandomItem()
    {
        return new()
        {
            Id = Guid.NewGuid(), 
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}