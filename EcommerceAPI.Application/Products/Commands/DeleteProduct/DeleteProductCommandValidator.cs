using FluentValidation;

namespace EcommerceAPI.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Product Id must be greater than zero.");
        }
    }
}
