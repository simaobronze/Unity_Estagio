using UnityEngine;
using UnityEngine.UI;

public class ListToggler : MonoBehaviour
{
    public Toggle toggle;      
    public GameObject listPanel;  

    void Awake()
    {
        if (toggle == null || listPanel == null)
        {
            Debug.LogError("ListToggler and/or ListPanel not assigned!");
            enabled = false;
            return;
        }

        listPanel.SetActive(toggle.isOn);

        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        listPanel.SetActive(isOn);
    }
}
