using AutoMapper;
using Codebridge.TechnicalTask.Application.Dogs.Common;
using Codebridge.TechnicalTask.Domain.Common;
using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Dogs;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Codebridge.TechnicalTask.Domain.Dogs.Repositories;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using MediatR;

namespace Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;

public class CreateDogCommandHandler : IRequestHandler<CreateDogCommand, Result<DogDto>>
{
    private readonly IDogRepository _dogRepository;
    private readonly IMapper _mapper;

    public CreateDogCommandHandler(IDogRepository dogRepository, IMapper mapper)
    {
        _dogRepository = dogRepository;
        _mapper = mapper;
    }

    public async Task<Result<DogDto>> Handle(CreateDogCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await _dogRepository.ExistsAsync(request.Name, cancellationToken);
        if (nameExists)
        {
            return Result.Failure<DogDto>(DomainErrors.Dog.NameExists);
        }
        
        var dog = _mapper.Map<Dog>(request);
        await _dogRepository.AddAsync(dog, cancellationToken);
        return _mapper.Map<DogDto>(dog);
    }
}