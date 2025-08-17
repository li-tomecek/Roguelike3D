using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int attack, defense, agility;
    public int level;
}

[RequireComponent(typeof(CharacterController))]
public abstract class Unit : MonoBehaviour
{
    #region
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
    protected CharacterController controller;

    public bool isGuarding { get; private set; }
    public const float GUARD_DAMAGE_REDUCTION = 0.5f;

    //Events
    public UnityEvent OnDamageTaken = new UnityEvent();                     //to update when the unit takes damage. used for animations
    public UnityEvent<float> OnHealthChanged = new UnityEvent<float>();     //to update when the unit's health value has changed
    public UnityEvent<int> OnBPChanged = new UnityEvent<int>();             //to update when the unit uses or gains BP
    public UnityEvent OnDeath = new UnityEvent();                           //to update when the unit faints/dies in battle
    #endregion
    //---------------------------------------------------
    //---------------------------------------------------

    protected virtual void Awake()
    {
        _health = stats.maxHealth;
        controller = gameObject.GetComponent<CharacterController>();
    }


    // --- Combat Methods ---
    // ----------------------
    public void HealDamage(int damage)
    {
        _health += damage;
        _health = Mathf.Min(stats.maxHealth, _health);

        OnHealthChanged.Invoke((float)_health / stats.maxHealth);
    }
    public void TakeDamage(int damage)
    {

        if (isGuarding)
        {
            damage = Mathf.CeilToInt(Unit.GUARD_DAMAGE_REDUCTION * (float)damage); //halve damage
           isGuarding = false;
        }

        _health -= damage;
        _health = _health < 0 ? 0 : _health;

        CombatInterface.Instance.SetIndicator(damage.ToString(), gameObject.transform);
        Debug.Log($"{name} took {damage} damage!");


        if (damage > 0)
        {
            OnDamageTaken.Invoke();
        }

        if (_health <= 0)
        {
            CombatManager.Instance.RemoveFromCombat(this);
            Debug.Log($"{name} is Dead.");
            OnDeath.Invoke();

        }
        
        OnHealthChanged.Invoke((float) _health / stats.maxHealth);
    }
    public void ApplyModifier(StatType type, int value)
    {
        switch (type)
        {
            case StatType.ATK:
                modifiers.attack += value;
                break;
            case StatType.DEF:
                modifiers.defense += value;
                break;
            case StatType.AGI:
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
        InputController.Instance.DisableActiveMap();
        Vector3 direction;
        bool atDestination = false;
        
        while (!atDestination)
        {
            direction = targetPosition - gameObject.transform.position;
            direction.y = 0f;               //so we do not adjust the height of the unit while moving
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
        InputController.Instance.EnableActiveMap();
    }
    public virtual IEnumerator RotateTo(Vector3 lookVector, float rotationSpeed)
    {
        InputController.Instance.DisableActiveMap();
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
        InputController.Instance.EnableActiveMap();
    }
    #endregion


    // --- Getters / Setters ---
    // -------------------------
    #region
    public Stats GetStats() { return stats; }
    public void SetStats(Stats s) {stats = s; }
    public Stats GetModifiers() { return modifiers; }
    public List<Effect> GetActiveEffects() { return _activeEffects; }
    public virtual TurnManager GetTurnManager() { return turnManager; }
    public Skill GetDefaultSkill() { return defaultSkill; }
    public List<Skill> GetSkills() { return skills; }
    public void SetSkills(List<Skill> s) {skills = s; }
    public int GetHealth() { return _health; }
    public void SetHealth(int value) { 
        _health = value;
        OnHealthChanged.Invoke((float)_health / stats.maxHealth);
    }
    public int GetBP() { return _bp; }
    public void IncrementBP() { 
        _bp++; 
        OnBPChanged.Invoke(_bp); 
    }
    public void SetBP(int amt) {
        _bp = Mathf.Max(amt, 0);
        OnBPChanged.Invoke(_bp); 
    }
    public void DecrementBP(int amt) { 
        _bp = Mathf.Max(_bp -amt, 0); 
        OnBPChanged.Invoke(_bp); 
    }
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
    public void SetGuard(bool guarding) { isGuarding = guarding; }
    #endregion
}
