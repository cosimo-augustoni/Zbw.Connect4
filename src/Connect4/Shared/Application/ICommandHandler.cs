using MediatR;

namespace Application
{
    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : Command;
}
