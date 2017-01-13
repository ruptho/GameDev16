using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerMenuController : MonoBehaviour {



	//==================== START LEGACY CODE ====================

	public enum InputType { Keyboard, Controller };
	private class PlayerConfiguration
	{
		public int ID = 1;
		public string playerName = "";
		public bool isRobber = false;
		public InputType inputType = InputType.Keyboard;
		public KeyCode forward;
		public KeyCode backward;
		public KeyCode left;
		public KeyCode right;

		public PlayerConfiguration(int ID, string playerName, bool isRobber, InputType inputType, KeyCode forward, KeyCode backward, KeyCode left, KeyCode right)
		{
			this.ID = ID;
			this.playerName = playerName;
			this.isRobber = isRobber;
			this.inputType = inputType;
			this.forward = forward;
			this.backward = backward;
			this.left = left;
			this.right = right;
		}

		public string toString()
		{
			return "Name: " + playerName + "\nInputType: " + inputType;
		}
	}


	private Dictionary<int, PlayerConfiguration> allPlayers = new Dictionary<int, PlayerConfiguration>();
	private int playerCount = 2;



	/**
     *
     * Save all user made settings to the player preferences.
     * See https://docs.unity3d.com/ScriptReference/PlayerPrefs.html
     * 
     */
	private void SavePlayerSettings()
	{
		int numbRobbers = 0;
		int numbCops = 0;

		foreach (KeyValuePair<int, PlayerConfiguration> entry in allPlayers)
		{
			string currID = "R" + numbRobbers;

			if (!entry.Value.isRobber)
				currID = "C" + numbCops;

			PlayerPrefs.SetString("Forward" + currID, entry.Value.forward.ToString());
			PlayerPrefs.SetString("Backward" + currID, entry.Value.backward.ToString());
			PlayerPrefs.SetString("Left" + currID, entry.Value.left.ToString());
			PlayerPrefs.SetString("Right" + currID, entry.Value.right.ToString());

			if (entry.Value.isRobber)
				numbRobbers++;
			else
				numbCops++;
		}

		PlayerPrefs.SetInt("NumbRobbers", numbRobbers);
		PlayerPrefs.SetInt("NumbCops", numbCops);
		PlayerPrefs.Save();
	}



	void SetupGameLegacy(){
		// On awake, insert the first two (default) players
		PlayerConfiguration p1 = new PlayerConfiguration(1, "Default", true, InputType.Keyboard, KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
		allPlayers.Add(1, p1);
		PlayerConfiguration p2 = new PlayerConfiguration(1, "Default", false, InputType.Keyboard, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
		allPlayers.Add(2, p2);

		SavePlayerSettings ();
	}
		
	//==================== END LEGACY CODE ====================





	public void OnStartPrototype(){
		SetupGameLegacy (); //remove when implementing "real" networking

		SceneManager.LoadScene("minigame");
	}

	public void OnCreate(){

	}
}
