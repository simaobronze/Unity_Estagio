using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ImagesScriptableObject", menuName = "ScriptableObjects/Images")]
public class ImagesScriptableObject : ScriptableObject
{
    public WMS_Layer Layer { get; private set; }
	public List<WMS_Layer> Layers { get; private set; } = new();

    [NonSerialized]
    public UnityEvent<WMS_Layer> layerUpdatedEvent;


    private void OnEnable()
    {
        layerUpdatedEvent ??= new();
    }

    private void OnDisable()
    {
        layerUpdatedEvent = null;
    }


    public void Show3DImage(WMS_Layer layer)
    {
        Layer = layer;
		layerUpdatedEvent.Invoke(layer);
    }
}
