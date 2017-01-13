using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class ValidatePlayerName : MonoBehaviour {

	private InputField inputField;

	/**
	 * Button, which should be deactivated on failed validation and activated on successful.
	 * */
	public Button successButton;

	public string allowedCharsRegex = @"[a-zA-Z0-9_\-.]";
	public string allowedStringRegex = @".*";

	/**
	 * Minimal length of string
	 * */
	public int minLength = 0;
	/**
	 * Maximal length of string or -1 if you don't care
	 * */
	public int maxLength = -1;

	private Regex charRegex;
	private Regex stringRegex;

	public void Start()
	{
		//get inputfield this script is attached to
		inputField = FindObjectOfType<InputField> ();

		charRegex = new Regex (allowedCharsRegex);
		stringRegex = new Regex (allowedStringRegex);

		inputField.onValidateInput += delegate(string input, int charIndex, char addedChar) { return ValidateAlias( input, addedChar ); };
		inputField.onValueChange.AddListener (delegate {OnChange ();});
		successButton.interactable = false;
	}

	private char ValidateAlias(string input, char added){
		//if it's a match, add char to text. else return \0.
		if (charRegex.IsMatch(char.ToString(added))) {
			return added;
		} else {
			return '\0';
		}
	}

	public void OnChange()
	{
		int length = inputField.text.Trim ().Length;
		if ((minLength <= length) && (maxLength >= length || maxLength == -1) && stringRegex.IsMatch(inputField.text)) {
			successButton.interactable = true;
		} else {
			successButton.interactable = false;
		}
	}

}
