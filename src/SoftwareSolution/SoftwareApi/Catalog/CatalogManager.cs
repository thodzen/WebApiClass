using Marten;
using Software.Api.User;

namespace Software.Api.Catalog;

public class CatalogManager(IDocumentSession session, TimeProvider timeProvider, IHttpContextAccessor contextAccessor, IProvideUserInformation userInfoProvider)
{


    public async Task<CatalogItemResponseModel?> GetCatalogItemByAsync(Guid vendorId, Guid catalogId)
    {
        return await session.Query<CatalogItemEntity>()
                .Where(c => c.VendorId == vendorId && c.Id == catalogId)
                .Select(c => new CatalogItemResponseModel
                {
                    Id = c.Id,
                    Description = c.Description,
                    Name = c.Name,
                }).SingleOrDefaultAsync();


    }

    public async Task<CatalogItemResponseModel> CreateCatalogItemAsync(CatalogCreateModel request, Guid vendorId)
    {
        var user = await userInfoProvider.GetUserInformationAsync();

        var entity = new CatalogItemEntity
        {
            Id = Guid.NewGuid(),
            AddedBySub = user.Id.ToString(),
            Created = timeProvider.GetUtcNow(),
            Name = request.Name,
            VendorId = vendorId,
            Description = request.Description,
        };

        session.Store(entity);
        await session.SaveChangesAsync();

        // fake response
        var response = new CatalogItemResponseModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,

        };

        return response;
    }
}
