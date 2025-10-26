using UnityEngine;

public class ARCardData : MonoBehaviour
{
    [Header("Card Information")]
    public string title;
    [TextArea(3, 5)]
    public string description;
    
    // Add any other data you want
    public Sprite cardImage;
}