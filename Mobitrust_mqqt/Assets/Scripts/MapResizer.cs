using UnityEngine;
using UnityEngine.UI;

public class MapResizer : MonoBehaviour
{
    public RectTransform mapCanvas;       // UI - Map Canvas
    public RectTransform streamCanvas;    // UI - Stream Canvas

    public enum SizeOptions { Small, Medium, Large }
    public SizeOptions sizeOption = SizeOptions.Small;
    public Mapbox mapbox; 

    private void Start()
    {
        if (mapCanvas == null || streamCanvas == null)
        {
            Debug.LogError("MapCanvas and/or StreamCanvas are not assigned!");
            return;
        }

        ApplyHardcodedSettings();
    }

    public void Resize()
    {
        ApplyHardcodedSettings();
    }

    private void ApplyHardcodedSettings()
    {
        switch (sizeOption)
        {
            case SizeOptions.Small:
                // Stream
                streamCanvas.SetAsFirstSibling();
                streamCanvas.anchorMin = new Vector2(0, 0);
                streamCanvas.anchorMax = new Vector2(0, 0);
                streamCanvas.pivot = new Vector2(0.5f, 0.5f);
                streamCanvas.anchoredPosition = new Vector2(-272f, 241f);
                streamCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 650f);
                streamCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 350f);
                streamCanvas.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                // Map
                mapCanvas.anchorMin = new Vector2(0.5f, 0.5f);
                mapCanvas.anchorMax = new Vector2(0.5f, 0.5f);
                mapCanvas.pivot = new Vector2(0.5f, 0.5f);
                mapCanvas.anchoredPosition = new Vector2(-682f, 331f);
                mapCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
                mapCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);
                mapCanvas.localScale = new Vector3(1f, 1f, 1f);
                mapbox.zoom = 8; 
                
                
                break;

            case SizeOptions.Medium:
                // Stream
                streamCanvas.anchorMin = new Vector2(0, 0);
                streamCanvas.anchorMax = new Vector2(0, 0);
                streamCanvas.pivot = new Vector2(0.5f, 0.5f);
                streamCanvas.anchoredPosition = new Vector2(-50f, 241f);
                streamCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 650f);
                streamCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 650f);
                streamCanvas.localScale = new Vector3(0.744285f, 0.744285f, 0.744285f);

                // Map
                mapCanvas.anchorMin = new Vector2(0.5f, 0.5f);
                mapCanvas.anchorMax = new Vector2(0.5f, 0.5f);
                mapCanvas.pivot = new Vector2(0.5f, 0.5f);
                mapCanvas.anchoredPosition = new Vector2(-582f, 196f);
                mapCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
                mapCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);
                mapCanvas.localScale = new Vector3(2.328084f, 2.328084f, 2.328084f);
                mapbox.zoom = 8;
                break;

            case SizeOptions.Large:
                // Stream
                streamCanvas.anchorMin = new Vector2(0, 0);
                streamCanvas.anchorMax = new Vector2(0, 0);
                streamCanvas.pivot = new Vector2(0.5f, 0.5f);
                streamCanvas.anchoredPosition = new Vector2(-619f, 365f);
                streamCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300f);
                streamCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);
                streamCanvas.localScale = new Vector3(0.744285f, 0.744285f, 0.744285f);

                // Map
                //Map should be the first child
                mapCanvas.SetAsFirstSibling();
                mapCanvas.anchorMin = new Vector2(0, 0);
                mapCanvas.anchorMax = new Vector2(0, 0);
                mapCanvas.pivot = new Vector2(0.5f, 0.5f);
                mapCanvas.anchoredPosition = new Vector2(-271f, 225f);
                mapCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 650f);
                mapCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 350f);
                mapCanvas.localScale = new Vector3(1.442571f, 1.442571f, 1.442571f);
                mapbox.zoom = 8;
                break;
        }
    }

    public void NextSizeOption()
    {
        var values = (SizeOptions[])System.Enum.GetValues(typeof(SizeOptions));
        int index = (System.Array.IndexOf(values, sizeOption) + 1) % values.Length;
        sizeOption = values[index];

        Resize();
    }
}
