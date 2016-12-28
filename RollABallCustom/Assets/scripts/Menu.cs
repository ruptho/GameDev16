using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public enum InputType { Keyboard, Controller };
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public Dictionary<int, PlayerConfiguration> allPlayers = new Dictionary<int, PlayerConfiguration>();
    private int playerCount = 1;
    private const int MAX_PLAYERS = 4;
    private float configHeight = 70.0f;
    /**
     * Container class used to represent a player
     * configuration based on the settings menu
     */
    public class PlayerConfiguration
    {
        private int ID = 1;
        private string playerName = "";
        private InputType controlType = InputType.Keyboard;

        public PlayerConfiguration(int ID, string playerName, InputType controlType)
        {
            this.ID = ID;
            this.playerName = playerName;
            this.controlType =controlType;
        }


        public int getID()
        {
            return ID;
        }

        public void setID(int ID)
        {
            this.ID = ID;
        }

        public string getPlayerName()
        {
            return playerName;
        }

        public void setPlayerName(string playerName)
        {
            this.playerName = playerName;
        }

        public InputType getInputType()
        {
            return controlType;
        }

        public void setInputType(InputType controlType)
        {
            this.controlType = controlType;
        }

        public string toString()
        {
            return "Name: " + playerName + "\nInputType: " + controlType;
        }
    }

    void Awake()
    {
        // On awake, insert the first (default) player
        PlayerConfiguration p1 = new PlayerConfiguration(1, "Default", InputType.Keyboard);
        allPlayers.Add(1, p1);
        configHeight = (float)Screen.height / 10.0f;
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
            allPlayers[playerCounter].setPlayerName(playerNameInput.text);

            Dropdown inputSelectionDropdown = settingsCurrPlayer.GetComponentInChildren<Dropdown>();
            allPlayers[playerCounter].setInputType((InputType)inputSelectionDropdown.value);
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

            PlayerConfiguration newPlayerConfig = new PlayerConfiguration(playerCount, "Default", InputType.Keyboard);
            allPlayers.Add(playerCount, newPlayerConfig);

            GameObject addPlayerButton = GameObject.Find("ButtonAddPlayer");
            addPlayerButton.transform.position = new Vector3(addPlayerButton.transform.position.x, addPlayerButton.transform.position.y - configHeight, addPlayerButton.transform.position.z);
        }
        LogAllPlayers();
    }

    /**
     * Removes an existing player from the options menu.
     *
     * The player is also removed from the allPlayers dictionary.
     * AddPlayer button is positioned correctly afterwards.
     */
    public void RemovePlayer(int playerID)
    {
        if (playerCount > 1)
        {
            Destroy(GameObject.Find("PanelControlSelectionP" + playerID));
            allPlayers.Remove(playerID);
            playerCount--;

            GameObject addPlayerButton = GameObject.Find("ButtonAddPlayer");
            addPlayerButton.transform.position = new Vector3(addPlayerButton.transform.position.x, addPlayerButton.transform.position.y + configHeight, addPlayerButton.transform.position.z);
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
