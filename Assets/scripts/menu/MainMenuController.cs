using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	public void QuitGame(){
		// Note: Not working in unity editor
		// as quiting the application would kill
		// unity. Only works on built application.
		Application.Quit();
	}
}
