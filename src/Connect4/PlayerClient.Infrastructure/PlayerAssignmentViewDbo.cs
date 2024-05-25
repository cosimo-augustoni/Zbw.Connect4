using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlayerClient.Infrastructure
{
    internal class PlayerAssignmentViewDbo
    {
        public ObjectId Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid GameId { get; set; }
    }
}
