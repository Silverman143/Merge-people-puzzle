using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour
{
    public static VFXHandler Instance;

    public VFXpool DamageEffect;

    public void Awake()
    {
        if(Instance != this)
        {
            Instance = this;
        }
    }

    public void DamageEffectActivate(Vector3 pos)
    {
        GameObject effect = DamageEffect.GetFreeElement();
        effect.transform.position = pos;
    }
}
