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
    [SerializeField] TextMeshProUGUI startGameButtonText;

    [field: SerializeField]
    public Canvas Canvas { get; private set; }

    void Awake ()
    {
        startGameButton.onClick.AddListener(() => OnClick?.Invoke());
    }

    public void SetGameEndMessageActive (bool active)
        => endGameMessageText.gameObject.SetActive(active);

    public void SertEndGameMessageText (string text) => endGameMessageText.text = text;

    public void SetStartGameUIActive (bool active) => startGameUI.SetActive(active);

    public void SetStartGameButtonText (string text) => startGameButtonText.text = text;
}
