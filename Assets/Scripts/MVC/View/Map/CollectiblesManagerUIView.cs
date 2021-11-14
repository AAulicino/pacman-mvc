using TMPro;
using UnityEngine;

public class CollectiblesManagerUIView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;

    public void SetCollectedCount (int count) => textMeshProUGUI.text = count.ToString();
}
