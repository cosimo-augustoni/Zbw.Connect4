using MediatR;
using Shared.Application;
using Visualizer.Application.Queries;
using Visualizer.Domain.VisualizerAggregate;

namespace Visualizer.Application.Commands
{
    internal class UpdateVisualizerCommandHandler(IVisualizerRepository repo, ISender mediator) : ICommandHandler<UpdateVisualizerCommand>
    {
        public async Task Handle(UpdateVisualizerCommand request, CancellationToken cancellationToken)
        {
            var visualizer = repo.GetById(request.VisualizerId);
            var visualizerDetail = await mediator.Send(new VisualizerByKeyQuery { VisualizerId = request.VisualizerId }, cancellationToken);

            if(visualizerDetail.Name != request.Name)
                await visualizer.ChangeNameAsync(request.Name);

            if(visualizerDetail.ExternalId != request.ExternalId)
                await visualizer.ChangeExternalIdAsync(request.ExternalId);
        }
    }
}