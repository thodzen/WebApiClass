using FluentValidation;

namespace Software.Api.Catalog;

public record CatalogCreateModel
{

    public string Name { get; init; } = string.Empty;


    public string Description { get; init; } = string.Empty;
}

public class CatalogCreateModelValidator : AbstractValidator<CatalogCreateModel>
{
    public CatalogCreateModelValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("Come on Give us a name!");

        RuleFor(c => c.Name).MinimumLength(5).MaximumLength(100);
        RuleFor(c => c.Description).NotEmpty().MinimumLength(10).MaximumLength(1024);
        //RuleFor(c => c.Name).MustAsync(async (c,x) => await session.Query<CatalogItemEntity>().AnyAsync(i => i.Name == c) == false);
    }
}

public record CatalogItemResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}