using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnManager : MonoBehaviour
{
    protected Unit unit;
    protected Skill _activeSkill;

    protected List<Unit> _targetPool;
    protected int _targetIndex;

    protected const float TRAVEL_TIME = 7f;
    protected const float ATTACKER_RADIUS = 2f;     // how far the unit can be away from the target when melee attacking
    protected const float RETURN_RADIUS = 0.02f;     // how far the unit can be away from its original positions when returning

    public virtual void StartTurn()
    {
        Debug.Log($"{unit.name}'s turn starting:");

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
        Vector3 originalPosition = unit.transform.position;
        
        //1. Face target
        if (skill.GetTargetMode() == TargetMode.MELEE || skill.GetTargetMode() == TargetMode.RANGED)
            unit.gameObject.transform.LookAt(target.transform, Vector3.up);

        //2. Move to Target (if applicable) ~ and play relevant animations ~  move back to position
        switch (skill.GetTargetMode())
        {
            case TargetMode.MELEE:
                
                yield return unit.MoveTo(target.transform.position, TRAVEL_TIME, ATTACKER_RADIUS);
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForMeleeAnimation();
                else
                    yield return new WaitForSeconds(0.5f);
                
                skill.UseSkill(unit, target);

                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForCurrentAnimation();
                yield return unit.MoveTo(originalPosition, TRAVEL_TIME, RETURN_RADIUS);

                break;

            case TargetMode.ALL_ENEMIES:
                
                yield return unit.MoveTo(originalPosition + Vector3.forward, TRAVEL_TIME, RETURN_RADIUS);
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForMeleeAnimation();
                else
                    yield return new WaitForSeconds(0.5f);

                for (int i = 0; i < _targetPool.Count; i++)
                {
                    skill.UseSkill(unit, _targetPool[i]);
                }

                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForCurrentAnimation();
                yield return unit.MoveTo(originalPosition, TRAVEL_TIME, RETURN_RADIUS);

                break;

            case TargetMode.ALL_ALLIES:
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForMagicAnimation();

                for (int i = 0; i < _targetPool.Count; i++)
                {
                    skill.UseSkill(unit, _targetPool[i]);
                }
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForCurrentAnimation();

                break;

            default:
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForMagicAnimation();

                skill.UseSkill(unit, target);
                
                if (unit.gameObject.GetComponent<PlayerAnimator>())
                    yield return unit.gameObject.GetComponent<PlayerAnimator>().WaitForCurrentAnimation();

                break;
        }

        //3. Reset Rotation
        unit.gameObject.transform.rotation = originalRotation;

        EndTurn();

        yield return null;
    }

}
