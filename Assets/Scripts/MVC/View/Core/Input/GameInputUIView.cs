using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputUIView : MonoBehaviour, IPointerDownHandler
{
    public event Action<Direction> OnPointerDown;

    [SerializeField] Direction direction;

    void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
    {
        OnPointerDown?.Invoke(direction);
    }
}
