using UnityEngine;

public class MapCenter : MonoBehaviour
{
    
    public Mapbox mapbox;

    public enum CenterOn { Drone, VR }
    public CenterOn centerOn = CenterOn.Drone;

    public void ApplyCenter()
    {
        if (mapbox == null)
        {
            Debug.LogError("[MapCenter] Mapbox is not assigned!");
            return;
        }

        centerOn = (centerOn == CenterOn.Drone) ? CenterOn.VR : CenterOn.Drone;

        mapbox.centerOn = (centerOn == CenterOn.Drone)
            ? Mapbox.CenterOn.Drone
            : Mapbox.CenterOn.VR;

        mapbox.ForceUpdate();
    }
}
