using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AlJawad.DefaultCQRS.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using FluentValidation.Results;
using AlJawad.DefaultCQRS.Models.Responses;

namespace AlJawad.DefaultCQRS.CQRS.Behaviors
{
    public class ValidateEntityModelCommandBehavior<TEntityModel, TResponse> :
        PipelineBehaviorBase<EntityModelCommand<TEntityModel, Response<TResponse>>, Response<TResponse>>
    {
        private readonly IEnumerable<IValidator> _validator;
        public ValidateEntityModelCommandBehavior(ILoggerFactory loggerFactory, IEnumerable<IValidator<TEntityModel>> validator) : base(loggerFactory)
        {
            _validator = validator;
        }
        protected override async Task<Response<TResponse>> Process(EntityModelCommand<TEntityModel, Response<TResponse>> request, CancellationToken cancellationToken, RequestHandlerDelegate<Response<TResponse>> next)
        {


            var context = new ValidationContext<TEntityModel>(request.Model);
            var failures = _validator
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();
            //if (failures.Any()) throw new ValidationException(failures);
            //return await next().ConfigureAwait(false);
            return failures.Any()
                ? await Errors(failures).ConfigureAwait(false)
                : await next().ConfigureAwait(false);

        }
        private static async Task<Response<TResponse>> Errors(IEnumerable<ValidationFailure> failures)
        {
            var response = new Response<TResponse>();
            foreach (var failure in failures)
            {
                response.Errors.Add(failure.PropertyName, failure.ErrorMessage);
            }
            response.ReturnStatus = false;
            response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response.ReturnMessage.Add("Validation Error");
            return await Task.FromResult(response);
        }


    }
}
