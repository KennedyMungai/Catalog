using Microsoft.AspNetCore.Mvc;

using Catalog.Repositories;

namespace Catalog.Controllers;

//GET /Items
[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly InMemItemsRepository? repository;

    public ItemsController()
    {
        repository = new InMemItemsRepository();
    }
}