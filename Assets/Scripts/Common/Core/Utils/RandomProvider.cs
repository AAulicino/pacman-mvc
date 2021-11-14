using UnityEngine;

public class RandomProvider : IRandomProvider
{
    static RandomProvider _instance;
    public static RandomProvider Instance => _instance ?? (_instance = new RandomProvider());

    public float Value => Random.value;
    public int Range (int a, int b) => Random.Range(a, b);
    public float Range (float a, float b) => Random.Range(a, b);
}
