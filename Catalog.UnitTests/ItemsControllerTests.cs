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
    private readonly TimeSpan milliseconds = new TimeSpan(1000000);

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
    /// This method testst the GetItemsAsync method
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
        // Given
        var expectedItems = new[]{CreateRandomItem(), CreateRandomItem(), CreateRandomItem()};

        repositoryStub.Setup(repo => repo.GetItemsAsync())
            .ReturnsAsync(expectedItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
        // When
        var actualItems = await controller.GetItemsAsync();
    
        // Then
        actualItems.Should().BeEquivalentTo(
            expectedItems,
            options => options.ComparingByMembers<Item>()
            );
    }

    /// <summary>
    /// This tests CreatedItemAsync controller method
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        // Given
        var itemToCreate = new CreateItemDto()
        {
            Name = Guid.NewGuid().ToString(),
            Price = rand.Next(1000)
        };

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
        // When
        var result = await controller.CreateItemAsync(itemToCreate);

        // Then
        var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
        itemToCreate.Should().BeEquivalentTo(
            createdItem, 
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
        
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, milliseconds);
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