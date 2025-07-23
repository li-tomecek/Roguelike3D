using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _party;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_party.gameObject);   //Party information should persist between scenes
    }


}
