using MediatR;
using Shared.Application;
using Visualizer.Contract.Commands;
using Visualizer.Contract.Queries;
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

    internal class AddVisualizerToGameCommandHandler(IVisualizerRepository repo) : ICommandHandler<AddVisualizerToGameCommand>
    {
        public async Task Handle(AddVisualizerToGameCommand request, CancellationToken cancellationToken)
        {
            var visualizer = repo.GetById(request.VisualizerId);
            await visualizer.AddToGameAsync(request.GameId);
        }
    }

    internal class RemoveVisualizerFromGameCommandHandler(IVisualizerRepository repo) : ICommandHandler<RemoveVisualizerFromGameCommand>
    {
        public async Task Handle(RemoveVisualizerFromGameCommand request, CancellationToken cancellationToken)
        {
            var visualizer = repo.GetById(request.VisualizerId);
            await visualizer.RemoveFromGameAsync();
        }
    }
}