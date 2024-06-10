using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Contract;
using Game.Contract.Commands;
using Game.Contract.Events;
using Game.Contract.Queries;
using Game.Contract.Queries.Dtos;
using MediatR;
using PlayerClient.Contract;
using PlayerClient.Domain;

namespace PlayerClient.AI
{
    internal class GamePiecePlacedPolicy(IMediator mediator, IPlayerAssignmentQuery playerAssignmentQuery)
        : INotificationHandler<GamePiecePlacedEventDto>,
            INotificationHandler<GameStartedEventDto>
    {
        private const int activePlayer = 2;
        private const int otherPlayer = 1;
        private const int empty = 0;

        public async Task Handle(GameStartedEventDto notification, CancellationToken cancellationToken)
        {
            var gameId = notification.GameId;
            var players = await playerAssignmentQuery.GetPlayersByGameAsync(gameId);
            var nextPlayerType = players.FirstOrDefault(p => p.PlayerId == notification.StartingPlayerId).PlayerType;

            this.TryMakeNextMove(gameId, PlayerSide.Red, nextPlayerType, players.AreAllAi(), cancellationToken);
        }

        public async Task Handle(GamePiecePlacedEventDto notification, CancellationToken cancellationToken)
        {
            var gameId = notification.GameId;
            var players = await playerAssignmentQuery.GetPlayersByGameAsync(gameId);
            var nextPlayerType = players.FirstOrDefault(p => p.PlayerId != notification.PlacedBy.Id).PlayerType;
            var nextPlayingSide = notification.PlayingSide == PlayerSide.Red ? PlayerSide.Yellow : PlayerSide.Red;

            //TODO Zug erst ausführen wenn Roboter auch bereit ist
            this.TryMakeNextMove(gameId, nextPlayingSide, nextPlayerType, players.AreAllAi(), cancellationToken);
        }

        private void TryMakeNextMove(GameId gameId, PlayerSide nextPlayingSide, PlayerClientType nextPlayerType, bool isVsOtherAi, CancellationToken cancellationToken)
        {
            if (nextPlayerType == AiPlayerClientConstants.EasyPlayerClientType)
                this.MakeNextMove(gameId, nextPlayingSide, DifficultyLevel.Easy, isVsOtherAi, cancellationToken);

            if (nextPlayerType == AiPlayerClientConstants.MediumPlayerClientType)
                this.MakeNextMove(gameId, nextPlayingSide, DifficultyLevel.Medium, isVsOtherAi, cancellationToken);

            if (nextPlayerType == AiPlayerClientConstants.HardPlayerClientType)
                this.MakeNextMove(gameId, nextPlayingSide, DifficultyLevel.Hard, isVsOtherAi, cancellationToken);
        }

        private void MakeNextMove(GameId gameId, PlayerSide nextPlayingSide, DifficultyLevel difficultyLevel, bool isVsOtherAi, CancellationToken cancellationToken)
        {
            _ = Task.Run((Func<Task>)(async () =>
            {
                var game = await mediator.Send(new GameByIdQuery { Id = gameId }, cancellationToken);
                if (!game.IsRunning)
                    return;

                var minimumTime = isVsOtherAi ? 1500 : 500;
                var minmumDelay = Task.Delay(minimumTime, cancellationToken);
                var getNextBoardPositionResult = Task.Run(() => this.GetNextBestBoardPosition(game, nextPlayingSide, (int)difficultyLevel), cancellationToken);
                var completedTask = await Task.WhenAny(getNextBoardPositionResult, minmumDelay);

                if (completedTask == getNextBoardPositionResult)
                {
                    await minmumDelay;
                }

                var boardPosition = await getNextBoardPositionResult;
                await mediator.Send(new PlaceGamePieceCommand(gameId, boardPosition), cancellationToken);

            }), cancellationToken);
        }

        private BoardPosition GetNextBestBoardPosition(GameDto game, PlayerSide nextPlayingSide, int searchDepth)
        {
            var board = game.Board.BoardState.Select(b => b.Select(c =>
                {
                    if (c.SlotState == SlotState.Empty)
                        return empty;
                    if (c.SlotState == nextPlayingSide.ToSlotState())
                        return activePlayer;
                    return otherPlayer;
                }).ToArray()).ToArray()
                .To2DArray();

            var bestX = Connect4Ai.GetBestMove(board, searchDepth);

            var freeY = -1;
            foreach (var gameBoard in game.Board.BoardState)
            {
                if (gameBoard[bestX!.Value].SlotState == SlotState.Empty)
                    freeY++;
                else
                    break;
            }

            return new BoardPosition(bestX!.Value, freeY);
        }
    }

    file static class ArrayExtensions
    {
        public static bool AreAllAi(this List<(PlayerId, PlayerClientType PlayerType)> players)
        {
            return players.All(p => p.PlayerType.Identifier.StartsWith(AiPlayerClientConstants.AiPrefix));
        }

        public static int[,] To2DArray(this int[][] array)
        {
            var result = new int[array.Length, array[0].Length];
            for (var i = 0; i < array.Length; i++)
            {
                for (var j = 0; j < array[i].Length; j++)
                {
                    result[i, j] = array[i][j];
                }
            }

            return result;
        }
    }
}
