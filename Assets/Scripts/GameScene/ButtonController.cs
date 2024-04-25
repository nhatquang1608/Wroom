using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private enum ControlType
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField] private ControlType controlType;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch(controlType)
        {
            case ControlType.Up:
                CarController.moveDirection = new Vector2(1, CarController.moveDirection.y);
                break;
            case ControlType.Down:
                CarController.moveDirection = new Vector2(-1, CarController.moveDirection.y);
                break;
            case ControlType.Left:
                CarController.moveDirection = new Vector2(CarController.moveDirection.x, -1);
                break;
            case ControlType.Right:
                CarController.moveDirection = new Vector2(CarController.moveDirection.x, 1);
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch(controlType)
        {
            case ControlType.Up:
                CarController.moveDirection = new Vector2(0, CarController.moveDirection.y);
                break;
            case ControlType.Down:
                CarController.moveDirection = new Vector2(0, CarController.moveDirection.y);
                break;
            case ControlType.Left:
                CarController.moveDirection = new Vector2(CarController.moveDirection.x, 0);
                break;
            case ControlType.Right:
                CarController.moveDirection = new Vector2(CarController.moveDirection.x, 0);
                break;
        }
    }
}
