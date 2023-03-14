using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHeroController : MonoBehaviour
{
    [SerializeField] private HeroPrefab[] _herosPrefabs;
    [SerializeField] private float _speed;
    [SerializeField] private CharacterController _characterController;

    private bool _isActive = false;
    private Vector3 _moveVector;
    private Animator _animator;

    private void Start()
    {
        _characterController = GetComponentInChildren<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void Activate(HeroType type)
    {
        foreach (HeroPrefab hero in _herosPrefabs)
        {
            if (hero.Type == type)
            {
                hero.Obj.SetActive(true);
            }
        }

        _animator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        //GlobalEvents.OnInputPositionEnded.RemoveListener(Activate);
        transform.parent = null;
        _moveVector = Vector3.forward;
        _isActive = true;
        _animator.SetBool("isWalking", true);
    }

    public void OnEnable()
    {
        //GlobalEvents.OnInputPositionEnded.AddListener(Activate);
        
    }

    public void OnDisable()
    {
        GlobalEvents.OnInputPositionEnded.RemoveListener(Attack);
    }

    public void FixedUpdate()
    {
        if (_isActive)
        {
            Move();
        }
    }

    private void Move()
    {
        //transform.position+= _moveVector*Time.deltaTime*_speed;

        _characterController.Move(_moveVector * Time.deltaTime * _speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<EnemyMono>(out EnemyMono enemy))
        {
            Destroy(this.gameObject);
        }
    }
}
