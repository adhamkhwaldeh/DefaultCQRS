using AlJawad.DefaultCQRS.CQRS.Behaviors;
using AlJawad.DefaultCQRS.UnitOfWork;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Distributed;

namespace DefaultCQRS.Validators.Product
{

    public class ProductCreateValidator : BaseValidator<CreateProductDto,Entities.Product, long, ProductDto>
    {
        public ProductCreateValidator(IUnitOfWork unitOfWork, IDistributedCache cache)
            : base(unitOfWork, cache)
        {

            var _repository = unitOfWork.Set<Entities.Product>();

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Required");
            RuleFor(x => x.Price).NotNull().WithMessage("Required");
            RuleFor(e => e).Custom((p, context) =>
            {
                var alreadyExist = _repository.FirstOrDefault(x => x.Name == p.Name);
                if (alreadyExist != null)
                {
                    context.AddFailure(new ValidationFailure("Name", "Already Existed"));
                }
            });
        }

    }
}