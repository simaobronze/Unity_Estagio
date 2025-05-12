using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerRaycastLogger : MonoBehaviour
{
    [Header("Arrasta aqui os Transforms dos controladores")]
    public Transform leftController;
    public Transform rightController;

    [Header("Parâmetros do Raycast")]
    public float maxDistance = 10f;
    public LayerMask interactableLayers = ~0; // todos por defeito

    void Awake()
    {
        foreach (var ri in FindObjectsOfType<RayInteractable>())
        {
            if (ri.Surface == null)
                Debug.LogError($"RayInteractable sem Surface em: {ri.name}", ri);
        }
    }

    void Update()
    {
        // Dispara o raycast quando o trigger esquerdo é pressionado
        if (IsTriggerPressed(InputDeviceRole.LeftHanded))
            TryLogHit(leftController);

        // Dispara o raycast quando o trigger direito é pressionado
        if (IsTriggerPressed(InputDeviceRole.RightHanded))
            TryLogHit(rightController);
    }

    bool IsTriggerPressed(InputDeviceRole hand)
    {
        // Encontra o dispositivo (Left ou Right)
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithRole(hand, devices);
        foreach (var dev in devices)
        {
            if (dev.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed) && pressed)
                return true;
        }
        return false;
    }

    void TryLogHit(Transform controllerTransform)
    {
        Ray ray = new Ray(controllerTransform.position, controllerTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayers))
        {
            Debug.Log($"[{controllerTransform.name}] Atingiu: {hit.collider.gameObject.name}");
        }
    }
}
