using AlJawad.DefaultCQRS.CQRS.Handlers;
using AlJawad.DefaultCQRS.Interfaces;
using AlJawad.DefaultCQRS.Models.Responses;
using AlJawad.DefaultCQRS.UnitOfWork;
using AutoMapper;
using DefaultCQRS.DTOs;
using DefaultCQRS.Entities;
using MediatR;

namespace DefaultCQRS.Handlers
{
    public class CreateCategoryHandler : CreateHandler<CreateCategoryDto, Category, long>
    {
        public CreateCategoryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheManager appCache) : base(unitOfWork, mapper, appCache)
        {
        }
    }
}