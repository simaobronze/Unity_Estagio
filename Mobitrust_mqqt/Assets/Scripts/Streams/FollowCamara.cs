using Oculus.Platform;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    public Transform vrCamera;
    public Vector3 offset = new(0, 0, 10);

    
    void LateUpdate()
    {
        if (vrCamera != null)
        {
            // Atualiza a posição da câmera com base na posição do objeto
            
            // Faz com que o painel olhe sempre na direção da câmara, ou opcionalmente mantenha uma rotação fixa.
            transform.SetPositionAndRotation(vrCamera.position + vrCamera.forward * offset.z
                                                 + vrCamera.up * offset.y
                                                 + vrCamera.right * offset.x, Quaternion.LookRotation(transform.position - vrCamera.position));
        }
        else
        {
            Debug.LogError("vrCamara não está definido. A câmera não pode seguir o objeto.");
        }
    }

}
