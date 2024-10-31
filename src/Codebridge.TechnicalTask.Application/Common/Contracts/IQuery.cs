using Codebridge.TechnicalTask.Domain.Shared.Models;
using MediatR;

namespace Codebridge.TechnicalTask.Application.Common.Contracts;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;