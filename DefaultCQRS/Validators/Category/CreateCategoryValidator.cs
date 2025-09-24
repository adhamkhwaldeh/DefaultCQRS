using AlJawad.DefaultCQRS.CQRS.Behaviors;
using DefaultCQRS.DTOs;
using FluentValidation;

namespace DefaultCQRS.Validators.Category
{
    public class CreateCategoryValidator : BaseValidator<CreateCategoryDto, Entities.Category, long, CategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}