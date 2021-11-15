using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameInputUIView : MonoBehaviour, IPointerDownHandler
{
    public event Action<Direction> OnPointerDown;

    [SerializeField] Image image;

    [field: SerializeField]
    public Direction Direction { get; private set; }

    void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
    {
        OnPointerDown?.Invoke(Direction);
    }

    public void SetSelected (bool selected) => image.color = selected ? Color.white : Color.gray;
}
