using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour {

	public InputField playerAliasInput;

	public const string KEY_PLAYER_ALIAS = "options.playerAlias";

	void Start () {
		if (PlayerPrefs.HasKey (KEY_PLAYER_ALIAS)) {
			playerAliasInput.text = PlayerPrefs.GetString (KEY_PLAYER_ALIAS);
		}
	}

	public void OnApply(){
		string alias = playerAliasInput.text;

		if (alias.Length > 0) {
			PlayerPrefs.SetString (KEY_PLAYER_ALIAS, alias);
		}
	}
}
