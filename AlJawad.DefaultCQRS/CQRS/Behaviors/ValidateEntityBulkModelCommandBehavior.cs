using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Behaviors
{
    public class ValidateEntityBulkModelCommandBehavior<TEntityModel, TResponse> :

        PipelineBehaviorBase<EntityBulkModelCommand<TEntityModel, ResponseArray<TResponse>>, ResponseArray<TResponse>>

    {
        private readonly IEnumerable<IValidator> _validator;
        public ValidateEntityBulkModelCommandBehavior(ILoggerFactory loggerFactory, IEnumerable<IValidator<TEntityModel>> validator) : base(loggerFactory)
        {
            _validator = validator;
        }

        protected override async Task<ResponseArray<TResponse>> Process(EntityBulkModelCommand<TEntityModel, ResponseArray<TResponse>> request, CancellationToken cancellationToken, RequestHandlerDelegate<ResponseArray<TResponse>> next)
        {
            var errors = new List<List<ValidationFailure>>();
            foreach (var item in request.Model)
            {
                var context = new ValidationContext<TEntityModel>(item);
                var failures = _validator
                    .Select(v => v.Validate(context))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();
                errors.Add(failures);
            }
            return errors.Any()
                   ? await Errors(errors).ConfigureAwait(false)
                   : await next().ConfigureAwait(false);

        }
        private static async Task<ResponseArray<TResponse>> Errors(IEnumerable<IEnumerable<ValidationFailure>> failures)
        {
            var response = new ResponseArray<TResponse>();
            foreach (var failure in failures)
            {
                foreach (var item in failure)
                {
                    response.Errors.Add(item.PropertyName, item.ErrorMessage);
                }
            }
            response.ReturnStatus = false;
            response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response.ReturnMessage.Add("Validation Error");
            return await Task.FromResult(response);
        }


    }
}
