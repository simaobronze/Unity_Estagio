using TMPro;
using UnityEngine;

public class PinDrone : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI _label;


    internal void UpdateDroneAltitude(double altitudeInMeters)
    {
        _label.text = "Altitude: " + altitudeInMeters.ToString("F0");
    }

    void Start()
	{
		

	}
	private void Update()
	{
		
	}
}
