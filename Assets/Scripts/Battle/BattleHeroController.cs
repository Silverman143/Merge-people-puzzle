using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BattleHeroController : MonoBehaviour
{
    [SerializeField] private HeroPrefab[] _herosPrefabs;
    [SerializeField] private float _speed;
    [SerializeField] private float _health;
    [SerializeField] private CharacterController _characterController;

    private bool _isActive = false;
    private Vector3 _moveVector;
    private Animator _animator;

    private bool _gamePaused = false;

    private void Start()
    {
        _characterController = GetComponentInChildren<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void SetGamePause() => _gamePaused = true;
    private void ResetGamePause() => _gamePaused = false;

    private void OnEnable()
    {
        GlobalEvents.OnGamePaused.AddListener(SetGamePause);
        GlobalEvents.OnGameContinue.AddListener(ResetGamePause);
    }

    private void OnDisable()
    {
        GlobalEvents.OnGamePaused.RemoveListener(SetGamePause);
        GlobalEvents.OnGameContinue.RemoveListener(ResetGamePause);
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


    public void FixedUpdate()
    {
        if (_gamePaused) return;

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

    private void GetDamage(float value)
    {
        _health -= value;

        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.collider.gameObject.TryGetComponent<EnemyMono>(out EnemyMono enemy))
        {
            VFXHandler.Instance.DamageEffectActivate(hit.point);
            float enemyHealth = enemy.Health;
            enemy.GetDamage(_health);
            GetDamage(enemyHealth);
        }
    }
}
