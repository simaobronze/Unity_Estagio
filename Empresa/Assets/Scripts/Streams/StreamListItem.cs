using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StreamListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStream(Stream stream)
    {
        _text.text = stream.description;
    }
}
