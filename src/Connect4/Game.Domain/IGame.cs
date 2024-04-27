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
        ValueTask<Guid> GetId();
        Task UpdateGameNameAsync(string name);
    }
}
