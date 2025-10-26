using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class UIModal : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}