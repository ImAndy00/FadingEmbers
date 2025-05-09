using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlashUI : MonoBehaviour
{
    public Image flashImage;
    private Coroutine _flashRoutine;

    public void Flash(Color color, float totalDuration)
    {
        if (_flashRoutine != null) 
            StopCoroutine(_flashRoutine);

        _flashRoutine = StartCoroutine(DoFlash(color, totalDuration));
    }

    private IEnumerator DoFlash(Color color, float totalDuration)
    {
        float half = totalDuration * 0.5f;
        // fade in
        for (float t = 0f; t < half; t += Time.unscaledDeltaTime)
        {
            float a = Mathf.Lerp(0f, color.a, t / half);
            flashImage.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }
        // fade out
        for (float t = 0f; t < half; t += Time.unscaledDeltaTime)
        {
            float a = Mathf.Lerp(color.a, 0f, t / half);
            flashImage.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }
        flashImage.color = new Color(color.r, color.g, color.b, 0f);
        _flashRoutine = null;
    }

    public void FlashDamage() =>
        Flash(new Color(1,0,0,0.5f), 0.2f);
        
    public void FlashDeath() =>
        Flash(new Color(1,1,1,1f), 1f);
}
