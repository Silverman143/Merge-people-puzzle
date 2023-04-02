using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents 
{
    public static UnityEvent<MergeHeroController> OnHeroSelected = new UnityEvent<MergeHeroController>();
    public static UnityEvent<Vector3,HeroType> OnHeroDestroy = new UnityEvent<Vector3, HeroType>();
    public static UnityEvent<int, HeroType> OnChangedHeroesAmount = new UnityEvent<int, HeroType>();
    public static UnityEvent OnChangeCoinsAmount = new UnityEvent();
    public static UnityEvent OnFillInButton = new UnityEvent();
    public static UnityEvent<Vector3> OnInputPositionChanged = new UnityEvent<Vector3>();
    public static UnityEvent OnInputPositionEnded = new UnityEvent();

    // Battle

    public static UnityEvent<Vector3> OnHeroHitEnemy = new UnityEvent<Vector3>();
    public static UnityEvent OnEnemyDead = new UnityEvent();
    public static UnityEvent OnGameStart = new UnityEvent();
    public static UnityEvent OnGamePaused = new UnityEvent();
    public static UnityEvent OnGameContinue = new UnityEvent();

    public static void HeroSelectedInvoke(MergeHeroController hero) => OnHeroSelected?.Invoke(hero);

    public static void HeroDestroyInvoke(Vector3 pos, HeroType type) => OnHeroDestroy.Invoke(pos, type);

    public static void ChangedHeroesAmountInvoke(int amount, HeroType type) => OnChangedHeroesAmount.Invoke(amount, type);

    public static void ChangeCoinsAmountInvoke() => OnChangeCoinsAmount.Invoke();

    public static void FillInButtonInvoke() => OnFillInButton.Invoke();

    public static void InputPositionChangedInvoke(Vector3 pos) => OnInputPositionChanged.Invoke(pos);

    public static void InputPositionEndedInvoke() => OnInputPositionEnded.Invoke();

    // Battle

    public static void HeroHitEnemyInvoke(Vector3 pos) => OnHeroHitEnemy.Invoke(pos);

    public static void EnemyDeadInvoke() => OnEnemyDead.Invoke();

    public static void GameStartInvoke() => OnGameStart.Invoke();

    public static void GamePouseInvoke() => OnGamePaused.Invoke();

    public static void GameContinueInvoke() => OnGameContinue.Invoke();
}
