
namespace Software.Api.Catalog;

public interface ILookupVendors
{
    Task<bool> VendorExistsAsync(Guid vendorId);
}