using UnityEngine;

/*
 * This class essentially acts as a way to control player animations more intuitively from other scripts.
 * It removes the need to update string literals for setting parameters of repeating animations accross scripts.
 */

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    
    //ideally this is the only place in which the strings need to be changed
    private int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private int MeleeHash = Animator.StringToHash("MeleeAttack");
    private int DeadHash = Animator.StringToHash("IsDead");
    private int CombatHash = Animator.StringToHash("InCombat");
    private int DamagedHash = Animator.StringToHash("TakeDamage");

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayDamagedAnimation()
    {
        _animator.SetTrigger(DamagedHash);
    }
    public void PlayMeleeAnimation()
    {
        _animator.SetTrigger(MeleeHash);
    }
    public void SetCombatAnimations(bool inCombat)
    {
        _animator.SetBool(CombatHash, inCombat);
    }
    public void SetDeathAnimations(bool isDead)
    {
       _animator.SetBool(DeadHash, isDead);
    }

    public void EndCombatAnimations()
    {
        _animator.SetBool(DeadHash, false);
        _animator.SetBool(CombatHash, false);
    }

    public void SetMovementSpeed(float moveSpeed)
    {
        _animator.SetFloat(MoveSpeedHash, moveSpeed);
    }
    
    
    
}
