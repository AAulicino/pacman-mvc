public class TimeProvider : ITimeProvider
{
    static TimeProvider _instance;
    public static TimeProvider Instance => _instance ?? (_instance = new TimeProvider());

    public float DeltaTime => UnityEngine.Time.deltaTime;
}
