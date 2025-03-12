using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModel : MonoBehaviour
{
	private GameObject player;
	private DistanceGrabInteractor distanceGrabInteractor;

	void Start()
	{
		player = GameObject.Find("OVRCameraRig");
		GameObject controllerDistanceGrabInteractor = GameObject.Find("ControllerDistanceGrabInteractor");
		if (controllerDistanceGrabInteractor == null) { return; }
		controllerDistanceGrabInteractor.TryGetComponent(out distanceGrabInteractor);
	}

	void Update()
	{
		if (distanceGrabInteractor == null) { return; }
		if (distanceGrabInteractor.Interactable == null) { return; }
		if (distanceGrabInteractor.Interactable.transform.parent.gameObject != gameObject) { return; }
		if (player == null) { return; }
		Debug.Log("ChegouAQui");
		if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) <= 0) { return; }
		Vector3 relativePos = player.transform.position - transform.position;
		transform.rotation = Quaternion.LookRotation((relativePos + new Vector3(0f, 1f, 0f)), Vector3.up);
	}
}
