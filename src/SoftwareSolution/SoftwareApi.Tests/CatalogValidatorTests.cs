using FluentValidation.TestHelper;
using Software.Api.Catalog;

namespace SoftwareApi.Tests;

[Trait("Category", "Unit")]
public class CatalogValidatorTests
{
    [Fact]
    public void Thingy()
    {
        var validator = new CatalogCreateModelValidator();
        var model = new CatalogCreateModel
        {
            Name = "",
            Description = "Editor for Programmers"
        };
        var result = validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(m => m.Name).WithErrorMessage("Come on Give us a name!");
        result.ShouldNotHaveValidationErrorFor(m => m.Description);
    }

    [Theory, MemberData(nameof(GetValidExamples))]
    public void ValidExamples(CatalogCreateModel model)
    {
        var validator = new CatalogCreateModelValidator();
        var result = validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory, MemberData(nameof(GetInValidExamples))]

    public void InvalidExamples(CatalogCreateModel model)
    {
        var validator = new CatalogCreateModelValidator();
        var result = validator.TestValidate(model);
        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetValidExamples()
    {
        yield return new object[]
        {
            new CatalogCreateModel() { Name = "Visual Studio Code", Description = "Editor for Programmers" }
        };
        yield return new object[]
        {
            new CatalogCreateModel() { Name = "Rider", Description = "IDE for Programmers" }
        };
        yield return new object[]
        { 
            //  right at the edge of the minimum length
            new CatalogCreateModel() { Name = new string('x',5), Description = new string('X', 1024)}
        };
    }

    public static IEnumerable<object[]> GetInValidExamples()
    {
        yield return new object[]
        {
            new CatalogCreateModel() { Name = "aa", Description = "E" }
        };
        yield return new object[]
        { 
            // too short description
            new CatalogCreateModel() { Name = "Rider", Description = "IDE" }
        };
        yield return new object[]
        { 
            // empty name
            new CatalogCreateModel() { Name = "", Description = "IDE" }
        };
        yield return new object[]
        { 
            // empty name and description
            new CatalogCreateModel() { Name = "", Description = "" }
        };
        yield return new object[]
        { 
            // empty name and description
            new CatalogCreateModel() { Name = null!, Description = null! }
        };
        yield return new object[]
        { 
            //  name too long.
            new CatalogCreateModel() { Name = new string('X', 101), Description = "Good Value Here"}
        };
        yield return new object[]
        { 
            //  name too long.
            new CatalogCreateModel() { Name = "Valid Name Here", Description = new string('X', 1025)}
        };

    }


}