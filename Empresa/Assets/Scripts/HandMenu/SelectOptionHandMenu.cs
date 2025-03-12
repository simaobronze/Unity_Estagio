using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectOptionHandMenu : MonoBehaviour
{
	[SerializeField] Color optionSelectColor;
	[SerializeField] Color optionColor;
	[SerializeField] int distanceRaycast = 20;
	private int numOptions = -1;
	private GameObject[] listOptions;
	private GameObject listMenu;
	private int index = 0;
	private bool oneTime = true;
	private CharacterController player;
	private Dictionary<string, UnityEvent> optionFunction = new();
	static bool selectOption = true;

	public void SetOptions(int length, GameObject[] list)
	{
		numOptions = length;
		listOptions = new GameObject[numOptions];
		listOptions = list;
	}

	public void AddOptionFunction(string key, UnityEvent value)
	{
		optionFunction.Add(key, value);
	}

	private void Start()
	{
		player = GameObject.Find("OVRPlayerController").GetComponent<CharacterController>();
		if (ColorUtility.TryParseHtmlString("#004AB9", out Color newColor))
		{
			optionSelectColor = newColor;
		}
		if (ColorUtility.TryParseHtmlString("#5C5E5E", out Color newColor1))
		{
			optionColor = newColor1;
		}
	}

	private void Update()
	{
		if (numOptions < 0)
			return;
		if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch) == Vector2.zero)
			selectOption = true;
		if (OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) <= 0)
		{
			gameObject.SetActive(false);
			player.enabled = true;
		}
		else
		{
			player.enabled = false;
			if (selectOption)
				SelectOption();
		}
	}

	public void SelectOption()
	{ 
		listMenu = transform.parent.gameObject;
		Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
		Vector2 direction = input.normalized;
		if (input != Vector2.zero)
		{
			float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
			if (angle < 0)
				angle += 360;
			listOptions[index].GetComponent<Image>().color = optionColor;
			index = (int)angle / (360 / numOptions);
			listOptions[index].GetComponent<Image>().color = optionSelectColor;
			if (oneTime)
			{
				oneTime = false;
				bool done = false;
				for (int i = 0; i < listMenu.transform.childCount; i++)
				{
					Transform child = listMenu.transform.GetChild(i);
					if (child.name == listOptions[index].name)
					{
						selectOption = false;
						listOptions[index].GetComponent<Image>().color = optionColor;
						gameObject.SetActive(false);
						child.gameObject.SetActive(true);
						done = true;
					}
				}
				if (!done)
				{
					if (optionFunction.ContainsKey(listOptions[index].name))
					{
						optionFunction[listOptions[index].name].Invoke();
					}
				}
			}
		}
		else
		{
			listOptions[index].GetComponent<Image>().color = optionColor;
			oneTime = true;
		}
	}		
}
