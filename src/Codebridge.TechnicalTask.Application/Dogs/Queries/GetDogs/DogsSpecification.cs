using Codebridge.TechnicalTask.Application.Common.Constants;
using Codebridge.TechnicalTask.Application.Common.Extensions;
using Codebridge.TechnicalTask.Application.Common.Models;
using Codebridge.TechnicalTask.Domain.Dogs.Entities;
using Codebridge.TechnicalTask.Domain.Shared.Specifications;

namespace Codebridge.TechnicalTask.Application.Dogs.Queries.GetDogs;

public class DogsSpecification : BaseSpecification<Dog>
{
    public DogsSpecification(GetDogsQuery request)
    {
        if (request.SortParameters is null)
        {
            AddOrderBy(d => d.Name);
        }
        else
        {
            var isDescending = request.SortParameters.Order == SortOrder.Desc;
            
            switch (request.SortParameters.PropertyName.ToLower())
            {
                case "name":
                    if (isDescending)
                        AddOrderByDescending(d => d.Name);
                    else
                        AddOrderBy(d => d.Name);
                    break;
                case "weight":
                    if (isDescending)
                        AddOrderByDescending(d => d.Weight);
                    else
                        AddOrderBy(d => d.Weight);
                    break;
                case "tail_length":
                    if (isDescending)
                        AddOrderByDescending(d => d.TailLength);
                    else
                        AddOrderBy(d => d.TailLength);
                    break;
                case "color":
                    if (isDescending)
                        AddOrderByDescending(d => d.Color);
                    else
                        AddOrderBy(d => d.Color);
                    break;
                default:
                    throw new InvalidOperationException("Invalid sort parameter provided.");
            }
        }

        var skip = (request.PaginationParameters.PageNumber - 1) * request.PaginationParameters.PageSize;
        ApplyPaging(skip, request.PaginationParameters.PageSize);
    }
}