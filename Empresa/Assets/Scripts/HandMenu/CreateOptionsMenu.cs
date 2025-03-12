using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CreateOptionsMenu : MonoBehaviour
{
    //public Icons Icons { get; private set; }
	public List<Menus> menus;
	[SerializeField] private string pathFolder = "IconsMenu";

	private void Awake()
	{
		if (menus == null)
		{
			menus = new List<Menus>();
		}
	}

	private void SetIconsDictionary()
	{
		//Icons.icons = new();
		//Sprite[] sprites = Resources.LoadAll<Sprite>(pathFolder);
		//foreach (Sprite sprite in sprites)
		//{
		//	Icons.icons.Add(sprite.name, sprite);
		//}
	}
}
