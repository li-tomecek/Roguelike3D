using System.Collections;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] Follower _follower;
    [SerializeField] float _followDelay;
   

    private float _stoppedTime;         //how long the character has not been moving
    private const float STOP_TIMER = 0.2f;

    Transform _followPoint;
    Vector3 _followRotation;
    private Vector3 _lastPosition;
    private bool _isFollowing;

    private void Start()
    {
        if (_follower)
            _follower.SetFollowPoint(transform.Find("FollowerPoint"));
    }
    void Update()
    {
        //check to see if we should stop following
        if (_isFollowing && _lastPosition == transform.position)          //character has stopped moving
        {
            _stoppedTime += Time.deltaTime;
            
            if (_stoppedTime >= STOP_TIMER)
            {
                _isFollowing = false;
                _stoppedTime = 0;
            }
        }
           
        _lastPosition = transform.position;

        //follow the unit in front
        if (_isFollowing)
        {
            //face the unit in front
            _followRotation.x = (_followPoint.position.x - transform.position.x);
            _followRotation.z = (_followPoint.position.z - transform.position.z);
            
            _followPoint.position = new Vector3(_followPoint.position.x, transform.position.y, _followPoint.position.z); //temporary, to avoid having the follower jump.

            if(_followRotation != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_followRotation);

            transform.position = Vector3.MoveTowards(transform.position, _followPoint.position, 0.03f);
        }
    }

    public IEnumerator StartFollowing()
    {
        yield return new WaitForSeconds(_followDelay);
        _isFollowing = true;


        if (_follower)
            _follower.StartCoroutine(_follower.StartFollowing());
    }

    public void SetFollowPoint(Transform point)
    {
        _followPoint = point;
    }
}
