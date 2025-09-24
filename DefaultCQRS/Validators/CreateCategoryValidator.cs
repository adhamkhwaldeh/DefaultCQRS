using DefaultCQRS.DTOs;
using FluentValidation;

namespace DefaultCQRS.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}