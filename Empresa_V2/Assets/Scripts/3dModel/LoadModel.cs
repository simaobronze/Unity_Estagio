using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
//using Siccity.GLTFUtility;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;

public class LoadModel : MonoBehaviour
{
	[SerializeField] public GameObject _handle;
	[SerializeField] public RayInteractable _rayGrab;
	[SerializeField] public GrabInteractable _grabGrab;
	[SerializeField] public Collider _collider;
	[SerializeField] private ImagesScriptableObject _imagesStore;
	private float width;

	private void Start()
	{
		//mapRenderer = GameObject.Find("Map").GetComponent<MapRenderer>();
		//width = mapRenderer.LocalMapDimension.x;
	}

	private void OnEnable()
	{
		_imagesStore.layerUpdatedEvent.AddListener(Load3dModel);
	}

	private void OnDisable()
	{
		_imagesStore.layerUpdatedEvent.RemoveListener(Load3dModel);
	}

    public void Load3dModel(WMS_Layer layer)
    {
        CleanModel();
        string url = null;
        if (layer != null && layer.link_3d != null && layer.link_3d != "")
        {
            url = layer.link_3d;
        }

        if (url == null)
        {
            return;
        }

		if (url.Contains("?"))
		{
			url = url.Split("?")[0];
		}
		url = "https://cloud.onesource.pt/s/WkPGXRaybmGt5g2/download/Rua-Porto-do-Meio-23-01-2024-textured_model.glb";
        StartCoroutine(LoadModelFromURL(url, async (model) =>
        {
            if (model == null)
            {
                Debug.LogError($"Model not loaded");
                return;
            }
            GLTFast.GltfImport gltf = new();
            if (await gltf.LoadGltfBinary(model, new Uri(url)))
            {
                if (await gltf.InstantiateMainSceneAsync(transform))
                {
                    AdjustModel();
                }
                else
                {
                    Debug.LogError($"Model not instantiated");
                }
            }
        }));
    }

    public void AdjustModel()
	{
		_handle.SetActive(true);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
			if (child.gameObject == _handle || child.gameObject == _grabGrab || child.gameObject == _rayGrab || child.gameObject == _collider)
			{
				continue;
			}
            if (child.TryGetComponent(out MeshFilter mesh))
            {
                Bounds bounds = mesh.mesh.bounds;
                float x = bounds.size.x;
                child.localScale = new Vector3(width / x, width / x, width / x);
                child.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
				break;
            }
        }
    }

	private void CleanModel()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject go = transform.GetChild(i).gameObject;
			if (go == _handle) { continue; }
			if (go.TryGetComponent(out Grabbable grabbable))
			{
				continue;
			}
			Destroy(go);
		}
	}

    IEnumerator LoadModelFromURL(string url, Action<byte[]?> callback)
	{
		Debug.Log($"LOAD MODEL FROM URL: {url}");
		byte[] model = null;
		UnityWebRequest webRequest;

        try
		{
			webRequest = UnityWebRequest.Get(url);
            webRequest.certificateHandler = new BypassCertificate();
        }
        catch (Exception e)
		{
			Debug.LogException(e);
			webRequest = null;
		}
		if (webRequest != null)
		{
            using (webRequest)
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
                else
                {
                    model = webRequest.downloadHandler.data;
                }
            }
        }
		callback(model);
	}

	//private void AddComponents(GameObject obj)
	//{
	//	Rigidbody rb = obj.AddComponent<Rigidbody>();
	//	rb.useGravity = false;
	//	rb.isKinematic = true;
	//	OneGrabTranslateTransformer grabTrans = obj.AddComponent<OneGrabTranslateTransformer>();
	//	grabTrans.Constraints.MinX.Constrain = true;
	//	grabTrans.Constraints.MinX.Value = -4f;
	//	grabTrans.Constraints.MaxX.Constrain = true;
	//	grabTrans.Constraints.MaxX.Value = 5.5f;
	//	grabTrans.Constraints.MinY.Constrain = true;
	//	grabTrans.Constraints.MinY.Value = 1;
	//	grabTrans.Constraints.MaxY.Constrain = true;
	//	grabTrans.Constraints.MaxY.Value = 2.5f;
	//	grabTrans.Constraints.MinZ.Constrain = true;
	//	grabTrans.Constraints.MinZ.Value = -1;
	//	grabTrans.Constraints.MaxZ.Constrain = true;
	//	grabTrans.Constraints.MaxZ.Value = 9f;
	//	var grabbable = obj.AddComponent<Grabbable>();
	//	grabbable.TransferOnSecondSelection = true;
	//	grabbable.InjectOptionalOneGrabTransformer(grabTrans);
	//	obj.AddComponent<InteractableTriggerBroadcaster>();
	//	obj.AddComponent<MoveModel>();
	//	obj.AddComponent<MoveFromTargetProvider>();
	//	var disthandgrab = obj.AddComponent<DistanceHandGrabInteractable>();
	//	disthandgrab.InjectOptionalPointableElement(grabbable);
	//	disthandgrab.InjectRigidbody(rb);
	//	var handgrab = obj.AddComponent<HandGrabInteractable>();
	//	handgrab.InjectOptionalPointableElement(grabbable);
	//	handgrab.InjectRigidbody(rb);
	//	var distgrab = obj.AddComponent<DistanceGrabInteractable>();
	//	distgrab.InjectOptionalPointableElement(grabbable);
	//	distgrab.InjectRigidbody(rb);
	//}
}
