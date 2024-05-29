namespace TheOtherUs.Modules.Randoms;

public interface IRandom<T>
{
    public T GetRandom(T Min, T Max);
}