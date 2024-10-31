using Codebridge.TechnicalTask.Application.Common.Contracts;
using Codebridge.TechnicalTask.Application.Dogs.Common;

namespace Codebridge.TechnicalTask.Application.Dogs.Queries.GetDog;

public record GetDogQuery(string Name) : IQuery<DogDto>;