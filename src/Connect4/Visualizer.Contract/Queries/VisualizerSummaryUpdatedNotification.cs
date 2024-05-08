using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Visualizer.Contract.Queries
{
    public class VisualizerSummaryUpdatedNotification : INotification
    {
        public required VisualizerId VisualizerId { get; init; }
    }
}
