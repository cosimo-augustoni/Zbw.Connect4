using Shared.Application;
using Visualizer.Contract.Commands;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Application.Commands
{
    internal class CreateVisualizerCommandHandler(IVisualizerRepository repo) : ICommandHandler<CreateVisualizerCommand, Guid>
    {
        public async Task<Guid> Handle(CreateVisualizerCommand request, CancellationToken cancellationToken)
        {
            var visualizer = repo.Create();
            return await visualizer.CreateVisualizer(request.Name, request.ExternalId);
        }
    }
}