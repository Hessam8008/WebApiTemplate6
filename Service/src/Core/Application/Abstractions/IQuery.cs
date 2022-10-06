using Domain.Primitives.Result;
using MediatR;

namespace Application.Abstractions;

/// <summary>
///     Represents the query interface.
/// </summary>
/// <typeparam name="TResponse">The query response type.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}