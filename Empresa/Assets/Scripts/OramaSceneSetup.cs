using System.Collections.Generic;
using UnityEngine;

public class OramaSceneSetup : MonoBehaviour
{
    [SerializeField]
    private GameObject _handPhySprefab;

    private DissectedGameObject _LHAndPhyS;
    private DissectedGameObject _RHAndPhyS;

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> hands = new List<GameObject>();
        GameObject xrRig = GameObject.Find("XRRig");
        hands.Add(GameObject.Find("XRRig/HandInteractorL"));
        hands.Add(GameObject.Find("XRRig/HandInteractorR"));

        xrRig.AddComponent<DissectedGameObject>();

        if (xrRig == null)
        {
            Debug.Log("xrRig not found");
        }
        else
        {
            xrRig.transform.position = new Vector3(0,0,transform.position.z-1);
            xrRig.transform.rotation = new Quaternion(0, -90, 0, 0);
        }

        Debug.Log($"Hands count: {hands.Count}");
        if (hands.Count == 2) 
        {
            foreach (GameObject hand in hands)
            {
                //GameObject handForPhyS = Instantiate(_handPhySprefab,xrRig.transform);
                //handForPhyS.name = hand.name[^1] + handForPhyS.name;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
