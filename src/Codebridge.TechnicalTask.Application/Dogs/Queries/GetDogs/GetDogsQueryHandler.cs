using AutoMapper;
using Codebridge.TechnicalTask.Application.Common;
using Codebridge.TechnicalTask.Application.Common.Models;
using Codebridge.TechnicalTask.Application.Dogs.Common;
using Codebridge.TechnicalTask.Domain.Dogs;
using Codebridge.TechnicalTask.Domain.Dogs.Repositories;
using Codebridge.TechnicalTask.Domain.Shared.Models;
using MediatR;

namespace Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;

public class GetDogsQueryHandler : IRequestHandler<GetDogsQuery, Result<PaginatedList<DogDto>>>
{
    private readonly IDogRepository _dogRepository;
    private readonly IMapper _mapper;

    public GetDogsQueryHandler(IMapper mapper, IDogRepository dogRepository)
    {
        _mapper = mapper;
        _dogRepository = dogRepository;
    }

    public async Task<Result<PaginatedList<DogDto>>> Handle(GetDogsQuery request, CancellationToken cancellationToken)
    {
        var spec = new DogsSpecification(request);
        var dogs = await _dogRepository.GetListAsync(spec, cancellationToken);
        var dogDtos = _mapper.Map<List<DogDto>>(dogs);
        
        var totalCount = await _dogRepository.CountAsync(cancellationToken);
        return Result.Success(
            new PaginatedList<DogDto>(
                items: dogDtos,
                totalCount: totalCount,
                pageNumber: request.PaginationParameters.PageNumber,
                pageSize: request.PaginationParameters.PageSize));

    }
}