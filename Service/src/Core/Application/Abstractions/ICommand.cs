using Domain.Primitives.Result;
using MediatR;

namespace Application.Abstractions;

/// <summary>
///     Represent the command interface.
/// </summary>
/// <seealso cref="MediatR.IRequest&lt;Domain.Primitives.Result.Result&gt;" />
public interface ICommand : IRequest<Result>
{
}

/// <summary>
///     Represents the command interface.
/// </summary>
/// <typeparam name="TResponse">The command response type.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}


