using UnityEngine;
using UnityEngine.UI;

public class MapResizer : MonoBehaviour
{
    public RawImage map;

    public enum SizeOptions { Small, Medium, Large }
    public SizeOptions sizeOption = SizeOptions.Small;

    [SerializeField] private Vector2 smallSize = new Vector2(200, 200);
    [SerializeField] private Vector2 mediumSize = new Vector2(300, 300);
    [SerializeField] private Vector2 largeSize = new Vector2(400, 400);

    private void Start()
    {
        if (map == null)
        {
            Debug.LogError("Map não está atribuído!");
            return;
        }
        Resize();
    }

    public void Resize()
    {
        Vector2 target = sizeOption switch
        {
            SizeOptions.Small => smallSize,
            SizeOptions.Medium => mediumSize,
            SizeOptions.Large => largeSize,
            _ => smallSize
        };

        // garante width/height absolutos
        RectTransform rt = map.rectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target.x);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, target.y);

        Debug.Log($"Map resized to {target.x}×{target.y}");
    }

    public void NextSizeOption()
    {
        int next = ((int)sizeOption + 1)
                   % System.Enum.GetValues(typeof(SizeOptions)).Length;
        sizeOption = (SizeOptions)next;
        Resize();
        Debug.Log($"Switched to {sizeOption}");
    }
}
