using MediatR;

namespace Shared.Contract
{
    public interface IQuery<out TResponse> : IRequest<TResponse>;
}