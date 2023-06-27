public interface IMagic
{
    public ItemType Type { get; }

    public bool Use()
    {  return false; }
}