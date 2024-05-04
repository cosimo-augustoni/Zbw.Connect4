using MediatR;

namespace Shared.Application
{
    public interface IQuery<out TResponse> : IRequest<TResponse>;
}