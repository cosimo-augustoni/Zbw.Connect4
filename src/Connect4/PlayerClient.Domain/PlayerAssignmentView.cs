using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerClient.Contract;

namespace PlayerClient.Domain
{
    public class PlayerAssignmentView
    {
        public required PlayerId PlayerId { get; init; }

        public required GameId GameId { get; init; }
    }
}
