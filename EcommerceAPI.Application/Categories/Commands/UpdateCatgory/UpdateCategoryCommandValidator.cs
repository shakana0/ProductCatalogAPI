using FluentValidation;

namespace EcommerceAPI.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Category Id must be greater than zero.");

            RuleFor(c => c.Name)
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Name));

            RuleFor(c => c.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(c => !string.IsNullOrWhiteSpace(c.Description));
        }
    }
}
