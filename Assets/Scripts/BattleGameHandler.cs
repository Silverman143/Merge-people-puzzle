using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameHandler : MonoBehaviour
{
    
    private void StartLevel()
    {
        GlobalEvents.GameStartInvoke();
    }

    private void PouseGame()
    {
        GlobalEvents.GamePouseInvoke();
    }

    private void ContinueGame()
    {
        GlobalEvents.GameContinueInvoke();
    }
}
