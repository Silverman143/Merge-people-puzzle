using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    a, b, c
}

public struct Selector
{
    public HeroController SelectedHero0;
    public HeroController SelectedHero1;
    public bool isReady=> SelectedHero0&&SelectedHero1 ;
}

public class GameController : MonoBehaviour
{
    private Selector _selector;

    private void Awake()
    {
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began | touch.phase == TouchPhase.Moved | touch.phase == TouchPhase.Stationary)
            {
                Vector2 touchPos = Input.GetTouch(0).position;
                // The ray to the touched object in the world
                Ray ray = Camera.main.ScreenPointToRay(touchPos);

                // Your raycast handling
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    if (hit.transform.TryGetComponent<HeroController>(out HeroController hero))
                    {
                        if (_selector.SelectedHero0 == null)
                        {
                            _selector.SelectedHero0 = hero;
                            _selector.SelectedHero0.OutLine(true);
                        }
                        else
                        {
                            _selector.SelectedHero1 = hero;

                            if(_selector.SelectedHero0.Type == _selector.SelectedHero1.Type)
                            {
                                _selector.SelectedHero1.OutLine(true);
                            }
                        }
                    }
                }
            }
        }
    }
}