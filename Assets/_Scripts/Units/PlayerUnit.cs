
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUnit : Unit
{
    protected override void Awake()
    {
        turnManager = gameObject.AddComponent<PlayerTurnManager>();
        base.Awake();
    }

    public void Start()
    {
        CombatManager.Instance.OnCombatWin.AddListener(Resurrect);
    }

    private void Resurrect()
    {
        if (_health <= 0)
            _health = 1;
    }
    public PlayerTurnManager GetPlayerTurnManager()
    {
        return (PlayerTurnManager)turnManager;
    }

    public override IEnumerator MoveTo(Vector3 targetPosition, float travelSpeed, float acceptedRadius)
    {
      
        this.GetComponent<PlayerAnimator>().SetMovementSpeed(travelSpeed);

        yield return base.MoveTo(targetPosition, travelSpeed, acceptedRadius);

        this.GetComponent<PlayerAnimator>().SetMovementSpeed(0f);

    }
    public override IEnumerator RotateTo(Vector3 lookVector, float rotationSpeed)
    {
        this.GetComponent<PlayerAnimator>().SetMovementSpeed(rotationSpeed);

        yield return base.RotateTo(lookVector, rotationSpeed);

        this.GetComponent<PlayerAnimator>().SetMovementSpeed(0f);
    }
}
