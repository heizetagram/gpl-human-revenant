using System.Collections;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public float duration;
    public SpriteRenderer spriteRenderer;

    public void ShowMuzzleFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(duration);
        spriteRenderer.enabled = false;
    }
}