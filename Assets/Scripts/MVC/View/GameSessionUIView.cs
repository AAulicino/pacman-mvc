using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSessionUIView : MonoBehaviour
{
    public event Action OnClick;

    [SerializeField] GameObject startGameUI;
    [SerializeField] Button startGameButton;
    [SerializeField] TextMeshProUGUI endGameMessageText;

    void Awake ()
    {
        startGameButton.onClick.AddListener(() => OnClick?.Invoke());
    }

    public void SetGameEndMessageActive (bool active)
        => endGameMessageText.gameObject.SetActive(active);

    public void SertEndGameMessageText (string text) => endGameMessageText.text = text;

    public void SetStartGameUIActive (bool active) => startGameUI.SetActive(active);
}
