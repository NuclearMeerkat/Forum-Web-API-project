namespace WebApp.Core.Entities;

public class CompositeKey
{
    public int KeyPart1 { get; set; }
    public int KeyPart2 { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is CompositeKey otherKey)
        {
            return this.KeyPart1 == otherKey.KeyPart1 && this.KeyPart2 == otherKey.KeyPart2;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.KeyPart1, this.KeyPart2);
    }
}
