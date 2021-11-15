using System.Collections;
using UnityEngine;

public class EnemyView : ActorView
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float frightenedSpriteSpeed = 1f;
    [SerializeField] Sprite[] frightenedSprites;

    [SerializeField] Sprite deadSprite;

    Sprite defaultSprite;
    WaitForSeconds frightenedWait;
    Coroutine frightenedCoroutine;

    void Awake ()
    {
        defaultSprite = spriteRenderer.sprite;
        frightenedWait = new WaitForSeconds(frightenedSpriteSpeed);
    }

    public void SetAsDead ()
    {
        StopFrightenedRoutine();
        spriteRenderer.sprite = deadSprite;
    }

    public void SetAsDefault ()
    {
        StopFrightenedRoutine();
        spriteRenderer.sprite = defaultSprite;
    }

    public void SetAsFrightened ()
    {
        StopFrightenedRoutine();
        frightenedCoroutine = StartCoroutine(FrightenedRoutine());
    }

    void StopFrightenedRoutine ()
    {
        if (frightenedCoroutine != null)
            StopCoroutine(frightenedCoroutine);
    }

    IEnumerator FrightenedRoutine ()
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = frightenedSprites[index++ % frightenedSprites.Length];
            yield return frightenedWait;
        }
    }
}
