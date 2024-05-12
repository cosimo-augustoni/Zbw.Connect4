using Connect4.Frontend.Shared;
using Game.Contract;
using Game.Contract.Commands;
using Game.Domain.GameProjections;
using MediatR;
using Microsoft.AspNetCore.Components;
using Visualizer.Contract.Queries;
using Visualizer.Physical.Infrastructure;

namespace Connect4.Frontend.Components.Pages
{
    public partial class Home : IDisposable
    {
        [Inject] private IMediator Mediator { get; set; } = null!;

        [Inject] private IVisualizerMqttClient MqttClient { get; set; } = null!;

        [Inject] private IGameLobbiesQuery GameLobbiesQuery { get; set; } = null!;

        [Inject] private VisualizerChangedEventHandler VisualizerChangedEventHandler { get; set; } = null!;

        private Guid? GameId { get; set; }
        public IReadOnlyList<GameLobby> Games { get; set; } = [];
        public string? MqttPayload { get; set; } = "3";
        public string MqttTopic { get; set; } = "R001_to_IT";
        private IReadOnlyList<VisualizerDto>? visualizers;

        protected override async Task OnInitializedAsync()
        {
            await this.LoadVisualizersAsync();
            this.VisualizerChangedEventHandler.VisualizerUpdated += this.OnVisualizerUpdated;
        }

        private async Task OnVisualizerUpdated(object? sender, VisualizerChangedEventArgs e)
        {
            await this.LoadVisualizersAsync();
            await this.InvokeAsync(this.StateHasChanged);
        }

        private async Task LoadVisualizersAsync()
        {
            this.visualizers = await this.Mediator.Send(new AllVisualizersQuery());
        }

        private async Task CreateGame()
        {
            Console.WriteLine("My debug output.");
            this.GameId = await this.Mediator.Send(new CreateGameCommand());
        }

        private async Task RenameGame()
        {
            if (this.GameId == null)
                return;

            await this.Mediator.Send(new UpdateGameNameCommand(new GameId(this.GameId.Value), "New Game Name"));
        }

        private async Task GetGames()
        {
            this.Games = await this.GameLobbiesQuery.GetAllLobbies();
        }

        private void SendToBroker()
        {
            if (string.IsNullOrWhiteSpace(this.MqttPayload))
                return;

            this.MqttClient.PublishAsync(this.MqttTopic, this.MqttPayload);
        }

        public void Dispose()
        {
            this.VisualizerChangedEventHandler.VisualizerUpdated -= this.OnVisualizerUpdated;
        }
    }
}