using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleImagesTrackingManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabsToSpawn = new List<GameObject>();
    private ARTrackedImageManager _trackedImageManager;
    private Dictionary<string, GameObject> _arObjects;

    private void Start()
{
    Debug.LogError("!!! EL SCRIPT ESTÁ VIVO");
    _trackedImageManager = GetComponent<ARTrackedImageManager>();
    _arObjects = new Dictionary<string, GameObject>();

    
    _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);

    SetupSceneElements();
}

private void OnDestroy()
{
    
    if (_trackedImageManager != null)
    {
        _trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }
}
    private void SetupSceneElements()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            
            arObject.name = prefab.name;
            arObject.gameObject.SetActive(false);
            _arObjects.Add(arObject.name, arObject);
        }
    }

    private void OnImagesTrackedChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            Debug.LogError("!!! CÁMARA DETECTÓ ALGO NUEVO: " + trackedImage.referenceImage.name);
            UpdateTrackedImages(trackedImage);
            
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateTrackedImages(trackedImage);
        }

        
        foreach (var trackedImagePair in eventArgs.removed)
        {
            var image = trackedImagePair.Value;
            if (_arObjects.ContainsKey(image.referenceImage.name))
            {
                _arObjects[image.referenceImage.name].SetActive(false);
            }
        }
    }

    private void UpdateTrackedImages(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        if (_arObjects.ContainsKey(imageName))
        {
            GameObject model = _arObjects[imageName];

            if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
            {
                model.SetActive(false);
            }
            else
            {
                model.SetActive(true);
                
                
                model.transform.SetParent(trackedImage.transform);
                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                
                
                model.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
    }
}