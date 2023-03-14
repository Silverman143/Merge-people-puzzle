using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXpool : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effectPrefab;
    [SerializeField] private List<ParticleSystem> _pool;
    [SerializeField] private int _amount;
    [SerializeField] private bool _autoExpand;

    public void Awake()
    {
        Create();
    }


    private void Create()
    {
        _pool = new List<ParticleSystem>();
        if(_amount == 0)
        {
            _amount = 1;
        }

        for(int i =0; i<_amount; i++)
        {
            ParticleSystem newParticles = CreateParticle();
            _pool.Add(newParticles);
        }
    }

    private ParticleSystem CreateParticle()
    {
        Debug.Log(_effectPrefab);
        ParticleSystem newParticles = Instantiate(_effectPrefab, this.transform);
        newParticles.gameObject.SetActive(false);
        return newParticles;
    }

    private bool HasFreeElement(out GameObject element)
    {
        foreach(ParticleSystem particle in _pool)
        {
            if (!particle.gameObject.activeSelf)
            {
                element = particle.gameObject;
                element.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public GameObject GetFreeElement()
    {
        if(HasFreeElement(out GameObject element))
        {
            return element;
        }

        if (_autoExpand)
        {
            ParticleSystem newParticles = CreateParticle();
            _pool.Add(newParticles);
            newParticles.gameObject.SetActive(true);
            return newParticles.gameObject;
        }

        throw new System.Exception($" There is no free element in particles Pool");
    }
}
