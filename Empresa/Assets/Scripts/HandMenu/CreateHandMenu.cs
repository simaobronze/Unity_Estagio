using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateHandMenu : MonoBehaviour
{
	[SerializeField] GameObject handMenu;
	[SerializeField] Color backgroundColor;
	[SerializeField] Color optionColor;
	[SerializeField] private GameObject icon;
	[SerializeField, Range(1, 20)] float space = 1;

	private readonly string colorValueOption = "#5C5E5E";
	private readonly string colorValueBck = "#1C2222";
	private float handMenuRadius = -1;
	private GameObject option;
	private List<Menus> menus;
	private CreateOptionsMenu createOptionsMenu;

	private void Start()
	{
		InitializeColors();
		space /= 100;
		StartCoroutine(WaitForComponentAndAssignMenus());
	}

	IEnumerator WaitForComponentAndAssignMenus()
	{
		while (createOptionsMenu == null)
		{
			createOptionsMenu = gameObject.GetComponent<CreateOptionsMenu>();
			yield return null;
		}
		menus = createOptionsMenu.menus;
		CreateMenus();
	} 

	private void CreateMenus()
	{
		foreach(var menu in menus)
		{
			if (menu.menus.Count > 0)
			{
				CreateMenu(menu.menus, menu.objAssociate.name, true);
			}
		}
	}

	private void CreateMenu(List<MenuOptions> options, string name, bool addPrimaryMenu)
	{
		int numOptions = options.Count;
		float fill = 1f;
		float optionFill = (fill / numOptions);
		GameObject [] listOptions = new GameObject[numOptions];
		GameObject menu = Instantiate(handMenu, GameObject.FindGameObjectWithTag("Menus").transform);
		if (handMenu != null && handMenu.GetComponent<RectTransform>() != null)
		{
			handMenuRadius = (menu.GetComponent<RectTransform>().sizeDelta.x - menu.GetComponent<RectTransform>().sizeDelta.x / 3) / 2;
		}
		for (int i = 0; i < menu.transform.childCount; i++)
			if (menu.transform.GetChild(i).name == "Background")
			{
				option = menu.transform.GetChild(i).gameObject;
				continue;
			}
		if (numOptions == 1)
		{
			_ = CreateOption(0, menu, listOptions, fill, options[0].Key);
		}
		else
		{
			optionFill -= space;
			for (int i = 0; i < numOptions; i++)
			{
				fill = CreateOption(i, menu, listOptions, fill, options[i].Key);
				if (options[i].action.GetPersistentEventCount() != 0)
				{
					menu.GetComponent<SelectOptionHandMenu>().AddOptionFunction(options[i].Key, options[i].action);
				}
				fill = CreateSpace(menu, fill, optionFill);
			}
		}
		CreateIcon(menu, numOptions, name);
		if(addPrimaryMenu)
		{
			gameObject.GetComponent<ActivateHandMenu>().SetListMenu(name, menu);			
		}
		menu.GetComponent<SelectOptionHandMenu>().SetOptions(numOptions, listOptions);
		menu.name = name;
		menu.SetActive(false);
		foreach (var option in options)
		{
			if (option.options.Count > 0)
			{
				CreateMenu(option.options, option.Key, false);
			}
		}
	}

	private float CreateOption(int index, GameObject menu, GameObject[] listOptions, float fill, string name)
	{
		listOptions[index] = Instantiate(option, menu.transform);
		listOptions[index].GetComponent<Image>().color = optionColor;
		listOptions[index].name = name;
		if (listOptions.Length == 1)
			return listOptions[index].GetComponent<Image>().fillAmount = fill;
		if (index == 0)
			fill -= (space / 2);
		else
			fill -= space;
		return listOptions[index].GetComponent<Image>().fillAmount = fill;
	}

	private float CreateSpace(GameObject menu, float fill, float optionFill)
	{
		GameObject temp = Instantiate(option, menu.transform);
		temp.GetComponent<Image>().color = backgroundColor;
		fill -= optionFill;
		return temp.GetComponent<Image>().fillAmount = fill;
	}
	private void InitializeColors()
	{
		if (UnityEngine.ColorUtility.TryParseHtmlString(colorValueOption, out Color newColor1))
		{
			optionColor = newColor1;
		}
		if (UnityEngine.ColorUtility.TryParseHtmlString(colorValueBck, out Color newColor2))
		{
			backgroundColor = newColor2;
		}
	}

	private void CreateIcon(GameObject menu, int numOptions, string name)
	{
		
		for (int i = 0; i < numOptions; i++)
		{
			double squareX, squareY;
			double angle = 2 * Math.PI * i / numOptions + Math.PI / numOptions;
			if (numOptions % 2 == 0)
			{
				if (numOptions % 4 == 0)
				{
					squareX = handMenuRadius * Math.Cos(angle);
					squareY = handMenuRadius * Math.Sin(angle);
				}
				else
				{
					squareX = handMenuRadius * Math.Cos(angle - Math.PI / numOptions);
					squareY = handMenuRadius * Math.Sin(angle - Math.PI / numOptions);
				}
			}
			else
			{
				squareX = handMenuRadius * Math.Sin(angle);
				squareY = handMenuRadius * Math.Cos(angle);
			}
			float adjustedX = (float)squareX;
			float adjustedY = (float)squareY;

			GameObject temp = Instantiate(icon, menu.transform);
			//icon.GetComponent<Image>().sprite = player.GetComponent<CreateOptions>().Icons.icons[name];
			temp.transform.localPosition = new Vector3(adjustedX, adjustedY, 0);
		}
	}
}
