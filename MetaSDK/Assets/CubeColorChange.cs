using UnityEngine;

public class CubeColorChanger : MonoBehaviour
{
    public Renderer cubeRenderer;

    public void ChangeToRed()
    {
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = Color.red;
        }
        else
        {
            Debug.LogWarning("Renderer do cubo não foi atribuído!");
        }
    }

    public void ChangeToGreen()
    {
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = Color.green;
        }
        else
        {
            Debug.LogWarning("Renderer do cubo não foi atribuído!");
        }
    }

    public void ChangeToBlue()
    {
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = Color.blue;
        }
        else
        {
            Debug.LogWarning("Renderer do cubo não foi atribuído!");
        }
    }

    public void ChangeToYellow()
    {
        if (cubeRenderer != null)
        {
            cubeRenderer.material.color = Color.yellow;
        }
        else
        {
            Debug.LogWarning("Renderer do cubo não foi atribuído!");
        }
    }
}
