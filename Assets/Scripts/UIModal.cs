using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class UIModal : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    [Header("UI Elements")]
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [Header("Animation Settings")]
    [SerializeField] private float showDuration = 0.3f;
    [SerializeField] private float hideDuration = 0.15f; // Faster hide
    [SerializeField] private AnimationCurve popCurve;
    
    private Coroutine animationCoroutine;
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        // Default pop curve if none set
        if (popCurve == null || popCurve.length <= 1)
        {
            popCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1.1f), // Overshoot
                new Keyframe(1f, 1f)
            );
        }
        
        Hide();
    }
    
    public void Show(ARCardData cardData)
    {
        if (cardData != null)
        {
            titleText.text = cardData.title;
            descriptionText.text = cardData.description;
        }
        
        gameObject.SetActive(true);
        
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        
        animationCoroutine = StartCoroutine(PopIn());
    }
    
    public void Hide()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);
        
        animationCoroutine = StartCoroutine(PopOut());
    }
    
    private IEnumerator PopIn()
    {
        float elapsed = 0f;
        
        while (elapsed < showDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / showDuration);
            float scale = popCurve.Evaluate(t);
            
            contentPanel.localScale = Vector3.one * scale;
            canvasGroup.alpha = t;
            
            yield return null;
        }
        
        contentPanel.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        animationCoroutine = null;
    }
    
    private IEnumerator PopOut()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        float elapsed = 0f;
        
        while (elapsed < hideDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / hideDuration);
            
            contentPanel.localScale = Vector3.one * t;
            canvasGroup.alpha = t;
            
            yield return null;
        }
        
        contentPanel.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
        
        animationCoroutine = null;
    }
}