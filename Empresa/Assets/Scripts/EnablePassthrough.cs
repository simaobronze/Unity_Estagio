using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePassthrough : MonoBehaviour 
{
	private bool oneTime = false;
    private void ActivatePassthrough()
    {
        for(int i=0; i<gameObject.transform.childCount; i++)
        {
			DesactivateMeshRenderersAndSons(gameObject.transform.GetChild(i).gameObject, false);
        }
        GameObject.Find("OVRCameraRig").GetComponent<OVRPassthroughLayer>().enabled = true;
	}

    private void DesactivatePassthrough()
    {
		GameObject.Find("OVRCameraRig").GetComponent<OVRPassthroughLayer>().enabled = false;
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			DesactivateMeshRenderersAndSons(gameObject.transform.GetChild(i).gameObject, true);
		}
	}

	void DesactivateMeshRenderersAndSons(GameObject objeto, bool state)
	{
		MeshRenderer meshRenderer;
		if (objeto.TryGetComponent<MeshRenderer>(out meshRenderer))
		{
			meshRenderer.enabled = state;
		}
		else
		{
			objeto.SetActive(state);
		}

		foreach (Transform child in objeto.transform)
		{
			DesactivateMeshRenderersAndSons(child.gameObject, state);
		}
	}

	private void Update()
	{
		if (OVRInput.GetDown(OVRInput.Button.One))
		{
			if (!oneTime)
			{
				ActivatePassthrough();
				oneTime = true;
			}
			else
			{
				DesactivatePassthrough();
				oneTime = false;
			}
		}
	}
}
