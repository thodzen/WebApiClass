using Marten;

namespace Software.Api.Vendors;

public class VendorManager(IDocumentSession session) : Catalog.ILookupVendors
{
    public async Task<VendorResponseModel> AddVendorAsync(VendorCreateModel request)
    {
        var entity = new VendorEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        session.Store(entity);
        await session.SaveChangesAsync();
        var response = new VendorResponseModel
        {
            Id = entity.Id,
            Name = entity.Name
        };
        return response;
    }

    public async Task<VendorResponseModel?> GetVendorByIdAsync(Guid id, CancellationToken token)
    {
        return await session.Query<VendorEntity>()
            .Where(v => v.Id == id)
            .Select(v => new VendorResponseModel
            {
                Id = v.Id,
                Name = v.Name,
            })

            .SingleOrDefaultAsync(token);
    }

    public async Task<IReadOnlyList<VendorResponseModel>> GetAllVendorsAsync()
    {
        return await session.Query<VendorEntity>()
             .Select(v => new VendorResponseModel { Id = v.Id, Name = v.Name })
             .ToListAsync();
    }

    async Task<bool> Catalog.ILookupVendors.VendorExistsAsync(Guid vendorId)
    {
        return await session.Query<VendorEntity>().AnyAsync(v => v.Id == vendorId);
    }
}