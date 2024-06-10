using System.Diagnostics.CodeAnalysis;
using Game.Contract;
using Game.Domain.GameAggregate.Events;
using Shared.Domain;

namespace Game.Domain.GameAggregate
{
    public class Game(GameId id, IEventRegistry<GameEvent> eventRegistry) : AggregateRoot<GameEvent>(eventRegistry), IGame
    {
        public Game(IEventRegistry<GameEvent> eventRegistry, GameId id, string? name) : this(id, eventRegistry)
        {
            this.Name = name;
        }

        public GameId Id { get; } = id;

        private string? Name { get; set; }

        private Player? YellowPlayer { get; set; }

        private Player? RedPlayer { get; set; }

        [MemberNotNullWhen(true, nameof(CurrentPlayingSide))]
        [MemberNotNullWhen(true, nameof(YellowPlayer))]
        [MemberNotNullWhen(true, nameof(RedPlayer))]
        private bool IsRunning => this.GameState == GameState.Running;

        private GameState GameState { get; set; }

        private PlayerSide? CurrentPlayingSide { get; set; }

        private GamePiecePlacementRequest? PlacementRequest { get; set; }

        private Board Board { get; } = new();

        private Player? GetPlayerById(PlayerId playerId)
        {
            if (this.YellowPlayer?.Id == playerId)
                return this.YellowPlayer;
            if (this.RedPlayer?.Id == playerId)
                return this.RedPlayer;

            return null;
        }

        public Task<Board> GetBoardState()
        {
            return Task.FromResult(this.Board);
        }

        public async Task<Guid> CreateGame(string? name)
        {
            await this.RaiseEventAsync(new GameCreatedEvent
            {
                GameId = this.Id,
                Name = name != null ? $"{name}'s Spiel" : "4 Gewinnt Spiel"
            });

            return this.Id.Id;
        }

        public async Task ChangeNameAsync(string name)
        {
            if (this.Name == name)
                return;

            await this.RaiseEventAsync(new GameNameChangedEvent
            {
                GameId = this.Id,
                Name = name
            });
        }

        public async Task AddPlayer(Player player, PlayerSide playerSide)
        {
            if (playerSide == PlayerSide.Red && this.RedPlayer != null)
                throw new PlayerSlotAlreadyOccupiedException(playerSide);
            if (playerSide == PlayerSide.Yellow && this.YellowPlayer != null)
                throw new PlayerSlotAlreadyOccupiedException(playerSide);

            if (this.GetPlayerById(player.Id) != null)
                throw new PlayerAlreadyInGameException();

            await this.RaiseEventAsync(new PlayerAddedEvent
            {
                GameId = this.Id,
                Player = player,
                PlayerSide = playerSide
            });
        }

        public async Task RemovePlayer(PlayerId playerId)
        {
            if (this.IsRunning)
                throw new GameAlreadyStartedException();

            if (this.GetPlayerById(playerId) == null)
                return;

            await this.RaiseEventAsync(new PlayerRemovedEvent
            {
                GameId = this.Id,
                PlayerId = playerId
            });
        }

        public async Task ReadyPlayer(PlayerId playerId)
        {
            if (this.IsRunning)
                throw new GameAlreadyStartedException();

            var playerToReady = this.GetPlayerById(playerId);
            if (playerToReady == null || playerToReady.IsReady)
                return;

            await this.RaiseEventAsync(new PlayerReadiedEvent
            {
                GameId = this.Id,
                PlayerId = playerId
            });
        }

        public async Task StartGame()
        {
            if(!this.YellowPlayer?.IsReady ?? false)
                throw new PlayerNotReadyException();

            if(!this.RedPlayer?.IsReady ?? false)
                throw new PlayerNotReadyException();

            var startingSide = (PlayerSide)new Random().Next(0, 2);
            await this.RaiseEventAsync(new GameStartedEvent
            {
                GameId = this.Id,
                StartingPlayerId = startingSide switch
                {
                    PlayerSide.Red => this.RedPlayer!.Id,
                    PlayerSide.Yellow => this.YellowPlayer!.Id,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });
        }

        public async Task UnreadyPlayer(PlayerId playerId)
        {
            if (this.IsRunning)
                throw new GameAlreadyStartedException();

            var playerToReady = this.GetPlayerById(playerId);
            if (playerToReady is not { IsReady: true })
                return;

            await this.RaiseEventAsync(new PlayerUnreadiedEvent
            {
                GameId = this.Id,
                PlayerId = playerId
            });
        }

        public async Task PlaceGamePiece(BoardPosition boardPosition)
        {
            if (!this.IsRunning)
                throw new GameNotStartedException();

            if (this.Board.IsSlotOccupied(boardPosition))
                throw new SlotAlreadyFilledException();

            var player = this.CurrentPlayingSide == PlayerSide.Yellow ? this.YellowPlayer : this.RedPlayer;
            await this.RaiseEventAsync(new GamePiecePlacementRequestedEvent
            {
                GameId = this.Id,
                RequestingPlayer = player,
                Position = boardPosition
            });
        }

        public async Task NotAcknowledgeGamePiecePlacement(PlayerId playerId)
        {
            if (this.PlacementRequest == null)
                return;

            this.ThrowIfAcknowledgementNotAllowed(playerId);

            await this.RaiseEventAsync(new GamePiecePlacementRejectedEvent
            {
                GameId = this.Id,
                Position = this.PlacementRequest.Position,
            });
        }

        public async Task AcknowledgeGamePiecePlacement(PlayerId playerId)
        {
            if (this.PlacementRequest == null)
                return;

            if (this.PlacementRequest.RequestingPlayer == playerId)
                return;

            this.ThrowIfAcknowledgementNotAllowed(playerId);

            var playingSide = this.GetPlayerSideByPlayerId(this.PlacementRequest.RequestingPlayer);
            await this.RaiseEventAsync(new GamePiecePlacedEvent
            {
                GameId = this.Id,
                Position = this.PlacementRequest.Position,
                PlacedBy = this.GetPlayerById(this.PlacementRequest.RequestingPlayer) ?? throw new ArgumentNullException(),
                PlayingSide = playingSide
            });

            if (this.Board.IsWinningMove(this.PlacementRequest.Position, playingSide))
            {
                await this.RaiseEventAsync(new GameFinishedEvent
                {
                    GameId = this.Id,
                    FinishReason = FinishReason.Win,
                    WinningPlayerId = this.PlacementRequest.RequestingPlayer
                });
            }
            else if (this.Board.FreeSlots == 1)
            {
                await this.RaiseEventAsync(new GameFinishedEvent
                {
                    GameId = this.Id,
                    FinishReason = FinishReason.Draw
                });
            }
        }

        private void ThrowIfAcknowledgementNotAllowed(PlayerId playerId)
        {
            if (this.PlacementRequest == null)
                return;

            if (this.PlacementRequest.RequestingPlayer == this.YellowPlayer?.Id && playerId != this.RedPlayer?.Id)
                throw new GamePiecePlacementAcknowledgementNotAllowedException();

            if (this.PlacementRequest.RequestingPlayer == this.RedPlayer?.Id && playerId != this.YellowPlayer?.Id)
                throw new GamePiecePlacementAcknowledgementNotAllowedException();
        }

        public async Task AbortGame()
        {
            await this.RaiseEventAsync(new GameAbortedEvent
            {
                GameId = this.Id
            });
        }

        public override void Apply(GameEvent @event)
        {
            switch (@event)
            {
                case GameAbortedEvent gameAbortedEvent:
                    this.Apply(gameAbortedEvent);
                    break;
                case GameCreatedEvent gameCreatedEvent:
                    this.Apply(gameCreatedEvent);
                    break;
                case GameFinishedEvent gameFinishedEvent:
                    this.Apply(gameFinishedEvent);
                    break;
                case GameNameChangedEvent gameNameChangedEvent:
                    this.Apply(gameNameChangedEvent);
                    break;
                case GamePiecePlacedEvent gamePiecePlacedEvent:
                    this.Apply(gamePiecePlacedEvent);
                    break;
                case GamePiecePlacementRejectedEvent gamePiecePlacementRejectedEvent:
                    this.Apply(gamePiecePlacementRejectedEvent);
                    break;
                case GamePiecePlacementRequestedEvent gamePiecePlacementRequestedEvent:
                    this.Apply(gamePiecePlacementRequestedEvent);
                    break;
                case GameStartedEvent gameStartedEvent:
                    this.Apply(gameStartedEvent);
                    break;
                case PlayerAddedEvent playerAddedEvent:
                    this.Apply(playerAddedEvent);
                    break;
                case PlayerReadiedEvent playerReadiedEvent:
                    this.Apply(playerReadiedEvent);
                    break;
                case PlayerRemovedEvent playerRemovedEvent:
                    this.Apply(playerRemovedEvent);
                    break;
                case PlayerUnreadiedEvent playerUnreadiedEvent:
                    this.Apply(playerUnreadiedEvent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(@event));
            }
        }

        private void Apply(GameCreatedEvent @event)
        {
            this.Name = @event.Name;
            this.GameState = GameState.Creating;
        }

        private void Apply(GameNameChangedEvent @event)
        {
            this.Name = @event.Name;
        }

        private void Apply(GameAbortedEvent _)
        {
            this.GameState = GameState.Ended;
        }

        private void Apply(GameFinishedEvent _)
        {
            this.GameState = GameState.Ended;
        }

        private void Apply(PlayerAddedEvent @event)
        {
            if (@event.PlayerSide == PlayerSide.Red)
            {
                this.RedPlayer = @event.Player;
            }
            else
            {
                this.YellowPlayer = @event.Player;
            }
        }

        private void Apply(PlayerRemovedEvent @event)
        {
            if (@event.PlayerId == this.RedPlayer?.Id)
            {
                this.RedPlayer = null;
            }
            else if (@event.PlayerId == this.YellowPlayer?.Id)
            {
                this.YellowPlayer = null;
            }
        }

        private void Apply(PlayerReadiedEvent @event)
        {
            this.UpdatePlayerReadyState(@event.PlayerId, true);
        }

        private void Apply(PlayerUnreadiedEvent @event)
        {
            this.UpdatePlayerReadyState(@event.PlayerId, false);
        }

        private void Apply(GameStartedEvent @event)
        {
            this.GameState = GameState.Running;
            this.CurrentPlayingSide = this.GetPlayerSideByPlayerId(@event.StartingPlayerId);
        }

        private void Apply(GamePiecePlacementRequestedEvent @event)
        {
            this.PlacementRequest = new GamePiecePlacementRequest
            {
                RequestingPlayer = @event.RequestingPlayer.Id,
                Position = @event.Position
            };
        }

        private void Apply(GamePiecePlacedEvent @event)
        {
            this.Board.PlacePiece(@event.Position, @event.PlayingSide);
            this.PlacementRequest = null;
            this.CurrentPlayingSide = this.CurrentPlayingSide == PlayerSide.Red ? PlayerSide.Yellow : PlayerSide.Red;
        }

        private void Apply(GamePiecePlacementRejectedEvent _)
        {
            this.PlacementRequest = null;
        }

        private void UpdatePlayerReadyState(PlayerId playerId, bool isReady)
        {
            if (this.YellowPlayer?.Id == playerId)
                this.YellowPlayer = this.YellowPlayer with { IsReady = isReady };

            if (this.RedPlayer?.Id == playerId)
                this.RedPlayer = this.RedPlayer with { IsReady = isReady };
        }

        private PlayerSide GetPlayerSideByPlayerId(PlayerId playerId)
        {
            return this.YellowPlayer!.Id == playerId ? PlayerSide.Yellow : PlayerSide.Red;
        }

        private class GamePiecePlacementRequest
        {
            public required PlayerId RequestingPlayer { get; init; }
            public required BoardPosition Position { get; init; }
        }
    }

    public enum GameState
    {
        Creating,
        Running,
        Ended
    }





}
