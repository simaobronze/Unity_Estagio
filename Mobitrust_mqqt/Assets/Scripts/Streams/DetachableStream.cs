using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachableStream : MonoBehaviour
{
    [SerializeField] private Janus stream;
    private GameObject player;
    private DistanceGrabInteractor distanceGrabInteractor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("OVRCameraRig");
        GameObject controllerDistanceGrabInteractor = GameObject.Find("ControllerDistanceGrabInteractor");
        if (controllerDistanceGrabInteractor == null) { return; }
        controllerDistanceGrabInteractor.TryGetComponent(out distanceGrabInteractor);
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceGrabInteractor == null) { return; }
        if (distanceGrabInteractor.Interactable == null) { return; }
        if (distanceGrabInteractor.Interactable.transform.parent.gameObject != gameObject) { return; }
        if (player == null) { return; }
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) <= 0) { return; }
        Vector3 relativePos = player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation((relativePos + new Vector3(0f, 1f, 0f)), Vector3.up);
    }

    public void SetStream(User user, ulong streamId)
    {
        if (stream == null)
        {
            return;
        }
        stream.StreamId = streamId;
    }
}
