using UnityEngine;
using TMPro;

public class ARCardLabel : MonoBehaviour
{
    void OnValidate()
    {
        UpdateLabel();
    }
    
    void Start()
    {
        UpdateLabel();
    }
    
    private void UpdateLabel()
    {
        ARCardData cardData = GetComponentInParent<ARCardData>();
        TextMeshProUGUI labelText = GetComponentInChildren<TextMeshProUGUI>(true);
        
        if (cardData != null && labelText != null)
        {
            labelText.text = cardData.title;
        }
        else
        {
            Debug.LogWarning("Couldn't find CardData or TMP component");
        }
    }
}