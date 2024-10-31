using Codebridge.TechnicalTask.Domain.Shared.Models;
using MediatR;

namespace Codebridge.TechnicalTask.Application.Common.Contracts;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;