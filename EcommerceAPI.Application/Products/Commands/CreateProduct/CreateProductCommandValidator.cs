using FluentValidation;

namespace EcommerceAPI.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(p => p.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("StockQuantity cannot be negative.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");
        }
    }
}
