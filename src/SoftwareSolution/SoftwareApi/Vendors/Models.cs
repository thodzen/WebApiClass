using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Software.Api.Vendors;


public record VendorCreateModel
{
    public string Name { get; init; } = string.Empty;

}

public class VendorCreateModelValidator : AbstractValidator<VendorCreateModel>
{
    public VendorCreateModelValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
    }
}

public record VendorResponseModel
{
    [Required]
    public Guid Id { get; set; }
    [Required, MaxLength(255)]
    public string Name { get; init; } = string.Empty;
}

public class CollectionResponse<T>
{
    public IReadOnlyList<T>? Data { get; set; }
}