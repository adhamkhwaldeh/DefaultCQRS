using AlJawad.DefaultCQRS.CQRS.Behaviors;
using AlJawad.DefaultCQRS.UnitOfWork;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Distributed;

namespace DefaultCQRS.Validators.Product
{
    public class ProductUpdateValidator : BaseValidator<UpdateProductDto, Entities.Product, long, ProductDto>
    {
        public ProductUpdateValidator(IUnitOfWork unitOfWork, IDistributedCache cache)
           : base(unitOfWork, cache)
        {
            var _repository = unitOfWork.Set<Entities.Product>();
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Required");
            RuleFor(x => x.Price).NotNull().WithMessage("Required");
            RuleFor(e => e).Custom((p, context) =>
            {
                var alreadyExist = _repository.FirstOrDefault(x => x.Name == p.Name && x.Id != p.Id);
                if (alreadyExist != null)
                {
                    context.AddFailure(new ValidationFailure("Name", "Already Existed"));
                }
            });
            //RuleFor(x => x.Address).NotNull().WithMessage(string.Format(MainResource.IsRequired, MainResource.Address));
            //RuleFor(x => x.Locations).NotNull().WithMessage(string.Format(MainResource.IsRequired, MainResource.Locations));
            //RuleFor(x => x).Custom((p, context) => {
            //    if (p.Locations.Points == null || p.Locations.Points.Count() < 1)
            //    {
            //        var Locations = ExpressionExtensions.GetFullPropertyName<Branch, MultiPoint>(p => p.Locations);
            //        context.AddFailure(new ValidationFailure(Locations, ErrorMessages.LocationValidationError));
            //    }
            //});
            //RuleFor(x => x.Description).NotNull().WithMessage(string.Format(MainResource.IsRequired, MainResource.Description));
            //RuleFor(x => x.MobileNumber).NotNull().WithMessage(string.Format(MainResource.IsRequired, MainResource.MobileNumber));
            //RuleFor(x => x.CityId).NotNull().WithMessage(string.Format(MainResource.IsRequired, MainResource.City));
            //RuleFor(x => x.ContractorId).NotNull().WithMessage(string.Format(MainResource.IsRequired, MainResource.ContractorId));
            //RuleFor(e => e).Custom((p, context) =>
            //{
            //    var alreadyExist = _repository.FirstOrDefault(x => x.Name == p.Name && x.ContractorId == p.ContractorId && x.Id != p.Id);
            //    if (alreadyExist != null)
            //    {
            //        var Name = ExpressionExtensions.GetFullPropertyName<Branch, String>(p => p.Name);
            //        context.AddFailure(new ValidationFailure(Name, ErrorMessages.AlreadyExistForContractor));

            //    }
            //});
        }
    }
}