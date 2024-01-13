using MediatR;

namespace Application
{
    public record class Query<TResponse> : IRequest<TResponse>;
}
