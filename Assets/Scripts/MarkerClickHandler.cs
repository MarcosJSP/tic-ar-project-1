using UnityEngine;
using UnityEngine.EventSystems;

public class MarkerClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject uiPanel; // Assign your UI panel or Canvas in Inspector

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning($"{name} has no UI panel assigned!");
        }
    }
}