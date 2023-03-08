using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public HeroType Type;
    [SerializeField] private Outline _outline;

    private void Awake()
    {
        _outline = GetComponentInChildren<Outline>();
    }

    public void OutLine(bool value)
    {
        _outline.enabled = value;
    }
}
