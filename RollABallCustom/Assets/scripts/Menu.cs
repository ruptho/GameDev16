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
        logAllPlayers();
    }

    public void AddPlayer()
    {
        // TODO: Add implementation
    }

    public void RemovePlayer(int playerID)
    {
        // TODO: Add implementation
    }

    public void logAllPlayers()
    {
        foreach (KeyValuePair<int, PlayerConfiguration> entry in allPlayers)
        {
            Debug.Log("-------------------------------");
            Debug.Log(entry.Value.toString());
        }
    }
}
