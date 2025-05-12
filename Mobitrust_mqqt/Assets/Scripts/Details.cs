using UnityEngine;

public class Details : MonoBehaviour
{
    public GameObject sideBar;
    public GameObject underBar;

    public void Switch() { 
        if (sideBar.activeSelf && underBar.activeSelf)
        {
            sideBar.SetActive(false);
            underBar.SetActive(false);
        }
        else
        {
            sideBar.SetActive(true);
            underBar.SetActive(true);
        }
    }
}
