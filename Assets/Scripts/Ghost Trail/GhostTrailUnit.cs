using System.Collections;
using UnityEngine;

public class GhostTrailUnit : MonoBehaviour
{
    public bool IsActive { get; private set; }
    public float FadeoutSpeed;
    public SpriteRenderer SpriteRenderer;

    public void SetAlpha(float alpha)
    {
        Color color = new Color(
            SpriteRenderer.color.r, 
            SpriteRenderer.color.g, 
            SpriteRenderer.color.b, 
            alpha
        );
        SpriteRenderer.color = color;
    }

    public void Fadeout()
    {
        if (IsActive)
            return;

        StartCoroutine(Execute());

        IEnumerator Execute()
        {
            IsActive = true;
            while (SpriteRenderer.color.a > 0)
            {
                SetAlpha(SpriteRenderer.color.a - Time.deltaTime * FadeoutSpeed);
                yield return null;
            }
            IsActive = false;
        }
    }
}
