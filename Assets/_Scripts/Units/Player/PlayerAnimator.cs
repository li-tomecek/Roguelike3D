using System.Collections;
using UnityEngine;

/*
 * This class essentially acts as a way to control player animations more intuitively from other scripts.
 * It removes the need to update string literals for setting parameters of repeating animations across scripts.
 */

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Unit))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    // Animation Hashes ~ ideally this is the only place in which the strings need to be changed
    private int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    
    private int CombatStartHash = Animator.StringToHash("StartCombat");
    private int MeleeHash = Animator.StringToHash("MeleeAttack");
    private int MagicHash = Animator.StringToHash("MagicAttack");
    private int DamagedHash = Animator.StringToHash("TakeDamage");
    
    private int DeadHash = Animator.StringToHash("IsDead");
    private int CombatHash = Animator.StringToHash("InCombat");

    void Start()
    {
        _animator = GetComponent<Animator>();
        gameObject.GetComponent<Unit>().OnDamageTaken.AddListener(PlayDamagedAnimation); 
        CombatManager.Instance.OnCombatWin.AddListener(EndCombatAnimations);
    }
    // -- Wait for  triggered animations -- 
    public IEnumerator WaitForCurrentAnimation()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1);  //wait for transition
        yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);  //wait for animation
    }
    public IEnumerator WaitForMeleeAnimation()
    {
        PlayMeleeAnimation();
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1);  //wait for transition
        yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);  //wait for animation
    }
    public IEnumerator WaitForMagicAnimation()
    {
        PlayMagicAnimation();
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1);  //wait for transition
        yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);  //wait for animation
    }

    // -- Trigger Animations -- 
    public void StartCombatAnimations()
    {
        _animator.SetTrigger(CombatStartHash);
    }   
    public void PlayDamagedAnimation()
    {
        _animator.SetTrigger(DamagedHash);
    }
    public void PlayMeleeAnimation()
    {
        _animator.SetTrigger(MeleeHash);
    }
    public void PlayMagicAnimation()
    {
        _animator.SetTrigger(MagicHash);
    }

    // -- Adjustable Parameters -- 
    public void SetCombatAnimations(bool inCombat)
    {
        _animator.SetBool(CombatHash, inCombat);
    }
    public void SetDeathAnimations(bool isDead)
    {
       _animator.SetBool(DeadHash, isDead);
    }
    public void SetMovementSpeed(float moveSpeed)
    {
        _animator.SetFloat(MoveSpeedHash, moveSpeed);
    }

    // -- Multiple Parameters
    public void EndCombatAnimations()
    {
        _animator.SetBool(DeadHash, false);
        _animator.SetBool(CombatHash, false);
    }

    
    
    
}
