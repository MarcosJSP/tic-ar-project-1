using UnityEngine;

public class ARCardData : MonoBehaviour
{
    [Header("Card Information")]
    [TextArea(1, 2)]
    public string title;
    [TextArea(3, 5)]
    public string description;
    public string youtubeURL;

    // Add any other data you want
    public Sprite cardImage;
}