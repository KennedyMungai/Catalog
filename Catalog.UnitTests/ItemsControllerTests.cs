using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    private readonly Mock<IItemsRepository> repositoryStub = new Mock<IItemsRepository>();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new Mock<ILogger<ItemsController>>();
    private readonly Random rand = new();

    /// <summary>
    /// A test for the GetItemAsync method that returns null
    /// </summary>
    /// <returns></returns>
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
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    /// <summary>
    /// A test for the GetItemAsync method that returns an item
    /// </summary>
    /// <returns></returns>
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
        result.Value.Should().BeEquivalentTo(
            expectedItem, 
            options => options.ComparingByMembers<Item>()
            );
    }

    /// <summary>
    /// This method creates a random item
    /// </summary>
    /// <returns></returns>
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