using Game.Contract.Queries;
using Game.Contract.Queries.Dtos;
using Game.Domain.GameProjections;
using Shared.Application;

namespace Game.Application.Queries
{
    internal class GameByIdQueryHandler(IGameQuery gameQuery) : IQueryHandler<GameByIdQuery, GameDto>
    {
        public async Task<GameDto> Handle(GameByIdQuery request, CancellationToken cancellationToken)
        {
            var game = await gameQuery.GetByIdAsync(request.Id, cancellationToken);

            return new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                YellowPlayer = game.YellowPlayer != null ? new PlayerDto()
                {
                    Id = game.YellowPlayer.Id,
                    Owner = new PlayerOwnerDto
                    {
                        Identifier = game.YellowPlayer.Owner.Identifier,
                        DisplayName = game.YellowPlayer.Owner.DisplayName,
                    },
                    IsReady = game.YellowPlayer.IsReady,
                    Type = game.YellowPlayer.Type
                } : null,

                RedPlayer = game.RedPlayer != null ? new PlayerDto()
                {
                    Id = game.RedPlayer.Id,
                    Owner = new PlayerOwnerDto
                    {
                        Identifier = game.RedPlayer.Owner.Identifier,
                        DisplayName = game.RedPlayer.Owner.DisplayName,
                    },
                    IsReady = game.RedPlayer.IsReady,
                    Type = game.RedPlayer.Type
                } : null,
                CurrentPlayerId = game.CurrentPlayerId,
                WinningPlayerId = game.WinningPlayerId,
                FinishReason = game.FinishReason,
                IsFinished = game.IsFinished,
                IsAborted = game.IsAborted,
                IsRunning = game.IsRunning,
                Board = new BoardDto
                {
                    BoardState = game.Board.BoardState.Select(b => b.Select(r => new SlotDto(r.SlotState)).ToArray()).ToArray()
                }
            };
        }
    }
}