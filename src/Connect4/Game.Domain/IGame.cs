using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Domain
{
    public interface IGame
    {
        Task<Guid> CreateGame();
        Task UpdateGameNameAsync(string name);
    }
}
