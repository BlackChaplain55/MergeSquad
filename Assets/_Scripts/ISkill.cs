public interface ISkill
{
    public ItemType Type { get; }

    public bool Use()
    {  return false; }
}