using UnityEngine;

public enum EffectType
{
    ATK, DEF, AGI       //Note: 'Effect' and not "StatModifier' in case I want to add effects like paralysis, stuns, etc.
}


public class Effect
{
    private int amount;
    private EffectType type;
    public int duration;

    public Effect(int amount, EffectType type, int duration)
    {
        this.amount = amount;
        this.type = type;
        this.duration = duration;
    }

    public void ApplyEffect(Unit unit)
    {
        unit.ApplyModifier(type, amount);

        //if there are other types of non-stat effects application would be defined here
    }

    public void RemoveEffect(Unit unit)
    {
        unit.ApplyModifier(type, -amount);

        //if there are other types of non-stat effects removal would be defined here
    }



}
