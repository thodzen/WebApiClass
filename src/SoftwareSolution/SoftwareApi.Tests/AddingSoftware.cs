
using Alba;
using Alba.Security;
using Software.Api.Catalog;
using System.Security.Claims;
using Testcontainers.PostgreSql;

namespace SoftwareApi.Tests;
[Trait("Category", "SystemTest")]
public class AddingSoftware : IAsyncLifetime
{
    private IAlbaHost _host = null!;

    private PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
        .WithImage("jeffrygonzalez/software-test:test-data")
        .WithUsername("user")
        .WithPassword("password")
        .WithDatabase("software")
        .Build();

    [Fact]
    public async Task CanAddAnItemToTheCatalog()
    {
        // given
        // start up an instance of the api 


        var vendorId = Guid.Parse("d18a9612-d16d-44d0-8654-de4bb33730c7"); // Id for Test Data Vendor of Microsoft
        var requestBody = new CatalogCreateModel
        {
            Name = "Visual Studio Code",
            Description = "Editor for Programmers"
        };


        // Post a new piece of software to the catalog

        var responseFromPost = await _host.Scenario(api =>
        {
            api.Post
            .Json(requestBody)
            .ToUrl($"/vendors/{vendorId}/catalog");

            api.StatusCodeShouldBeOk();
        });
        // I want to post this data to this url 
        // and I should get back this status code
        // and I should be able to look that up again..
        // verify it.

        Assert.NotNull(responseFromPost);
        var postResponseModel = await responseFromPost.ReadAsJsonAsync<CatalogItemResponseModel>();

        Assert.Equal(requestBody.Name, postResponseModel.Name);
        Assert.Equal(requestBody.Description, postResponseModel.Description);

        // Get that newly created piece of software from the API
        var responseFromGet = await _host.Scenario(api =>
        {
            api.Get.Url($"/vendors/{vendorId}/catalog/{postResponseModel.Id}");
            api.StatusCodeShouldBeOk();

        });

        Assert.NotNull(responseFromGet);

        var getResponseModel = await responseFromGet.ReadAsJsonAsync<CatalogItemResponseModel>();

        Assert.Equal(getResponseModel, postResponseModel);

    }

    public async Task InitializeAsync()
    {

        await _postgresContainer.StartAsync(); // that will actually start the container.


        var fakeIdentity = new AuthenticationStub()
           .WithName("bob-smith")
           .With(new System.Security.Claims.Claim(ClaimTypes.Role, "software-center"));
        // when
        _host = await AlbaHost.For<Program>(cfg =>
        {
            var connectionString = _postgresContainer.GetConnectionString();
            cfg.UseSetting("ConnectionStrings:software", connectionString);
        }, fakeIdentity);
    }
    public async Task DisposeAsync()
    {
        await _host.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }



    [Fact]

    public async Task NonSoftwareTeamMembersCannotAddItemsToTheCatalog()
    {
        var vendorId = Guid.Parse("d18a9612-d16d-44d0-8634-de4bb33730c7"); // Id for Test Data Vendor of Microsoft
        var requestBody = new CatalogCreateModel
        {
            Name = "Visual Studio Code",
            Description = "Editor for Programmers"
        };

        // Post a new piece of software to the catalog
        var responseFromPost = await _host.Scenario(api =>
        {
            api.RemoveClaim(ClaimTypes.Role);
            api.Post
            .Json(requestBody)
            .ToUrl($"/vendors/{vendorId}/catalog");

            api.StatusCodeShouldBe(403);
        });

    }

    [Fact]
    public async Task ModelIsValidated()
    {
        var vendorId = Guid.Parse("d18a9612-d16d-44d0-8634-de4bb33730c7");
        await _host.Scenario(api =>
        {
            api.Post.Json(new CatalogCreateModel()).ToUrl($"/vendors/{vendorId}/catalog");
            api.StatusCodeShouldBe(400); // bad request.
        });
    }
}
