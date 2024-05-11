﻿using Shared.Application;
using Visualizer.Contract.Queries;
using Visualizer.Domain.VisualizerProjections;

namespace Visualizer.Application.Queries
{
    internal class VisualizerByKeyQueryHandler(IVisualizerQuery query) : IQueryHandler<VisualizerByKeyQuery, VisualizerDto>
    {
        public async Task<VisualizerDto> Handle(VisualizerByKeyQuery request, CancellationToken cancellationToken)
        {
            var visualizerDetail = await query.GetByIdAsync(request.VisualizerId, cancellationToken);
            return new VisualizerDto
            {
                Id = visualizerDetail.Id,
                Name = visualizerDetail.Name,
                ExternalId = visualizerDetail.ExternalId,
                Status = visualizerDetail.Status
            };
        }
    }
}