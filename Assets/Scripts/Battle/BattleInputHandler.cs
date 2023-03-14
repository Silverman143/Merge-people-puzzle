using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleInputHandler : MonoBehaviour
{
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log($"pointer down on pos = {eventData.pressPosition}");
    //}

    //private bool GetWorldCoordinats(out Vector3 pos)
    //{
    //    bool hit = false;



    //    return hit;
    //}

    private void OnMouseDown()
    {
        Vector2 mousePos = Input.mousePosition;

        // The ray to the touched object in the world
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Your raycast handling
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.transform.TryGetComponent<BattleInputHandler>(out BattleInputHandler inputHandler))
            {
                GlobalEvents.InputPositionChangedInvoke(hit.point);
            }

        }
    }

    private void OnMouseDrag()
    {
        Vector2 mousePos = Input.mousePosition;

        // The ray to the touched object in the world
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Your raycast handling
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.transform.TryGetComponent<BattleInputHandler>(out BattleInputHandler inputHandler))
            {
                GlobalEvents.InputPositionChangedInvoke(hit.point);
            }

        }

    }

    private void OnMouseUp()
    {
        Vector2 mousePos = Input.mousePosition;

        // The ray to the touched object in the world
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Your raycast handling
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.transform.TryGetComponent<BattleInputHandler>(out BattleInputHandler inputHandler))
            {
                GlobalEvents.InputPositionChangedInvoke(hit.point);
                GlobalEvents.InputPositionEndedInvoke();
            }

        }
    }
}
