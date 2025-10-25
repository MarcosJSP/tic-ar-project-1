using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{

    // Prefabs to spawn
    [SerializeField] List<GameObject> prefabsToSpawn = new List<GameObject>();

    // ARTrackedImageManager reference
    private ARTrackedImageManager _trackedImageManager;

    // Dictionary to referecnce spawned prefabs with tracked image name
    private Dictionary<string, GameObject> _arObjects;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        if (_trackedImageManager == null)
        {
            Debug.LogError("ARTrackedImageManager component not found in the GameObject.");
            return;
        }
        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);
        _arObjects = new Dictionary<string, GameObject>();
        SetupSceneElements();
    }

    private void OnDestroy()
    {
        if (_trackedImageManager != null)
        {
            _trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
        }
    }

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // Handle added images
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateTrackedImages(trackedImage);
        }

        // Handle updated images
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImages(trackedImage);
        }

        // Handle removed images
        foreach (var trackedImage in eventArgs.removed)
        {
            UpdateTrackedImages(trackedImage.Value);
        }
    } 

    private void UpdateTrackedImages(ARTrackedImage trackedImage)
    {
        if (trackedImage == null) return;

        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            _arObjects[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        _arObjects[trackedImage.referenceImage.name].SetActive(true);
        _arObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        _arObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
    }  

    // Setup scene elements
    private void SetupSceneElements()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            arObject.name = prefab.name;
            arObject.SetActive(false);
            _arObjects.Add(arObject.name, arObject);
        }
    }
}
