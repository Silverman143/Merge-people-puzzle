using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeGameHandler : MonoBehaviour
{


    private void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        MergeLevelGenerator.Instance.LoadLevel();
    }

}
