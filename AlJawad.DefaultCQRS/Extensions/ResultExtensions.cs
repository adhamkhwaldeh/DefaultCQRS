using AlJawad.DefaultCQRS.Models.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//TODO need to be checked
//using ProperMan.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProperMan.Infrastructure.Extensions
{
    public static class ResultExtensions
    {
        static IMapper _mapper;

        //static AutoOptions _autoOptions;
        public static void Configur(IMapper mapper
            //, IStoreContext storeContext
            )
        {
            _mapper = mapper;
            //_storeContext = storeContext;
            //_autoOptions = new AutoOptions
            //{
            //    StoreContext = storeContext
            //};
        }


        #region ActionResult
        public static ActionResult<Response<TDto>> AsActionResult<TDto>(this Response<TDto> model)
        {
            return new JsonResult(model);
        }

        public static ActionResult AsActionResult<TDto>(this ResponseArray<TDto> model)
        {
            return new JsonResult(model);
        }

        public static ActionResult AsActionResult<TEntity, TDto>(this ResponseArray<TEntity> model)
        {
            try
            {
                var response = new ResponseArray<TDto>
                {
                    Data = _mapper.Map<IEnumerable<TDto>>(model.Data//, x => x.Options(_autoOptions)
                                                                           ),
                    Status = model.Status
                };
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static ActionResult AsActionResult<TDto>(this ResponseList<TDto> model)
        {
            return new JsonResult(model);
        }
        #endregion

        public static IActionResult AsResult<TEntity, TDto>(this ResponseList<TEntity> model)
        {
            try
            {
                var response = new ResponseList<TDto>
                {
                    Page = model.Page,
                    PageCount = model.PageCount,
                    PageSize = model.PageSize,
                    Total = model.Total,
                    Data = _mapper.Map<IEnumerable<TDto>>(model.Data//, x => x.Options(_autoOptions)
                    )
                };
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static IActionResult AsResult<TEntity, TDto>(this ResponseArray<TEntity> model)
        {
            try
            {
                var response = new ResponseArray<TDto>
                {
                    Data = _mapper.Map<IEnumerable<TDto>>(model.Data//, x => x.Options(_autoOptions)
                                                                    ),
                    Status = model.Status
                };
                return new JsonResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static IActionResult AsResult<TEntity, TDto>(this TEntity model)
        {
            var response = new Response<TDto>
            {
                Data = _mapper.Map<TDto>(model//, x => x.Options(_autoOptions)
                )
            };
            return new JsonResult(response);
        }
        public static IActionResult AsResult<TDto>(this TDto model)
        {
            var response = new Response<TDto>
            {
                Data = model
            };
            return new JsonResult(response);
        }


        public static IActionResult AsResult<TDto>(this Response<TDto> model)
        {
            return new JsonResult(model);
        }
        public static IActionResult AsResult<TDto>(this ResponseList<TDto> model)
        {
            return new JsonResult(model);
        }
        public static IActionResult AsResult(this ResponseBool model)
        {
            return new JsonResult(model);
        }
        public static IActionResult AsResult(this ResponseResult model)
        {
            return new JsonResult(model);
        }
        public static ResponseList<TDto> AsResponse<TSource, TDto>(this ResponseList<TSource> model)
        {
            var response = new ResponseList<TDto>
            {
                Page = model.Page,
                PageCount = model.PageCount,
                PageSize = model.PageSize,
                Total = model.Total,
                Data = _mapper.Map<IEnumerable<TDto>>(model.Data//, x => x.Options(_autoOptions)
                                                                )
            };

            return response;
        }
        public static Response<TSource> AsResponse<TSource>(this TSource model)
        {
            var response = new Response<TSource>
            {
                Data = model,
                Status = true,
            };
            return response;
        }
        public static ResponseArray<TSource> AsResponse<TSource>(this IEnumerable<TSource> model)
        {
            var response = new ResponseArray<TSource>
            {
                Data = model,
                Status = true,
            };
            return response;
        }
        public static TDestination Map<TSource, TDestination>(this TSource model)
            where TSource : class
            where TDestination : class
        {
            var dto = _mapper.Map<TDestination>(model//, x => x.Options(_autoOptions)
            );
            return dto;
        }
        public static TDestination Map<TDestination>(this object model)
            where TDestination : class
        {
            var dto = _mapper.Map<TDestination>(model//, x => x.Options(_autoOptions)
                                                     );
            return dto;
        }


        //#region Object result
        public static ObjectResult GenerateResponse<T>(this Response<T> response)
        {
            ObjectResult objectResult = new ObjectResult(response);

            if (response.StatusCode == 100)
                objectResult.StatusCode = StatusCodes.Status100Continue;
            else if (response.StatusCode == 101)
                objectResult.StatusCode = StatusCodes.Status101SwitchingProtocols;
            else if (response.StatusCode == 102)
                objectResult.StatusCode = StatusCodes.Status102Processing;
            else if (response.StatusCode == 200)
                objectResult.StatusCode = StatusCodes.Status200OK;
            else if (response.StatusCode == 201)
                objectResult.StatusCode = StatusCodes.Status201Created;
            else if (response.StatusCode == 202)
                objectResult.StatusCode = StatusCodes.Status202Accepted;
            else if (response.StatusCode == 203)
                objectResult.StatusCode = StatusCodes.Status203NonAuthoritative;
            else if (response.StatusCode == 204)
                objectResult.StatusCode = StatusCodes.Status204NoContent;
            else if (response.StatusCode == 205)
                objectResult.StatusCode = StatusCodes.Status205ResetContent;
            else if (response.StatusCode == 206)
                objectResult.StatusCode = StatusCodes.Status206PartialContent;
            else if (response.StatusCode == 207)
                objectResult.StatusCode = StatusCodes.Status207MultiStatus;
            else if (response.StatusCode == 208)
                objectResult.StatusCode = StatusCodes.Status208AlreadyReported;
            else if (response.StatusCode == 226)
                objectResult.StatusCode = StatusCodes.Status226IMUsed;
            else if (response.StatusCode == 300)
                objectResult.StatusCode = StatusCodes.Status300MultipleChoices;
            else if (response.StatusCode == 301)
                objectResult.StatusCode = StatusCodes.Status301MovedPermanently;
            else if (response.StatusCode == 302)
                objectResult.StatusCode = StatusCodes.Status302Found;
            else if (response.StatusCode == 303)
                objectResult.StatusCode = StatusCodes.Status303SeeOther;
            else if (response.StatusCode == 304)
                objectResult.StatusCode = StatusCodes.Status304NotModified;
            else if (response.StatusCode == 305)
                objectResult.StatusCode = StatusCodes.Status305UseProxy;
            else if (response.StatusCode == 306)
                objectResult.StatusCode = StatusCodes.Status306SwitchProxy;
            else if (response.StatusCode == 307)
                objectResult.StatusCode = StatusCodes.Status307TemporaryRedirect;
            else if (response.StatusCode == 308)
                objectResult.StatusCode = StatusCodes.Status308PermanentRedirect;
            else if (response.StatusCode == 400)
                objectResult.StatusCode = StatusCodes.Status400BadRequest;
            else if (response.StatusCode == 401)
                objectResult.StatusCode = StatusCodes.Status401Unauthorized;
            else if (response.StatusCode == 402)
                objectResult.StatusCode = StatusCodes.Status402PaymentRequired;
            else if (response.StatusCode == 403)
                objectResult.StatusCode = StatusCodes.Status403Forbidden;
            else if (response.StatusCode == 404)
                objectResult.StatusCode = StatusCodes.Status404NotFound;
            else if (response.StatusCode == 405)
                objectResult.StatusCode = StatusCodes.Status405MethodNotAllowed;
            else if (response.StatusCode == 406)
                objectResult.StatusCode = StatusCodes.Status406NotAcceptable;
            else if (response.StatusCode == 407)
                objectResult.StatusCode = StatusCodes.Status407ProxyAuthenticationRequired;
            else if (response.StatusCode == 408)
                objectResult.StatusCode = StatusCodes.Status408RequestTimeout;
            else if (response.StatusCode == 409)
                objectResult.StatusCode = StatusCodes.Status409Conflict;
            else if (response.StatusCode == 410)
                objectResult.StatusCode = StatusCodes.Status410Gone;
            else if (response.StatusCode == 411)
                objectResult.StatusCode = StatusCodes.Status411LengthRequired;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status412PreconditionFailed;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413PayloadTooLarge;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413RequestEntityTooLarge;
            else if (response.StatusCode == 414)
                objectResult.StatusCode = StatusCodes.Status414RequestUriTooLong;
            else if (response.StatusCode == 415)
                objectResult.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RequestedRangeNotSatisfiable;
            else if (response.StatusCode == 417)
                objectResult.StatusCode = StatusCodes.Status417ExpectationFailed;
            else if (response.StatusCode == 418)
                objectResult.StatusCode = StatusCodes.Status418ImATeapot;
            else if (response.StatusCode == 419)
                objectResult.StatusCode = StatusCodes.Status419AuthenticationTimeout;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status421MisdirectedRequest;
            else if (response.StatusCode == 422)
                objectResult.StatusCode = StatusCodes.Status422UnprocessableEntity;
            else if (response.StatusCode == 423)
                objectResult.StatusCode = StatusCodes.Status423Locked;
            else if (response.StatusCode == 424)
                objectResult.StatusCode = StatusCodes.Status424FailedDependency;
            else if (response.StatusCode == 426)
                objectResult.StatusCode = StatusCodes.Status426UpgradeRequired;
            else if (response.StatusCode == 428)
                objectResult.StatusCode = StatusCodes.Status428PreconditionRequired;
            else if (response.StatusCode == 429)
                objectResult.StatusCode = StatusCodes.Status429TooManyRequests;
            else if (response.StatusCode == 431)
                objectResult.StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge;
            else if (response.StatusCode == 451)
                objectResult.StatusCode = StatusCodes.Status451UnavailableForLegalReasons;
            else if (response.StatusCode == 500)
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;
            else if (response.StatusCode == 501)
                objectResult.StatusCode = StatusCodes.Status501NotImplemented;
            else if (response.StatusCode == 502)
                objectResult.StatusCode = StatusCodes.Status502BadGateway;
            else if (response.StatusCode == 503)
                objectResult.StatusCode = StatusCodes.Status503ServiceUnavailable;
            else if (response.StatusCode == 504)
                objectResult.StatusCode = StatusCodes.Status504GatewayTimeout;
            else if (response.StatusCode == 505)
                objectResult.StatusCode = StatusCodes.Status505HttpVersionNotsupported;
            else if (response.StatusCode == 506)
                objectResult.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
            else if (response.StatusCode == 507)
                objectResult.StatusCode = StatusCodes.Status507InsufficientStorage;
            else if (response.StatusCode == 508)
                objectResult.StatusCode = StatusCodes.Status508LoopDetected;
            else if (response.StatusCode == 510)
                objectResult.StatusCode = StatusCodes.Status510NotExtended;
            else if (response.StatusCode == 511)
                objectResult.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
            else
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return objectResult;
        }

        public static ObjectResult GenerateResponseFrontEnd<T>(this Response<T> response)
        {
            ObjectResult objectResult = new ObjectResult(response);

            if (response.StatusCode == 100)
                objectResult.StatusCode = StatusCodes.Status100Continue;
            else if (response.StatusCode == 101)
                objectResult.StatusCode = StatusCodes.Status101SwitchingProtocols;
            else if (response.StatusCode == 102)
                objectResult.StatusCode = StatusCodes.Status102Processing;
            else if (response.StatusCode == 200)
                objectResult.StatusCode = StatusCodes.Status200OK;
            else if (response.StatusCode == 201)
                objectResult.StatusCode = StatusCodes.Status201Created;
            else if (response.StatusCode == 202)
                objectResult.StatusCode = StatusCodes.Status202Accepted;
            else if (response.StatusCode == 203)
                objectResult.StatusCode = StatusCodes.Status203NonAuthoritative;
            else if (response.StatusCode == 204)
                objectResult.StatusCode = StatusCodes.Status204NoContent;
            else if (response.StatusCode == 205)
                objectResult.StatusCode = StatusCodes.Status205ResetContent;
            else if (response.StatusCode == 206)
                objectResult.StatusCode = StatusCodes.Status206PartialContent;
            else if (response.StatusCode == 207)
                objectResult.StatusCode = StatusCodes.Status207MultiStatus;
            else if (response.StatusCode == 208)
                objectResult.StatusCode = StatusCodes.Status208AlreadyReported;
            else if (response.StatusCode == 226)
                objectResult.StatusCode = StatusCodes.Status226IMUsed;
            else if (response.StatusCode == 300)
                objectResult.StatusCode = StatusCodes.Status300MultipleChoices;
            else if (response.StatusCode == 301)
                objectResult.StatusCode = StatusCodes.Status301MovedPermanently;
            else if (response.StatusCode == 302)
                objectResult.StatusCode = StatusCodes.Status302Found;
            else if (response.StatusCode == 303)
                objectResult.StatusCode = StatusCodes.Status303SeeOther;
            else if (response.StatusCode == 304)
                objectResult.StatusCode = StatusCodes.Status304NotModified;
            else if (response.StatusCode == 305)
                objectResult.StatusCode = StatusCodes.Status305UseProxy;
            else if (response.StatusCode == 306)
                objectResult.StatusCode = StatusCodes.Status306SwitchProxy;
            else if (response.StatusCode == 307)
                objectResult.StatusCode = StatusCodes.Status307TemporaryRedirect;
            else if (response.StatusCode == 308)
                objectResult.StatusCode = StatusCodes.Status308PermanentRedirect;
            else if (response.StatusCode == 400)
                objectResult.StatusCode = StatusCodes.Status400BadRequest;
            else if (response.StatusCode == 401)
                objectResult.StatusCode = StatusCodes.Status401Unauthorized;
            else if (response.StatusCode == 402)
                objectResult.StatusCode = StatusCodes.Status402PaymentRequired;
            else if (response.StatusCode == 403)
                objectResult.StatusCode = StatusCodes.Status403Forbidden;
            else if (response.StatusCode == 404)
                objectResult.StatusCode = StatusCodes.Status404NotFound;
            else if (response.StatusCode == 405)
                objectResult.StatusCode = StatusCodes.Status405MethodNotAllowed;
            else if (response.StatusCode == 406)
                objectResult.StatusCode = StatusCodes.Status406NotAcceptable;
            else if (response.StatusCode == 407)
                objectResult.StatusCode = StatusCodes.Status407ProxyAuthenticationRequired;
            else if (response.StatusCode == 408)
                objectResult.StatusCode = StatusCodes.Status408RequestTimeout;
            else if (response.StatusCode == 409)
                objectResult.StatusCode = StatusCodes.Status409Conflict;
            else if (response.StatusCode == 410)
                objectResult.StatusCode = StatusCodes.Status410Gone;
            else if (response.StatusCode == 411)
                objectResult.StatusCode = StatusCodes.Status411LengthRequired;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status412PreconditionFailed;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413PayloadTooLarge;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413RequestEntityTooLarge;
            else if (response.StatusCode == 414)
                objectResult.StatusCode = StatusCodes.Status414RequestUriTooLong;
            else if (response.StatusCode == 415)
                objectResult.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RequestedRangeNotSatisfiable;
            else if (response.StatusCode == 417)
                objectResult.StatusCode = StatusCodes.Status417ExpectationFailed;
            else if (response.StatusCode == 418)
                objectResult.StatusCode = StatusCodes.Status418ImATeapot;
            else if (response.StatusCode == 419)
                objectResult.StatusCode = StatusCodes.Status419AuthenticationTimeout;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status421MisdirectedRequest;
            else if (response.StatusCode == 422)
                objectResult.StatusCode = StatusCodes.Status422UnprocessableEntity;
            else if (response.StatusCode == 423)
                objectResult.StatusCode = StatusCodes.Status423Locked;
            else if (response.StatusCode == 424)
                objectResult.StatusCode = StatusCodes.Status424FailedDependency;
            else if (response.StatusCode == 426)
                objectResult.StatusCode = StatusCodes.Status426UpgradeRequired;
            else if (response.StatusCode == 428)
                objectResult.StatusCode = StatusCodes.Status428PreconditionRequired;
            else if (response.StatusCode == 429)
                objectResult.StatusCode = StatusCodes.Status429TooManyRequests;
            else if (response.StatusCode == 431)
                objectResult.StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge;
            else if (response.StatusCode == 451)
                objectResult.StatusCode = StatusCodes.Status451UnavailableForLegalReasons;
            else if (response.StatusCode == 500)
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;
            else if (response.StatusCode == 501)
                objectResult.StatusCode = StatusCodes.Status501NotImplemented;
            else if (response.StatusCode == 502)
                objectResult.StatusCode = StatusCodes.Status502BadGateway;
            else if (response.StatusCode == 503)
                objectResult.StatusCode = StatusCodes.Status503ServiceUnavailable;
            else if (response.StatusCode == 504)
                objectResult.StatusCode = StatusCodes.Status504GatewayTimeout;
            else if (response.StatusCode == 505)
                objectResult.StatusCode = StatusCodes.Status505HttpVersionNotsupported;
            else if (response.StatusCode == 506)
                objectResult.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
            else if (response.StatusCode == 507)
                objectResult.StatusCode = StatusCodes.Status507InsufficientStorage;
            else if (response.StatusCode == 508)
                objectResult.StatusCode = StatusCodes.Status508LoopDetected;
            else if (response.StatusCode == 510)
                objectResult.StatusCode = StatusCodes.Status510NotExtended;
            else if (response.StatusCode == 511)
                objectResult.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
            else
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return objectResult;
        }

        public static ObjectResult GenerateResponsePagination<T>(this ResponseList<T> response)
        {
            ObjectResult objectResult = new ObjectResult(response);

            if (response.StatusCode == 100)
                objectResult.StatusCode = StatusCodes.Status100Continue;
            else if (response.StatusCode == 101)
                objectResult.StatusCode = StatusCodes.Status101SwitchingProtocols;
            else if (response.StatusCode == 102)
                objectResult.StatusCode = StatusCodes.Status102Processing;
            else if (response.StatusCode == 200)
                objectResult.StatusCode = StatusCodes.Status200OK;
            else if (response.StatusCode == 201)
                objectResult.StatusCode = StatusCodes.Status201Created;
            else if (response.StatusCode == 202)
                objectResult.StatusCode = StatusCodes.Status202Accepted;
            else if (response.StatusCode == 203)
                objectResult.StatusCode = StatusCodes.Status203NonAuthoritative;
            else if (response.StatusCode == 204)
                objectResult.StatusCode = StatusCodes.Status204NoContent;
            else if (response.StatusCode == 205)
                objectResult.StatusCode = StatusCodes.Status205ResetContent;
            else if (response.StatusCode == 206)
                objectResult.StatusCode = StatusCodes.Status206PartialContent;
            else if (response.StatusCode == 207)
                objectResult.StatusCode = StatusCodes.Status207MultiStatus;
            else if (response.StatusCode == 208)
                objectResult.StatusCode = StatusCodes.Status208AlreadyReported;
            else if (response.StatusCode == 226)
                objectResult.StatusCode = StatusCodes.Status226IMUsed;
            else if (response.StatusCode == 300)
                objectResult.StatusCode = StatusCodes.Status300MultipleChoices;
            else if (response.StatusCode == 301)
                objectResult.StatusCode = StatusCodes.Status301MovedPermanently;
            else if (response.StatusCode == 302)
                objectResult.StatusCode = StatusCodes.Status302Found;
            else if (response.StatusCode == 303)
                objectResult.StatusCode = StatusCodes.Status303SeeOther;
            else if (response.StatusCode == 304)
                objectResult.StatusCode = StatusCodes.Status304NotModified;
            else if (response.StatusCode == 305)
                objectResult.StatusCode = StatusCodes.Status305UseProxy;
            else if (response.StatusCode == 306)
                objectResult.StatusCode = StatusCodes.Status306SwitchProxy;
            else if (response.StatusCode == 307)
                objectResult.StatusCode = StatusCodes.Status307TemporaryRedirect;
            else if (response.StatusCode == 308)
                objectResult.StatusCode = StatusCodes.Status308PermanentRedirect;
            else if (response.StatusCode == 400)
                objectResult.StatusCode = StatusCodes.Status400BadRequest;
            else if (response.StatusCode == 401)
                objectResult.StatusCode = StatusCodes.Status401Unauthorized;
            else if (response.StatusCode == 402)
                objectResult.StatusCode = StatusCodes.Status402PaymentRequired;
            else if (response.StatusCode == 403)
                objectResult.StatusCode = StatusCodes.Status403Forbidden;
            else if (response.StatusCode == 404)
                objectResult.StatusCode = StatusCodes.Status404NotFound;
            else if (response.StatusCode == 405)
                objectResult.StatusCode = StatusCodes.Status405MethodNotAllowed;
            else if (response.StatusCode == 406)
                objectResult.StatusCode = StatusCodes.Status406NotAcceptable;
            else if (response.StatusCode == 407)
                objectResult.StatusCode = StatusCodes.Status407ProxyAuthenticationRequired;
            else if (response.StatusCode == 408)
                objectResult.StatusCode = StatusCodes.Status408RequestTimeout;
            else if (response.StatusCode == 409)
                objectResult.StatusCode = StatusCodes.Status409Conflict;
            else if (response.StatusCode == 410)
                objectResult.StatusCode = StatusCodes.Status410Gone;
            else if (response.StatusCode == 411)
                objectResult.StatusCode = StatusCodes.Status411LengthRequired;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status412PreconditionFailed;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413PayloadTooLarge;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413RequestEntityTooLarge;
            else if (response.StatusCode == 414)
                objectResult.StatusCode = StatusCodes.Status414RequestUriTooLong;
            else if (response.StatusCode == 415)
                objectResult.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RequestedRangeNotSatisfiable;
            else if (response.StatusCode == 417)
                objectResult.StatusCode = StatusCodes.Status417ExpectationFailed;
            else if (response.StatusCode == 418)
                objectResult.StatusCode = StatusCodes.Status418ImATeapot;
            else if (response.StatusCode == 419)
                objectResult.StatusCode = StatusCodes.Status419AuthenticationTimeout;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status421MisdirectedRequest;
            else if (response.StatusCode == 422)
                objectResult.StatusCode = StatusCodes.Status422UnprocessableEntity;
            else if (response.StatusCode == 423)
                objectResult.StatusCode = StatusCodes.Status423Locked;
            else if (response.StatusCode == 424)
                objectResult.StatusCode = StatusCodes.Status424FailedDependency;
            else if (response.StatusCode == 426)
                objectResult.StatusCode = StatusCodes.Status426UpgradeRequired;
            else if (response.StatusCode == 428)
                objectResult.StatusCode = StatusCodes.Status428PreconditionRequired;
            else if (response.StatusCode == 429)
                objectResult.StatusCode = StatusCodes.Status429TooManyRequests;
            else if (response.StatusCode == 431)
                objectResult.StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge;
            else if (response.StatusCode == 451)
                objectResult.StatusCode = StatusCodes.Status451UnavailableForLegalReasons;
            else if (response.StatusCode == 500)
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;
            else if (response.StatusCode == 501)
                objectResult.StatusCode = StatusCodes.Status501NotImplemented;
            else if (response.StatusCode == 502)
                objectResult.StatusCode = StatusCodes.Status502BadGateway;
            else if (response.StatusCode == 503)
                objectResult.StatusCode = StatusCodes.Status503ServiceUnavailable;
            else if (response.StatusCode == 504)
                objectResult.StatusCode = StatusCodes.Status504GatewayTimeout;
            else if (response.StatusCode == 505)
                objectResult.StatusCode = StatusCodes.Status505HttpVersionNotsupported;
            else if (response.StatusCode == 506)
                objectResult.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
            else if (response.StatusCode == 507)
                objectResult.StatusCode = StatusCodes.Status507InsufficientStorage;
            else if (response.StatusCode == 508)
                objectResult.StatusCode = StatusCodes.Status508LoopDetected;
            else if (response.StatusCode == 510)
                objectResult.StatusCode = StatusCodes.Status510NotExtended;
            else if (response.StatusCode == 511)
                objectResult.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
            else
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return objectResult;
        }

        public static ObjectResult GenerateResponseList<T>(this ResponseArray<T> response)
        {
            ObjectResult objectResult = new ObjectResult(response);

            if (response.StatusCode == 100)
                objectResult.StatusCode = StatusCodes.Status100Continue;
            else if (response.StatusCode == 101)
                objectResult.StatusCode = StatusCodes.Status101SwitchingProtocols;
            else if (response.StatusCode == 102)
                objectResult.StatusCode = StatusCodes.Status102Processing;
            else if (response.StatusCode == 200)
                objectResult.StatusCode = StatusCodes.Status200OK;
            else if (response.StatusCode == 201)
                objectResult.StatusCode = StatusCodes.Status201Created;
            else if (response.StatusCode == 202)
                objectResult.StatusCode = StatusCodes.Status202Accepted;
            else if (response.StatusCode == 203)
                objectResult.StatusCode = StatusCodes.Status203NonAuthoritative;
            else if (response.StatusCode == 204)
                objectResult.StatusCode = StatusCodes.Status204NoContent;
            else if (response.StatusCode == 205)
                objectResult.StatusCode = StatusCodes.Status205ResetContent;
            else if (response.StatusCode == 206)
                objectResult.StatusCode = StatusCodes.Status206PartialContent;
            else if (response.StatusCode == 207)
                objectResult.StatusCode = StatusCodes.Status207MultiStatus;
            else if (response.StatusCode == 208)
                objectResult.StatusCode = StatusCodes.Status208AlreadyReported;
            else if (response.StatusCode == 226)
                objectResult.StatusCode = StatusCodes.Status226IMUsed;
            else if (response.StatusCode == 300)
                objectResult.StatusCode = StatusCodes.Status300MultipleChoices;
            else if (response.StatusCode == 301)
                objectResult.StatusCode = StatusCodes.Status301MovedPermanently;
            else if (response.StatusCode == 302)
                objectResult.StatusCode = StatusCodes.Status302Found;
            else if (response.StatusCode == 303)
                objectResult.StatusCode = StatusCodes.Status303SeeOther;
            else if (response.StatusCode == 304)
                objectResult.StatusCode = StatusCodes.Status304NotModified;
            else if (response.StatusCode == 305)
                objectResult.StatusCode = StatusCodes.Status305UseProxy;
            else if (response.StatusCode == 306)
                objectResult.StatusCode = StatusCodes.Status306SwitchProxy;
            else if (response.StatusCode == 307)
                objectResult.StatusCode = StatusCodes.Status307TemporaryRedirect;
            else if (response.StatusCode == 308)
                objectResult.StatusCode = StatusCodes.Status308PermanentRedirect;
            else if (response.StatusCode == 400)
                objectResult.StatusCode = StatusCodes.Status400BadRequest;
            else if (response.StatusCode == 401)
                objectResult.StatusCode = StatusCodes.Status401Unauthorized;
            else if (response.StatusCode == 402)
                objectResult.StatusCode = StatusCodes.Status402PaymentRequired;
            else if (response.StatusCode == 403)
                objectResult.StatusCode = StatusCodes.Status403Forbidden;
            else if (response.StatusCode == 404)
                objectResult.StatusCode = StatusCodes.Status404NotFound;
            else if (response.StatusCode == 405)
                objectResult.StatusCode = StatusCodes.Status405MethodNotAllowed;
            else if (response.StatusCode == 406)
                objectResult.StatusCode = StatusCodes.Status406NotAcceptable;
            else if (response.StatusCode == 407)
                objectResult.StatusCode = StatusCodes.Status407ProxyAuthenticationRequired;
            else if (response.StatusCode == 408)
                objectResult.StatusCode = StatusCodes.Status408RequestTimeout;
            else if (response.StatusCode == 409)
                objectResult.StatusCode = StatusCodes.Status409Conflict;
            else if (response.StatusCode == 410)
                objectResult.StatusCode = StatusCodes.Status410Gone;
            else if (response.StatusCode == 411)
                objectResult.StatusCode = StatusCodes.Status411LengthRequired;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status412PreconditionFailed;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413PayloadTooLarge;
            else if (response.StatusCode == 413)
                objectResult.StatusCode = StatusCodes.Status413RequestEntityTooLarge;
            else if (response.StatusCode == 414)
                objectResult.StatusCode = StatusCodes.Status414RequestUriTooLong;
            else if (response.StatusCode == 415)
                objectResult.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RangeNotSatisfiable;
            else if (response.StatusCode == 416)
                objectResult.StatusCode = StatusCodes.Status416RequestedRangeNotSatisfiable;
            else if (response.StatusCode == 417)
                objectResult.StatusCode = StatusCodes.Status417ExpectationFailed;
            else if (response.StatusCode == 418)
                objectResult.StatusCode = StatusCodes.Status418ImATeapot;
            else if (response.StatusCode == 419)
                objectResult.StatusCode = StatusCodes.Status419AuthenticationTimeout;
            else if (response.StatusCode == 412)
                objectResult.StatusCode = StatusCodes.Status421MisdirectedRequest;
            else if (response.StatusCode == 422)
                objectResult.StatusCode = StatusCodes.Status422UnprocessableEntity;
            else if (response.StatusCode == 423)
                objectResult.StatusCode = StatusCodes.Status423Locked;
            else if (response.StatusCode == 424)
                objectResult.StatusCode = StatusCodes.Status424FailedDependency;
            else if (response.StatusCode == 426)
                objectResult.StatusCode = StatusCodes.Status426UpgradeRequired;
            else if (response.StatusCode == 428)
                objectResult.StatusCode = StatusCodes.Status428PreconditionRequired;
            else if (response.StatusCode == 429)
                objectResult.StatusCode = StatusCodes.Status429TooManyRequests;
            else if (response.StatusCode == 431)
                objectResult.StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge;
            else if (response.StatusCode == 451)
                objectResult.StatusCode = StatusCodes.Status451UnavailableForLegalReasons;
            else if (response.StatusCode == 500)
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;
            else if (response.StatusCode == 501)
                objectResult.StatusCode = StatusCodes.Status501NotImplemented;
            else if (response.StatusCode == 502)
                objectResult.StatusCode = StatusCodes.Status502BadGateway;
            else if (response.StatusCode == 503)
                objectResult.StatusCode = StatusCodes.Status503ServiceUnavailable;
            else if (response.StatusCode == 504)
                objectResult.StatusCode = StatusCodes.Status504GatewayTimeout;
            else if (response.StatusCode == 505)
                objectResult.StatusCode = StatusCodes.Status505HttpVersionNotsupported;
            else if (response.StatusCode == 506)
                objectResult.StatusCode = StatusCodes.Status506VariantAlsoNegotiates;
            else if (response.StatusCode == 507)
                objectResult.StatusCode = StatusCodes.Status507InsufficientStorage;
            else if (response.StatusCode == 508)
                objectResult.StatusCode = StatusCodes.Status508LoopDetected;
            else if (response.StatusCode == 510)
                objectResult.StatusCode = StatusCodes.Status510NotExtended;
            else if (response.StatusCode == 511)
                objectResult.StatusCode = StatusCodes.Status511NetworkAuthenticationRequired;
            else
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return objectResult;
        }

        //#endregion

    }

}
