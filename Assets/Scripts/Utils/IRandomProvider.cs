public interface IRandomProvider
{
    float Value { get; }
    int Range (int a, int b);
    float Range (float a, float b);
}
