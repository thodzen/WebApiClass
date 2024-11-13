using Microsoft.AspNetCore.Authorization;

namespace Software.Api.Vendors;

[Produces("application/json")]
public class VendorsApi(VendorManager vendorManager) : ControllerBase
{
    [HttpPost("/vendors")]
    [Authorize(Policy = "IsSoftwareManager")]

    public async Task<ActionResult> AddVendorAsync([FromBody] VendorCreateModel request, [FromServices] VendorCreateModelValidator validator)
    {

        var validations = await validator.ValidateAsync(request);

        if (!validations.IsValid)
        {
            return BadRequest();
        }
        var response = await vendorManager.AddVendorAsync(request);
        return Created($"/vendors/{response.Id}", response);
    }


    /// <summary>
    /// This is how you get a vendor
    /// </summary>
    /// <param name="id">The id of the vendor you are looking for</param>
    /// <returns></returns>
    [HttpGet("/vendors/{id:guid}")]

    public async Task<ActionResult> GetVendorById(Guid id, CancellationToken token)
    {


        VendorResponseModel? response = await vendorManager.GetVendorByIdAsync(id, token);


        if (response is null)
        {
            return NotFound();
        }
        else
        {
            return Ok(response);
        }
    }

    [HttpGet("/vendors")]
    public async Task<ActionResult<CollectionResponse<VendorResponseModel>>> GetAllVendors()
    {
        IReadOnlyList<VendorResponseModel> data = await vendorManager.GetAllVendorsAsync();

        var response = new CollectionResponse<VendorResponseModel> { Data = data };

        return Ok(response);
    }

}
