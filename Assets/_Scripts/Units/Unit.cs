using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int attack, defense, agility;
}

[RequireComponent(typeof(CharacterController))]
public abstract class Unit : MonoBehaviour
{
    [Header("Combat")]
    //Stats
    [SerializeField] protected Stats stats;
    [SerializeField] protected Stats modifiers;
    protected int _health;
    protected int _bp;

    //Skills
    public static int MAX_SKILL_COUNT = 2;
    [SerializeField] protected List<Skill> skills;
    [SerializeField] protected Skill defaultSkill;

    //Effects
    private List<Effect> _activeEffects = new List<Effect>();

    protected TurnManager turnManager;
    protected HealthBar healthBar;
    protected CharacterController controller;
    
    //---------------------------------------------------
    //---------------------------------------------------

    protected virtual void Awake()
    {
        _health = stats.maxHealth;
        healthBar = gameObject.GetComponentInChildren<HealthBar>();
        healthBar.gameObject.SetActive(false);  //hide health bar until combat

        controller = gameObject.GetComponent<CharacterController>();
    }


    // --- Combat Methods ---
    // ----------------------
    public void UseDefaultSkill(Unit target)
    {
        defaultSkill.UseSkill(this, target);
    }
    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = _health < 0 ? 0 : _health;
       
        //ToDo: make healthbar and animator subscribe to take damage event. Players can have a HUD including a health bar instead of just a slider
        healthBar.SetSliderPercent((float)_health / stats.maxHealth);

        if (damage > 0 && this.TryGetComponent<Animator>(out Animator animator))
            animator.SetTrigger("Take Damage");

        if (_health <= 0)
        {
            CombatManager.Instance.RemoveFromCombat(this);
            Debug.Log($"{name} is Dead.");
           
        }
    }
    public void ApplyModifier(EffectType type, int value)
    {
        switch (type)
        {
            case EffectType.ATK:
                modifiers.attack += value;
                break;
            case EffectType.DEF:
                modifiers.defense += value;
                break;
            case EffectType.AGI:
                modifiers.agility += value;
                break;

        }
    }
    public void ResolveEffects()
    {
        for(int i = 0; i < _activeEffects.Count; i++)
        {
            _activeEffects[i].duration--;
        }
    }

    // --- Move and Rotate Methods ---
    // -------------------------------
    #region
    public IEnumerator MoveToAndLook(Vector3 targetPosition, float travelSpeed, float acceptedRadius, Vector3 lookTarget, float rotationSpeed)
    {
        yield return MoveTo(targetPosition, travelSpeed, acceptedRadius);
        
        yield return RotateTo((lookTarget - transform.position).normalized, rotationSpeed);
    }
    public virtual IEnumerator MoveTo(Vector3 targetPosition, float travelSpeed, float acceptedRadius)
    {
        Vector3 direction;
        bool atDestination = false;

        targetPosition.y = transform.position.y;
        
        while (!atDestination)
        {
            direction = targetPosition - this.transform.position;
            atDestination = (direction.magnitude <= acceptedRadius);
                       
            if (!atDestination)
            {
                if ((direction.normalized * travelSpeed * Time.deltaTime).magnitude < direction.magnitude)      // so we dont overshoot and end up in a loop
                    direction = (direction.normalized * travelSpeed * Time.deltaTime);

                controller.Move(direction);
            }

            this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            yield return null;
        }
    }
    public virtual IEnumerator RotateTo(Vector3 lookVector, float rotationSpeed)
    {
        //lookVector.y = transform.forward.y;

        Quaternion start = Quaternion.LookRotation(transform.forward, Vector3.up);
        Quaternion end = Quaternion.LookRotation(lookVector, Vector3.up);

        float timer = 0f;
        float rotateTime = Vector3.Angle(transform.forward, lookVector) / rotationSpeed;

        while ((timer / rotateTime) < 1f)
        {

            transform.rotation = Quaternion.Slerp(start, end, (timer / rotateTime));
            timer += Time.deltaTime;
            yield return null;
        }
    }
    #endregion


    // --- Getters / Setters ---
    // -------------------------
    #region
    public Stats GetStats() { return stats; }
    public Stats GetModifiers() { return modifiers; }
    public List<Effect> GetActiveEffects() { return _activeEffects; }
    public virtual TurnManager GetTurnManager() { return turnManager; }
    public Skill GetDefaultSkill() { return defaultSkill; }
    public List<Skill> GetSkills() { return skills; }
    public HealthBar GetHealthBar() { return healthBar; }
    public int GetHealth() { return _health; }
    public void SetHealth(int value) { _health = value; }
    public int GetBP() { return _bp; }
    public void IncrementBP() { _bp++; }
    public void DecrementBP(int amt) { _bp = Mathf.Max(_bp -amt, 0); }
    public void ReplaceSkill(Skill oldSk, Skill newSk)
    {
        int index = skills.FindIndex(s => s == oldSk);
        if (index != -1)
            skills[index] = newSk;
    }
    public void TryAddSkill(Skill skill)
    {
        if (skills.Count < MAX_SKILL_COUNT)
        {
            //if there is an available space
            skills.Add(skill);
        }
        else
        {
            Debug.LogError($"The skills list is already full");
        }
    }
    #endregion
}
