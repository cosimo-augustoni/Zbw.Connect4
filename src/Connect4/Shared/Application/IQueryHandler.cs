using MediatR;

namespace Application
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : Query<TResponse>;
}
