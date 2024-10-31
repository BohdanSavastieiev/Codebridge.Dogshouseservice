using AutoMapper;
using Codebridge.TechnicalTask.Application.Dogs;
using Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;
using Codebridge.TechnicalTask.Application.Dogs.Common;
using Codebridge.TechnicalTask.Domain.Dogs;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;

namespace Codebridge.TechnicalTask.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateDogCommand, Dog>();
        CreateMap<Dog, DogDto>();
    }
}