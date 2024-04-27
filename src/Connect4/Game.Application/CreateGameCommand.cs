using Game.Domain;
using Shared.Application;

namespace Game.Application;

public record CreateGameCommand : ICommand<Guid>;