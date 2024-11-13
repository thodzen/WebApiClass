using Microsoft.AspNetCore.Authorization;

namespace Software.Api.Catalog;

public class AddingSoftwareController(CatalogManager catalogManager, ILookupVendors vendorLookup) : ControllerBase
{

    [HttpGet("/vendors/{vendorId}/catalog/{catalogId}")]
    public async Task<ActionResult> GetCatalogItemByIdAsync(Guid vendorId, Guid catalogId)
    {
        // Write the Code You wish You Had

        CatalogItemResponseModel? response = await catalogManager.GetCatalogItemByAsync(vendorId, catalogId);

        if (response == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }

    [HttpPost("/vendors/{vendorId:guid}/catalog")]
    [Authorize(Policy = "IsSoftwareCenter")]
    public async Task<ActionResult> CanAddSoftware(
        [FromBody] CatalogCreateModel request,
        [FromRoute] Guid vendorId,
        [FromServices] CatalogCreateModelValidator validator
        )
    {

        var validity = await validator.ValidateAsync(request);
        if (!validity.IsValid)
        {
            return BadRequest(); // 400
        }

        if (await vendorLookup.VendorExistsAsync(vendorId))
        {

            // validate the body (the owned data) // FluentValidation, DataAttributes, or just write some code.
            // if those are bad, send appropriate http response messages

            // uh, actually do something with this? do the "unsafe" thing. (add it to the database)
            CatalogItemResponseModel response = await catalogManager.CreateCatalogItemAsync(request, vendorId);
            return Ok(response); // 201 Created - with a Location header.
        }
        else
        {
            return NotFound();
        }
    }
}
