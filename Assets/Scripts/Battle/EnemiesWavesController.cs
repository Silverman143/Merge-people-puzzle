using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemiesWavesController : MonoBehaviour
{
    public int _amount;

    [SerializeField] private float _speed;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _instantiatePos;
    [SerializeField] private Transform[] _moveLinesPositions;
    [SerializeField] private Transform _moveLinesParrent;

    private BattleLevelData _currentLevelData;
    private List<EnemyData> _enemiesData;

    private void Awake()
    {
        GetMoveLines();
        //_currentLevelData = DataHandler.Instance.CurrentBattleLevelData;
        //_enemiesData = _currentLevelData.EnemiesDatas.ToList<EnemyData>();
    }

    private void Start()
    {
        _currentLevelData = DataHandler.Instance.CurrentBattleLevelData;
        _enemiesData = _currentLevelData.EnemiesDatas.ToList<EnemyData>();

        StartCoroutine(EnemiesInstantiation());
    }

    private void OnEnable()
    {
        GlobalEvents.OnGameStart.AddListener(StartWaves);
        GlobalEvents.OnGamePaused.AddListener(StopWaves);
        GlobalEvents.OnGameContinue.AddListener(StartWaves);
    }

    private void OnDisable()
    {
        GlobalEvents.OnGameStart.RemoveListener(StartWaves);
        GlobalEvents.OnGamePaused.RemoveListener(StopWaves);
        GlobalEvents.OnGameContinue.RemoveListener(StartWaves);
    }

    private void StartWaves()
    {
        StartCoroutine(EnemiesInstantiation());
    }

    private void StopWaves()
    {
        StopCoroutine(EnemiesInstantiation());
    }

    private void GetMoveLines()
    {
        _moveLinesPositions = _moveLinesParrent.GetComponentsInChildren<Transform>();
    }

    private void CreateEnemy(EnemyType type, float health)
    {
        GameObject newEnemy = Instantiate(_enemyPrefab, _instantiatePos.position, Quaternion.identity);
        EnemyMono enemy = newEnemy.GetComponent<EnemyMono>();
        enemy.Activate(_moveLinesPositions[Random.Range(0, _moveLinesPositions.Length)], health, type);
    }

    private bool GetEnemy(out EnemyData data)
    {
        int value = Random.Range(0, _enemiesData.Count);
        data = _enemiesData[value];

        if (_enemiesData.Count > 0)
        {
            
            data.Amount--;

            if (data.Amount == 0)
            {
                _enemiesData.Remove(_enemiesData[value]);
            }
            else
            {
                _enemiesData[value] = data;
            }

            return true;
        }
        else return false;
    }


    IEnumerator EnemiesInstantiation()
    {

        while(GetEnemy(out EnemyData data))
        {
            CreateEnemy(data.Type, data.Health);
            yield return new WaitForSeconds(_speed);
        }
    }
}
