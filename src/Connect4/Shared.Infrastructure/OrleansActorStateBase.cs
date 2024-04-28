using System.Diagnostics.CodeAnalysis;

namespace Shared.Infrastructure
{
    public abstract class OrleansActorStateBase<TAggregate>
        where TAggregate : class
    {
        [MemberNotNullWhen(true, nameof(Aggregate))]
        public bool IsInitialised { get; set; }
        
        public TAggregate? Aggregate { get; set; }
    }
}