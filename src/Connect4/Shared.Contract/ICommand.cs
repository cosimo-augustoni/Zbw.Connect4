using MediatR;

namespace Shared.Contract;

public interface ICommand : IRequest;

public interface ICommand<out TResponse> : IRequest<TResponse>;