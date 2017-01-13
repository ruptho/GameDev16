using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject serverMenu;


	void Start()
	{
		ShowMainMenu ();
	}

	/**
	 * Method to hide all menus, for easier extension (hide first, show new) 
	 */
	void HideAll(){
		mainMenu.SetActive (false);
		optionsMenu.SetActive (false);
		serverMenu.SetActive (false);
	}

	public void ShowMainMenu(){
		HideAll ();
		mainMenu.SetActive (true);
	}

	public void ShowOptionsMenu(){
		HideAll ();
		optionsMenu.SetActive (true);
	}

	public void ShowServerMenu(){
		HideAll ();
		serverMenu.SetActive (true);
	}
}
