using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class MenuOptions
{
	public string Key;
	public UnityEvent action;
	public List<MenuOptions> options;
}

[System.Serializable]
public class Icons
{
	public Dictionary<string, Sprite> icons;
}

[System.Serializable]
public class Menus
{
	public GameObject objAssociate;
	public List<MenuOptions> menus;
}

