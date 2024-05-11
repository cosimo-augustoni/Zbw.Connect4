using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Visualizer.Contract.Commands;

namespace Connect4.Frontend.Visualizer
{
    public partial class VisualizerCreationButton : IDisposable
    {
        [Inject] private ISender Mediator { get; set; } = null!;

        [Inject] private IDialogService DialogService { get; set; } = null!;

        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private async Task CreateNewVisualizer()
        {
            var parameters = new DialogParameters<VisualizerEditDialog>();
            parameters.Add(d => d.Visualizer, new VisualizerEditDialog.VisualizerModel());

            var dialog = await this.DialogService.ShowAsync<VisualizerEditDialog>("Roboter erfassen", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var visualizerModel = (VisualizerEditDialog.VisualizerModel)result.Data;
                await this.Mediator.Send(new CreateVisualizerCommand
                {
                    Name = visualizerModel.Name ?? throw new ArgumentNullException(),
                    ExternalId = visualizerModel.ExternalId ?? throw new ArgumentNullException(),
                });

                this.Snackbar.Add("Roboter erfasst", Severity.Success);
            }
        }

        public void Dispose()
        {
            this.Snackbar.Dispose();
        }
    }
}