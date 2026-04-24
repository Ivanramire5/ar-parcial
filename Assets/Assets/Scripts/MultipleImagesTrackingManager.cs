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
        Debug.LogError("!!! AAAAAAAAAAAAAAAAAAAAdffsdfdsfsdfAAAA");
        Debug.LogError("!!! EL SCRIPT ESTÁ VIVO");
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjects = new Dictionary<string, GameObject>();

        _trackedImageManager.trackablesChanged.AddListener(OnImagesTrackedChanged);

        SetupSceneElements();
    }

    private void OnDestroy()
    {
        if (_trackedImageManager != null)
            _trackedImageManager.trackablesChanged.RemoveListener(OnImagesTrackedChanged);
    }

    private void SetupSceneElements()
    {
        foreach (var prefab in prefabsToSpawn)
        {
            var arObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            // El nombre del objeto en el diccionario será "Marker-1" o "Marker-2"
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

        // CORRECCIÓN ERROR CS1061: Accedemos a .Value porque es un KeyValuePair
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
        if (trackedImage == null) return;

        string imageName = trackedImage.referenceImage.name;

        // Verificamos si el nombre de la imagen existe en nuestro diccionario de prefabs
        if (_arObjects.ContainsKey(imageName))
        {
            if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
            {
                _arObjects[imageName].SetActive(false);
            }
            else
            {
                _arObjects[imageName].SetActive(true);
                _arObjects[imageName].transform.position = trackedImage.transform.position;
                _arObjects[imageName].transform.rotation = trackedImage.transform.rotation;

                Debug.LogError(">>> MOSTRANDO PREFAB: " + imageName + " en posición: " + trackedImage.transform.position);
            }
        }
        else
        {
            // ERROR ROJO si el nombre no coincide (el error más común)
            Debug.LogError("!!! ERROR DE NOMBRES: El celu ve '" + imageName + "' pero en el diccionario solo tengo: " + string.Join(", ", _arObjects.Keys));
        }
    }
}