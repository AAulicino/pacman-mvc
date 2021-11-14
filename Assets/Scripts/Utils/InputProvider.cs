using UnityEngine;

public class InputProvider : IInputProvider
{
    static InputProvider _instance;
    public static InputProvider Instance => _instance ?? (_instance = new InputProvider());

    public bool GetKey (KeyCode key) => Input.GetKey(key);
}
