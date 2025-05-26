
/// <summary>
/// AnnounceEffect is a class that extends the 
/// <c>Effect</c> class. AnnounceEffect allows 
/// <c>Character</c> objects to announce their effect.
/// </summary>
public class AnnounceEffect : Effect
{
    private string[] _announcement;

    //Constructor
    public AnnounceEffect(string id, string name, EffectOrigin origin, EffectType type, MoveTarget target, double accuracy)
    {
        Id = id;
        Name = name;
        Origin = origin;
        Type = type;
        Target = target;
        Accuracy = accuracy;
    }

    /// <summary>
    /// Announces effect on the <paramref name="target"/>.
    /// </summary>
    /// <param name="target">target of the effect</param>
    /// <returns>an array of strings with result.</returns>
    public override string[] UseEffect(Character target)
    {
        //does nothing...
        return _announcement;
    }
}