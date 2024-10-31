using AutoMapper;
using Codebridge.TechnicalTask.Application.Dogs.Common;
using Codebridge.TechnicalTask.Domain.Common;
using Codebridge.TechnicalTask.Domain.Common.Constants;
using Codebridge.TechnicalTask.Domain.Dogs;
using Codebridge.TechnicalTask.Domain.Dogs.Repositories;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using MediatR;

namespace Codebridge.TechnicalTask.Application.Dogs.Queries.GetDog;

public class GetDogQueryHandler : IRequestHandler<GetDogQuery, Result<DogDto>>
{
    private readonly IDogRepository _dogRepository;
    private readonly IMapper _mapper;

    public GetDogQueryHandler(IDogRepository dogRepository, IMapper mapper)
    {
        _dogRepository = dogRepository;
        _mapper = mapper;
    }

    public async Task<Result<DogDto>> Handle(GetDogQuery request, CancellationToken cancellationToken)
    {
        var dog = await _dogRepository.FindAsync(request.Name, cancellationToken);

        return dog is null
            ? Result.Failure<DogDto>(DomainErrors.Dog.NotFound) 
            : Result.Success(_mapper.Map<DogDto>(dog));
    }
}