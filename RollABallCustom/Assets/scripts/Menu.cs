using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    private enum InputType { Keyboard, Controller };
    private GameObject mainMenuHolder;
    private GameObject optionsMenuHolder;
    private Dictionary<int, PlayerConfiguration> allPlayers = new Dictionary<int, PlayerConfiguration>();
    private int playerCount = 2;
    private const int MAX_PLAYERS = 4;
    private float configHeight = 70.0f;
    /**
     * Container class used to represent a player
     * configuration based on the settings menu
     */
    private class PlayerConfiguration
    {
        public int ID = 1;
        public string playerName = "";
        public bool isRobber = false;
        public InputType inputType = InputType.Keyboard;

        public PlayerConfiguration(int ID, string playerName, bool isRobber, InputType inputType)
        {
            this.ID = ID;
            this.playerName = playerName;
            this.isRobber = isRobber;
            this.inputType = inputType;
        }

        public string toString()
        {
            return "Name: " + playerName + "\nInputType: " + inputType;
        }
    }

    void Awake()
    {
        // On awake, insert the first two (default) players
        PlayerConfiguration p1 = new PlayerConfiguration(1, "Default", true, InputType.Keyboard);
        allPlayers.Add(1, p1);
        PlayerConfiguration p2 = new PlayerConfiguration(1, "Default", false, InputType.Keyboard);
        allPlayers.Add(2, p2);

        configHeight = (float)Screen.height / 10.0f + 25.0f;
        UpdateSettingsCanvasColors();
    }

    public void Play()
    {
        SceneManager.LoadScene("minigame");
    }

    public void Quit()
    {
        // Note: Not working in unity editor
        // as quiting the application would kill
        // unity. Only works on built application.
        Application.Quit();
    }

    public void Options()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void ApplyChanges()
    {
        // Iterate all players and save the changes according
        // to the settings in the PanelControlSelectionP*
        for (int playerCounter = 1; playerCounter <= allPlayers.Count; playerCounter++)
        {
            GameObject settingsCurrPlayer = GameObject.Find("PanelControlSelectionP" + playerCounter);

            InputField playerNameInput = settingsCurrPlayer.GetComponentInChildren<InputField>();
            allPlayers[playerCounter].playerName = playerNameInput.text;

            Dropdown inputSelectionDropdown = settingsCurrPlayer.GetComponentInChildren<Dropdown>();
            allPlayers[playerCounter].inputType = (InputType)inputSelectionDropdown.value;

            Toggle isRobber = settingsCurrPlayer.GetComponentInChildren<Toggle>();
            allPlayers[playerCounter].isRobber = isRobber.isOn;
        }
        LogAllPlayers();
    }

    /**
     * Add a new player configuration to the options menu. 
     * 
     * Instantiates the playerControlSelection prefab and offsets
     * its position as well as the position of the addPlayer button.
     */
    public void AddPlayer()
    {
        if (playerCount < MAX_PLAYERS)
        {
            GameObject newPlayerControl = (GameObject)Instantiate(Resources.Load("playerControlSelection"));
            Transform newPlayerTransform = newPlayerControl.transform;
            newPlayerTransform.SetParent(GameObject.Find("OptionsMenu").transform, false);
            newPlayerTransform.position = new Vector3(newPlayerTransform.position.x, newPlayerTransform.position.y - configHeight * playerCount, newPlayerTransform.position.z);
            newPlayerControl.name = "PanelControlSelectionP" + ++playerCount;

            Button removeNewPlayerButton = newPlayerControl.GetComponentInChildren<Button>();
            removeNewPlayerButton.onClick.AddListener(delegate {RemovePlayer(playerCount);});

            Text playerConfigHeadline = newPlayerControl.transform.FindChild("SettingsHeadlineText").GetComponent<Text>();
            playerConfigHeadline.text = "Settings Player " + playerCount + ":";

            Toggle isRobber = newPlayerControl.GetComponentInChildren<Toggle>();
            isRobber.onValueChanged.AddListener(delegate { UpdateSettingsCanvasColors(); });

            PlayerConfiguration newPlayerConfig = new PlayerConfiguration(playerCount, "Default", true, InputType.Keyboard);
            allPlayers.Add(playerCount, newPlayerConfig);

            GameObject addPlayerButton = GameObject.Find("ButtonAddPlayer");
            addPlayerButton.transform.position = new Vector3(addPlayerButton.transform.position.x, addPlayerButton.transform.position.y - configHeight, addPlayerButton.transform.position.z);
        }
    }

    /**
     * Removes an existing player from the options menu.
     *
     * The player is also removed from the allPlayers dictionary.
     * AddPlayer button is positioned correctly afterwards.
     */
    public void RemovePlayer(int playerID)
    {
        if (playerCount > 2 && playerID > 2)
        {
            Destroy(GameObject.Find("PanelControlSelectionP" + playerID));
            allPlayers.Remove(playerID);
            playerCount--;

            GameObject addPlayerButton = GameObject.Find("ButtonAddPlayer");
            addPlayerButton.transform.position = new Vector3(addPlayerButton.transform.position.x, addPlayerButton.transform.position.y + configHeight, addPlayerButton.transform.position.z);
        }
    }

    /**
     *
     * Iterates all players and changes the color of the 
     * settings panel according to the isRobber flag.
     * 
     */
    public void UpdateSettingsCanvasColors()
    {
        for (int playerCounter = 1; playerCounter <= allPlayers.Count; playerCounter++)
        {
            GameObject settingsCurrPlayer = GameObject.Find("PanelControlSelectionP" + playerCounter);
            Toggle isRobber = settingsCurrPlayer.GetComponentInChildren<Toggle>();
            Image img = GameObject.Find("PanelControlSelectionP" + playerCounter).GetComponent<Image>();

            if (isRobber.isOn)
                img.color = new Color(213.0f / 255.0f, 19.0f / 255.0f, 19.0f / 255.0f,100.0f / 255.0f);
            else
                img.color = new Color(19.0f / 255.0f, 125.0f / 255.0f, 231.0f / 255.0f, 100.0f / 255.0f);
        }
    }

    public void LogAllPlayers()
    {
        foreach (KeyValuePair<int, PlayerConfiguration> entry in allPlayers)
        {
            Debug.Log("-------------------------------");
            Debug.Log(entry.Value.toString());
        }
    }
}
