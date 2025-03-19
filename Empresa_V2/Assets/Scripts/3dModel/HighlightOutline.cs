using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightOutline : MonoBehaviour
{
    private Outline _outline;

    public void OnHover()
    {
        if (_outline == null)
        {
            _outline = gameObject.AddComponent<Outline>();
            if (_outline == null)
            {
                Debug.LogError($"Error getting {nameof(Outline)} component");
                return;
            }
        }
        else
        {
            _outline.enabled = true;
        }
        _outline.OutlineColor = Color.magenta;
        _outline.OutlineWidth = 7;
    }

    public void OnUnhover()
    {
        if ( _outline != null )
        {
            _outline.enabled = false;
        }
    }
}
