using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionLayerItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ImagesScriptableObject _imageStore;
    [SerializeField] TextMeshProUGUI _layerName;
    private WMS_Layer _layer;
    private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _toggle))
        {
            UnityEngine.Debug.LogError($"Error getting {typeof(Toggle).Name}");
            return;
        }
    }

    private void OnEnable()
    {
        _imageStore.layerUpdatedEvent.AddListener(HandleLayerUpdate);
    }

    private void OnDisable()
    {
        _imageStore.layerUpdatedEvent.RemoveListener(HandleLayerUpdate);
    }

    private void HandleLayerUpdate(WMS_Layer layer)
    {
        if (_toggle == null)
        {
            return;
        }
        if (layer == null)
        {
            _toggle.isOn = false;
        }
        _toggle.isOn = layer.layer == _layer.layer;
    }

    public void SetLayer(WMS_Layer layer)
    {
        _layer = layer;
        _layerName.text = layer.layer;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        try
        {
            WMS_Layer layer = _layer;

            if (_imageStore.Layer == null && _imageStore.Layer?.layer == _layer.layer)
            {
                layer = null;
            }
            _imageStore.Show3DImage(layer);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"Error on pointer click [{new StackTrace(e, true).GetFrame(0).GetFileLineNumber()}] [{e}]");
        }
    }
}
