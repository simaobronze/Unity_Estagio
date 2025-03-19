using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHandMenu : MonoBehaviour
{
	private Dictionary<string, GameObject> listPrimaryMenu = new();
	private GameObject raycaster;
	private CharacterController player;
	private bool oneTime = true;
	private GameObject lastObjHit = null;

	public void SetListMenu(string name, GameObject obj)
	{
		listPrimaryMenu.Add(name, obj);
	}

	private void Start()
	{
		player = GameObject.Find("OVRPlayerController").GetComponent<CharacterController>();
		raycaster = GameObject.Find("ControllerPointerPose");
		if (player == null || raycaster == null)
		{
			Debug.LogError("OVRPlayerController or ControllerPointerPose not found");
			return;
		}
	}

	void Update()
	{
		Ray ray = new(raycaster.transform.position, raycaster.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > 0)
			{
				if (listPrimaryMenu.TryGetValue(hit.transform.gameObject.name, out GameObject menuObj))
				{
					if (oneTime)
					{
						menuObj.SetActive(true);
						player.enabled = false;
						oneTime = false;
						lastObjHit = menuObj;
					}
				}
				if (hit.transform.gameObject == null)
				{
					if (listPrimaryMenu.TryGetValue("20m Epoxy Ground", out GameObject menuobj))
					{
						menuobj.SetActive(true);
						player.enabled = false;
						oneTime = false;
						lastObjHit = menuobj;
					}
				}
			}
			else
			{
				if (lastObjHit != null)
				{
					lastObjHit.SetActive(false);
					lastObjHit = null;
				}
				player.enabled = true;
				oneTime = true;
			}
		}
	}
}
