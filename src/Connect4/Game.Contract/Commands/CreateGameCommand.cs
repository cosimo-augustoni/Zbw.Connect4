using Shared.Contract;

namespace Game.Contract.Commands;

public record CreateGameCommand : ICommand<Guid>;