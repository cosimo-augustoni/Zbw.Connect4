using Shared.Application;
using Visualizer.Contract.Commands;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Application.Commands
{
    internal class ChangeVisualizerStatusCommandHandler(IVisualizerRepository repo) : ICommandHandler<ChangeVisualizerStatusCommand>
    {
        public async Task Handle(ChangeVisualizerStatusCommand request, CancellationToken cancellationToken)
        {
            var visualizer = repo.GetById(request.VisualizerId);
            await visualizer.ChangeStatusAsync(request.Status);
        }
    }
}