using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public enum EnemyType
{
    A, B, C
}

[System.Serializable]
public struct EnemyPrefab
{
    public GameObject Obj;
    public EnemyType Type;
}

[System.Serializable]
public enum EnemyStatus
{
    MoveToPoint, MoveToPlayer
}

public class EnemyMono : MonoBehaviour
{
    [SerializeField] private float _health;
    private EnemyType _type;
    public float Health => _health;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private EnemyPrefab[] _enemiesPrefabs;
    private EnemyStatus _status;
    private Transform _lineStartPos;
    private Vector3 _moveVector;
    private bool _isActive = false;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private bool _gamePaused = false;

    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _moveSpeed = _navMeshAgent.speed;
        _animator = GetComponentInChildren<Animator>();

        GlobalEvents.OnGamePaused.AddListener(SetGamePause);
        GlobalEvents.OnGameContinue.AddListener(ResetGamePause);
    }

    private void OnDisable()
    {
        GlobalEvents.OnGamePaused.RemoveListener(SetGamePause);
        GlobalEvents.OnGameContinue.RemoveListener(ResetGamePause);
    }

    private void SetGamePause() => _gamePaused = true;
    private void ResetGamePause() => _gamePaused = false;

    public void GetDamage(float amount)
    {
        _health -= amount;
        if(_health <= 0)
        {
            GlobalEvents.EnemyDeadInvoke();
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (_gamePaused) return;

        switch (_status)
        {
            case EnemyStatus.MoveToPoint:
                if (_isActive && !_navMeshAgent.hasPath)
                {
                    _isActive = false;
                    _navMeshAgent.enabled = false;
                    _status = EnemyStatus.MoveToPlayer;
                }
                break;

            case EnemyStatus.MoveToPlayer:
                transform.position += _moveVector * _moveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(_moveVector);
                break;
        }

        
    }

    public void Activate(Transform linePos, float health, EnemyType type)
    {
        _health = health;
        _type = type;


        foreach(EnemyPrefab pref in _enemiesPrefabs)
        {
            if(pref.Type == _type)
            {
                pref.Obj.SetActive(true);
            }
        }
        _lineStartPos = linePos;
        NavMeshPath path = new NavMeshPath();
        _navMeshAgent.CalculatePath(linePos.position, path);
        _navMeshAgent.SetPath(path);
        _navMeshAgent.Move(Vector3.zero);
        _isActive = true;
        _moveVector = linePos.forward*-1;
    }
}
