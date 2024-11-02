using System;

[Serializable]
public class CustomEffect
{
    public string Name;
    public EffectType EffectType;
    public float Chance;

    public float DelayTime;

    public EventType eventType;
    public ImpactType ImpactType;
    public ImpactParameter Impact;
    public ImpactBuff ImpactBuff;


    public CustomEffect()
    {
        Name = "New Custom Effect";
        Impact = new ImpactParameter();
    }
}
