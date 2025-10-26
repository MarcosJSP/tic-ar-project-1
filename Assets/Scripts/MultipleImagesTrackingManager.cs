using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    [Header("AR Setup")]
    [SerializeField] private List<GameObject> prefabsToSpawn = new List<GameObject>();
    
    [Header("UI")]
    [SerializeField] private GameObject navBar;
    [SerializeField] private UIModal modal;
    
    private ARTrackedImageManager _trackedImageManager;
    private Dictionary<string, GameObject> _arObjects = new();
    private int _visibleImageCount = 0;
    private ARCardData currentCardData;
    
    void Start()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        SetupSceneElements();
    }
    
    private void OnDestroy()
    {
        if (_trackedImageManager != null)
            _trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }
    
    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            UpdateTrackedImages(trackedImage);
        
        foreach (var trackedImage in eventArgs.updated)
            UpdateTrackedImages(trackedImage);
        
        foreach (var trackedImage in eventArgs.removed)
            HandleImageRemoved(trackedImage.Value);
        
        // Update UI visibility based on tracked images
        var uiSpring = navBar.GetComponent<UISlideSpring>();
        if (uiSpring != null)
        {
            if (_visibleImageCount > 0)
                uiSpring.Show();
            else
                uiSpring.Hide();
        }
    }
    
    private void UpdateTrackedImages(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;
        
        string name = trackedImage.referenceImage.name;
        if (!_arObjects.TryGetValue(name, out var obj)) return;
        
        bool isTracked = trackedImage.trackingState == TrackingState.Tracking;
        bool wasActive = obj.activeSelf;
        
        // Only update count if state changed
        if (isTracked && !wasActive)
        {
            _visibleImageCount++;
            obj.SetActive(true);
            // Store reference to this card's data
            currentCardData = obj.GetComponent<ARCardData>();
        }
        else if (!isTracked && wasActive)
        {
            _visibleImageCount = Mathf.Max(0, _visibleImageCount - 1);
            obj.SetActive(false);
            if (_visibleImageCount == 0)
                currentCardData = null;
        }
        
        if (isTracked)
        {
            obj.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
        }
    }
    
    private void HandleImageRemoved(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        if (_arObjects.TryGetValue(name, out var obj))
        {
            if (obj.activeSelf)
                _visibleImageCount = Mathf.Max(0, _visibleImageCount - 1);
            obj.SetActive(false);
        }
    }
    
    private void SetupSceneElements()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.name = prefab.name;
            obj.SetActive(false);
            _arObjects.Add(obj.name, obj);
        }
    }
    
    public void OpenModal()
    {
        if (currentCardData != null)
        {
            modal.Show(currentCardData);
        }
    }
}