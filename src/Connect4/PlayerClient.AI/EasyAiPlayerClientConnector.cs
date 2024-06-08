using Game.Contract;
using Game.Contract.Commands;
using MediatR;
using PlayerClient.Contract;

namespace PlayerClient.AI
{
    internal class EasyAiPlayerClientConnector(ISender mediator) : AiPlayerClientConnector(mediator)
    {
        protected override PlayerClientType PlayerClientType { get; } = AiPlayerClientConstants.EasyPlayerClientType;
        public override string DisplayName => "KI (Einfach)";
    }

    internal class MediumAiPlayerClientConnector(ISender mediator) : AiPlayerClientConnector(mediator)
    {
        protected override PlayerClientType PlayerClientType { get; } = AiPlayerClientConstants.MediumPlayerClientType;
        public override string DisplayName => "KI (Mittel)";
    }

    internal class HardAiPlayerClientConnector(ISender mediator) : AiPlayerClientConnector(mediator)
    {
        protected override PlayerClientType PlayerClientType { get; } = AiPlayerClientConstants.HardPlayerClientType;
        public override string DisplayName => "KI (Schwierig)";
    }

    internal abstract class AiPlayerClientConnector(ISender mediator) : IPlayerClientConnector
    {
        protected abstract PlayerClientType PlayerClientType { get; }
        public abstract string DisplayName { get; }
        public async Task JoinGame(GameId gameId, User user, PlayerSide playerSide)
        {
            var playerId = new PlayerId();
            var player = new Player(playerId, new PlayerOwner(this.PlayerClientType.Identifier, this.DisplayName), this.PlayerClientType.Identifier);
            await mediator.Send(new AddPlayerCommand(gameId, player, playerSide));

            await mediator.Send(new ReadyPlayerCommand(gameId, playerId));
        }
    }
}
