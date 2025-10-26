using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIModal : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    [Header("UI Elements")]
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button watchVideoButton; // Add this
    
    [Header("Animation Settings")]
    [SerializeField] private float showDuration = 0.3f;
    [SerializeField] private float hideDuration = 0.15f;
    [SerializeField] private AnimationCurve popCurve;
    
    private Coroutine animationCoroutine;
    private string currentVideoURL;
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (popCurve == null || popCurve.length <= 1)
        {
            popCurve = new AnimationCurve(
                new Keyframe(0f, 0f),
                new Keyframe(0.5f, 1.1f),
                new Keyframe(1f, 1f)
            );
        }
        
        // Setup button click
        if (watchVideoButton != null)
        {
            watchVideoButton.onClick.AddListener(OpenVideo);
        }
        
        Hide();
    }
    
    public void Show(ARCardData cardData)
    {
        if (cardData != null)
        {
            titleText.text = cardData.title;
            descriptionText.text = cardData.description;
            currentVideoURL = cardData.youtubeURL;
            
            // Show/hide button based on whether URL exists
            if (watchVideoButton != null)
            {
                watchVideoButton.gameObject.SetActive(!string.IsNullOrEmpty(currentVideoURL));
            }
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
    
    private void OpenVideo()
    {
        if (!string.IsNullOrEmpty(currentVideoURL))
        {
            Application.OpenURL(currentVideoURL);
        }
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