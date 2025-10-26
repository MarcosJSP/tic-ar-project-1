using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UISlideSpring : MonoBehaviour
{
    [Header("Animation Settings")]
    public float slideDistance = 300f; // how far below to start
    public float slideDuration = 0.5f; // total animation time
    public AnimationCurve bounceCurve;

    private RectTransform rect;
    private Vector2 startPos;
    private Vector2 hiddenPos;
    private Coroutine animRoutine;

    private bool isVisible = false;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
        startPos = rect.anchoredPosition;
        hiddenPos = startPos + new Vector2(0, -slideDistance);

        // optional default curve if none set
        if (bounceCurve == null || bounceCurve.length <= 1)
        {
            bounceCurve = new AnimationCurve(
                new Keyframe(0f, 0f, 0f, 2f),
                new Keyframe(0.6f, 1.2f, 0f, 0f),
                new Keyframe(1f, 1f)
            );
        }

        // Start hidden but active so it can animate
        rect.anchoredPosition = hiddenPos;
    }

    public void Show()
    {
        if (isVisible) return;
        isVisible = true;
        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(Slide(hiddenPos, startPos));
    }

    public void Hide()
    {
        if (!isVisible) return;
        isVisible = false;
        if (animRoutine != null) StopCoroutine(animRoutine);
        animRoutine = StartCoroutine(Slide(startPos, hiddenPos));
    }

    private IEnumerator Slide(Vector2 from, Vector2 to)
    {
        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            float curveT = bounceCurve.Evaluate(t);
            rect.anchoredPosition = Vector2.LerpUnclamped(from, to, curveT);
            yield return null;
        }

        rect.anchoredPosition = to;
        animRoutine = null;
    }
}
