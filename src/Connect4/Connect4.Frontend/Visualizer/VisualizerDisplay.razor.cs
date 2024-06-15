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
        public required VisualizerId? VisualizerId { get; init; }

        [Parameter]
        public required bool Editable { get; init; }

        [Parameter]
        public required bool ShowImage { get; init; } = true;

        private VisualizerDto? Visualizer { get; set; }

        public string VisualizerImage
        {
            get
            {
                if (this.VisualizerId == null)
                    return "";

                using var sha = MD5.Create();
                var imageCount = 8;
                var hashedGuid = sha.ComputeHash(this.VisualizerId.Id.ToByteArray());
                int imageId = Math.Abs(BitConverter.ToInt32(hashedGuid, 0)) % imageCount + 1;
                return $"{imageId}.jpeg";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await this.TryLoadVisualizerAsync();
            this.VisualizerChangedEventHandler.VisualizerUpdated += this.OnVisualizerUpdated;
            await base.OnInitializedAsync();
        }

        private async Task OnVisualizerUpdated(object sender, VisualizerChangedEventArgs e)
        {
            if(await this.TryLoadVisualizerAsync())
                await this.InvokeAsync(this.StateHasChanged);
        }

        protected override async Task OnParametersSetAsync()
        {
            await this.TryLoadVisualizerAsync();
            await base.OnParametersSetAsync();
        }

        private async Task<bool> TryLoadVisualizerAsync()
        {
            if (this.VisualizerId == null)
            {
                this.Visualizer = null;
                return false;
            }

            this.Visualizer = await this.Mediator.Send(new VisualizerByKeyQuery { VisualizerId = this.VisualizerId });
            return true;
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
            if (!result.Canceled && !this.Visualizer.IsInGame)
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
            if (this.VisualizerId == null)
                return;

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
