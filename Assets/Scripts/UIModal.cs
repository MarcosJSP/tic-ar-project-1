using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIModal : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Hide(); // Start hidden
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        // Or use: gameObject.SetActive(false);
    }
}