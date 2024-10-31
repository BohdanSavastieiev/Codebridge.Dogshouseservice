using AutoMapper;
using Codebridge.TechnicalTask.API.Models.Dogs;
using Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;

namespace Codebridge.TechnicalTask.API.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateDogRequest, CreateDogCommand>();
    }
}