namespace EggCore;

public abstract class EggAction(string id)
{
    public readonly string Id = id;

    public abstract void Execute();
}