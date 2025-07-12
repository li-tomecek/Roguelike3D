using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnManager : MonoBehaviour
{
    protected Unit unit;
    protected Skill _activeSkill;

    protected List<Unit> _targetPool;
    protected int _targetIndex;

    public virtual void StartTurn()
    {
        //Resolve any active effects
        Effect effect;
        for (int i = 0; i < unit.GetActiveEffects().Count; i++)
        {
            effect = unit.GetActiveEffects()[i];
            effect.duration--;
            if (effect.duration < 0)    //0 means that is is still up for one turn
            {
                effect.RemoveEffect(unit);
                unit.GetActiveEffects().Remove(effect);
            }
        }

        unit.IncrementBP();
    }

    public virtual void EndTurn()
    {
        CombatManager.Instance.NextTurn();
    }

    protected void SetupTargetPool(bool isPlayer)
    {
        switch (_activeSkill.GetTargetMode())
        {
            case TargetMode.RANGED:
                _targetPool = isPlayer ? CombatManager.Instance.GetEnemyUnits() : CombatManager.Instance.GetPlayerUnits();
                break;
            case TargetMode.MELEE:
                _targetPool = isPlayer ? CombatManager.Instance.GetEnemyUnits() : CombatManager.Instance.GetPlayerUnits();
                break;
            case TargetMode.ALL_ENEMIES:
                _targetPool = isPlayer ? CombatManager.Instance.GetEnemyUnits() : CombatManager.Instance.GetPlayerUnits();
                break;
            case TargetMode.ALLY:
                _targetPool = isPlayer ? CombatManager.Instance.GetPlayerUnits() : CombatManager.Instance.GetEnemyUnits();
                break;
            case TargetMode.ALL_ALLIES:
                _targetPool = isPlayer ? CombatManager.Instance.GetPlayerUnits() : CombatManager.Instance.GetEnemyUnits();
                break;

                //ToDo: Setup proper targeting for 'RANGED', and 'MELEE'
        }
    }

    protected IEnumerator PlayTurnSequence(Skill skill, Unit target)
    {
        Quaternion originalRotation = unit.transform.rotation;

        //1. Face target
        if (skill.GetTargetMode() == TargetMode.MELEE || skill.GetTargetMode() == TargetMode.RANGED)
            unit.gameObject.transform.LookAt(target.transform, Vector3.up);

        //2. Move to Target (if applicable) and play relevant animation
        switch (skill.GetTargetMode())
        {
            case TargetMode.MELEE:
                //Run towards target here
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    unit.gameObject.GetComponent<PlayerAnimator>().PlayMeleeAnimation();

                skill.UseSkill(unit, target);
                break;

            case TargetMode.ALL_ENEMIES:
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    unit.gameObject.GetComponent<PlayerAnimator>().PlayMeleeAnimation();

                for (int i = 0; i < _targetPool.Count; i++)
                {
                    skill.UseSkill(unit, _targetPool[i]);
                }
                break;

            case TargetMode.ALL_ALLIES:
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    unit.gameObject.GetComponent<PlayerAnimator>().PlayMagicAnimation();
                
                for (int i = 0; i < _targetPool.Count; i++)
                {
                    skill.UseSkill(unit, _targetPool[i]);
                }
                break;

            default:
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    unit.gameObject.GetComponent<PlayerAnimator>().PlayMagicAnimation();

                skill.UseSkill(unit, target);
                break;
        }

        //3. Reset Rotation
        unit.gameObject.transform.rotation = originalRotation;

        EndTurn();

        yield return null;
    }

}
