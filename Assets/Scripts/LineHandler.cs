using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LineHandler : MonoBehaviour
{
    public Vector3 pos0;
    public Vector3 pos1;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetPositions0(Vector3 pos)
    {
        pos0 = pos;

        _lineRenderer.SetPosition(0, pos);
    }

    public void SetPositions1(Vector3 pos)
    {
        _lineRenderer.SetPosition(1, pos);
    }

}
