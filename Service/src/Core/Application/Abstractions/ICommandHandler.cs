using Domain.Primitives.Result;
using MediatR;

namespace Application.Abstractions;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary>
///     Represents the command handler interface.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TResponse">The command response type.</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}