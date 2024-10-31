using Codebridge.TechnicalTask.Application.Common.Contracts;
using Codebridge.TechnicalTask.Application.Dogs.Common;

namespace Codebridge.TechnicalTask.Application.Dogs.Commands.CreateDog;

public record CreateDogCommand(string Name, string Color, double TailLength, double Weight) 
    : ICommand<DogDto>;