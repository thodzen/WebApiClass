namespace Software.Api.Catalog;

public class CatalogItemEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid VendorId { get; set; }
    public string AddedBySub { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }

}