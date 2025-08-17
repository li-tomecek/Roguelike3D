using UnityEngine;

public enum StatType
{
    HP, ATK, DEF, AGI 
}


public class Effect
{
    private int amount;
    private StatType type;
    public int duration;

    public Effect(int amount, StatType type, int duration)
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
