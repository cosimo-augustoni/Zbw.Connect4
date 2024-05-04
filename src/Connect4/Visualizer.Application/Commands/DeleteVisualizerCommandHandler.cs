using Shared.Application;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Application.Commands
{
    internal class DeleteVisualizerCommandHandler(IVisualizerRepository repo) : ICommandHandler<DeleteVisualizerCommand>
    {
        public async Task Handle(DeleteVisualizerCommand request, CancellationToken cancellationToken)
        {
            var visualizer = repo.GetById(request.VisualizerId);
            await visualizer.DeleteAsync();
        }
    }
}