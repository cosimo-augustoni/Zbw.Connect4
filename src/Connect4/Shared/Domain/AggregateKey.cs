namespace Domain;

public record AggregateKey(Guid Value)
{
    public AggregateKey() : this(Guid.NewGuid())
    {
    }
}