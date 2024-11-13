using Marten;
using Software.Api.Catalog;

namespace Software.Api.User;

public class UserApi(IDocumentSession session) : ControllerBase
{
    [HttpGet("/techs")]
    public async Task<ActionResult> GetAllTechs()
    {
        var techs = await session.Query<UserInformation>().ToListAsync();
        return Ok(new { data = techs });
    }

    [HttpGet("/techs/{id:guid}/software")]
    public async Task<ActionResult> GetSoftwareForTech(Guid id)
    {
        // 404?

        // TODO: HAve the user feature, create an interface that the CatalogManager implements.
        var response = await session.Query<CatalogItemEntity>()
            .Where(c => c.AddedBySub == id.ToString())
            .ToListAsync();

        return Ok(new { data = response });
    }
}
