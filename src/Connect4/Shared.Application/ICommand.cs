using MediatR;

namespace Shared.Application;

public interface ICommand : IRequest;

public interface ICommand<out TResponse> : IRequest<TResponse>;