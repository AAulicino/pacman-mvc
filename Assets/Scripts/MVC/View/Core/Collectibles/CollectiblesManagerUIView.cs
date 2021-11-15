using TMPro;
using UnityEngine;

public class CollectiblesManagerUIView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public void SetCollectedCountText (string text) => textMeshProUGUI.text = text;
}
