using System.Security.Cryptography;
using Connect4.Frontend.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Visualizer.Contract;
using Visualizer.Contract.Commands;
using Visualizer.Contract.Queries;

namespace Connect4.Frontend.Visualizer
{
    public partial class VisualizerDisplay : IDisposable
    {
        [Inject]
        private ISender Mediator { get; set; } = null!;

        [Inject]
        private IDialogService DialogService { get; set; } = null!;

        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        private VisualizerChangedEventHandler VisualizerChangedEventHandler { get; set; } = null!;

        [Parameter]
        public required VisualizerId VisualizerId { get; init; }

        [Parameter]
        public required bool Editable { get; init; }

        private VisualizerDto? Visualizer { get; set; }

        public string VisualizerImage
        {
            get
            {
                using var sha = MD5.Create();
                var imageCount = 8;
                var hashedGuid = sha.ComputeHash(this.VisualizerId.Id.ToByteArray());
                int imageId = Math.Abs(BitConverter.ToInt32(hashedGuid, 0)) % imageCount + 1;
                return $"{imageId}.jpeg";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(500);

            this.Visualizer = await this.Mediator.Send(new VisualizerByKeyQuery { VisualizerId = this.VisualizerId });
            this.VisualizerChangedEventHandler.VisualizerUpdated += this.OnVisualizerUpdated;
            await base.OnInitializedAsync();
        }

        private async Task OnVisualizerUpdated(object sender, VisualizerChangedEventArgs e)
        {
            this.Visualizer = await this.Mediator.Send(new VisualizerByKeyQuery { VisualizerId = this.VisualizerId });
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task EditVisualizer()
        {
            if (this.Visualizer == null)
                return;

            var parameters = new DialogParameters<VisualizerEditDialog>();
            parameters.Add(d => d.Visualizer, new VisualizerEditDialog.VisualizerModel
            {
                VisualizerId = this.Visualizer.Id,
                Name = this.Visualizer.Name,
                ExternalId = this.Visualizer.ExternalId,
            });

            var dialog = await this.DialogService.ShowAsync<VisualizerEditDialog>(this.Visualizer.Name, parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var visualizerModel = (VisualizerEditDialog.VisualizerModel)result.Data;
                await this.Mediator.Send(new UpdateVisualizerCommand
                {
                    VisualizerId = visualizerModel.VisualizerId ?? throw new ArgumentNullException(),
                    Name = visualizerModel.Name ?? throw new ArgumentNullException(),
                    ExternalId = visualizerModel.ExternalId ?? throw new ArgumentNullException(),

                });

                this.Snackbar.Add("Roboter gespeichert", Severity.Success);
            }
        }

        private async Task DeleteVisualizer()
        {
            await this.Mediator.Send(new DeleteVisualizerCommand
            {
                VisualizerId = this.VisualizerId
            });
            this.Snackbar.Add("Roboter gelöscht", Severity.Success);
        }

        public void Dispose()
        {
            this.VisualizerChangedEventHandler.VisualizerUpdated -= this.OnVisualizerUpdated;
            this.Snackbar.Dispose();
        }
    }
}
