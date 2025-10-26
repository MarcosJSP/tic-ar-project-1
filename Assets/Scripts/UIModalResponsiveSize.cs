using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class UIModalResponsiveSize : MonoBehaviour
{
    [System.Serializable]
    public class OrientationSettings
    {
        [Header("Relative Size (% of screen)")]
        [Range(0f, 1f)] public float widthPercent = 0.9f;
        [Range(0f, 1f)] public float heightPercent = 0.7f;

        [Header("Minimum Absolute Size (pixels)")]
        public float minWidth = 300f;
        public float minHeight = 200f;

        [Header("Maximum Absolute Size (optional, 0 = ignore)")]
        public float maxWidth = 0f;
        public float maxHeight = 0f;
    }

    [Header("Portrait Settings")]
    public OrientationSettings portrait = new OrientationSettings { widthPercent = 0.9f, heightPercent = 0.7f };

    [Header("Landscape Settings")]
    public OrientationSettings landscape = new OrientationSettings { widthPercent = 0.7f, heightPercent = 0.9f };

    private RectTransform rect;
    private bool lastIsLandscape;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        ApplySize();
    }

    void Update()
    {
        bool isLandscape = Screen.width > Screen.height;

        // React only when orientation changes or while editing in Editor
        if (Application.isPlaying)
        {
            if (isLandscape != lastIsLandscape)
            {
                lastIsLandscape = isLandscape;
                ApplySize();
            }
        }
        else
        {
            ApplySize();
        }
    }

    private void ApplySize()
    {
        if (rect == null) return;

        bool isLandscape = Screen.width > Screen.height;
        var settings = isLandscape ? landscape : portrait;

        float targetWidth = Screen.width * settings.widthPercent;
        float targetHeight = Screen.height * settings.heightPercent;

        // Clamp between min and max (if set)
        if (settings.maxWidth > 0)
            targetWidth = Mathf.Min(targetWidth, settings.maxWidth);
        if (settings.maxHeight > 0)
            targetHeight = Mathf.Min(targetHeight, settings.maxHeight);

        targetWidth = Mathf.Max(settings.minWidth, targetWidth);
        targetHeight = Mathf.Max(settings.minHeight, targetHeight);

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
    }
}
