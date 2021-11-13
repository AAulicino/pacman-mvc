using UnityEngine;

public class InputWrapper : IInput
{
    public bool GetKey (KeyCode key) => Input.GetKey(key);
}
