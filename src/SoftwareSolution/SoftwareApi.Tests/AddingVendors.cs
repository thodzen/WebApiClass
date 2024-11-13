
using Alba;
using Alba.Security;
using Software.Api.Vendors;
using System.Security.Claims;

namespace SoftwareApi.Tests;
[Trait("Category", "SystemTest")]
public class AddingVendors
{

    // vendors have a name, and are assigned an ID.
    // only software-center-managers can add vendors

    [Fact]
    public async Task CanAddAVendorAsync()
    {

        // Someone named Joan is a software center admin.
        var fakeIdentity = new AuthenticationStub().WithName("Joan")
           .With(new System.Security.Claims.Claim(ClaimTypes.Role, "software-center-admin"));


        var host = await AlbaHost.For<Program>(fakeIdentity);

        // she wants to add the company "Jetbrains" to the list of approved vendors.
        var vendorToAdd = new VendorCreateModel { Name = "Jetbrains" };

        // She does this by posting some json to the /vendors resource.
        var postResponse = await host.Scenario(api =>
        {

            api.Post.Json(vendorToAdd).ToUrl("/vendors");
            api.StatusCodeShouldBe(201); // Thank to this PR https://github.com/JasperFx/alba/pull/161
        });

        Assert.NotNull(postResponse);

        var postBody = await postResponse.ReadAsJsonAsync<VendorResponseModel>();

        // Since the scenario above was successful (Ok), we should be able to get that vendor through the API

        var getResponse = await host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{postBody.Id}");
            api.StatusCodeShouldBeOk();
        });

        Assert.NotNull(postBody);



        var getBody = await getResponse.ReadAsJsonAsync<VendorResponseModel>();

        // If they are the same, we good here.
        Assert.Equal(postBody, getBody);

    }
}
