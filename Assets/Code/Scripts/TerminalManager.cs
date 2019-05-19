using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject requiredItem;
    public GameObject Textbox;
    public GameObject ComBox;
    public AudioSource source;
    public AudioClip DoorClose;
    public AudioClip Music;
    public GameObject poweredPrefab;
    public GameObject poweredFirePrefab;
    public GameObject poweredQuarentinePrefab;

    private float NewDoorSpawnOffset = 0.51f;

    // All holo decks in the scene
    public GameObject[] Holodecks;


    // Terminal level determines what the terminal will do when activated
    public enum TerminalLevel { Tutorial, Generator, GeneratorActivation, Hydroponics, Weapons };
    public TerminalLevel ThisTerminalLevel;

    // Array for communication text
    public string[] text;

    // True if the player requires an object to activate the terminal
    public bool requiresObject;

    // To prevent the terminal (especially terminals that dont require an item) from being used multiple times
    public bool AlreadyUsed = false;





    private void Awake()
    {
        // Automatically sets requiresObject based on if there is a required item
        if (requiredItem) { requiresObject = true; }
        else { requiresObject = false; }
    }

    // Called from PlayerCharacterController.cs whe the player tries to interact with a terminal
    public void activateTerminal(string currentItem)
    {
        // Checks if the terminal has already been used
        if (!AlreadyUsed)
        {
            // For if the player can use the terminal it will execute the function according to its level
            if (requiresObject == false || requiredItem.name == currentItem)
            {
                if (ThisTerminalLevel == TerminalLevel.Tutorial) { UnlockDoor(); }
                if (ThisTerminalLevel == TerminalLevel.Generator) { PowerDoors(); }
                if (ThisTerminalLevel == TerminalLevel.Hydroponics) { PowerHydro(); }
                if (ThisTerminalLevel == TerminalLevel.Weapons) { EndGamePower(); }
                if (ThisTerminalLevel == TerminalLevel.GeneratorActivation) { TryPower(); }

                StartCoroutine(ComAppear());
            }

            // If the player cannot use the terminal, do nothing
            else { return; }
        }
    }

    // Turns locked doors (only in the tutorial) into powered doors
    public void UnlockDoor()
    {
        Player = GameObject.Find("Player1");
        Player.GetComponent<PlayerCharacterController>().GameStage = 1;

        ChangeDoors("powered", "Locked", poweredPrefab);
        HolodeckWarningChange(HoloDeckManager.WarningState.NoPower);
    }

    // Turns unpowered doors into powered doors
    public void PowerDoors()
    {
        Player = GameObject.Find("Player1");
        Player.GetComponent<PlayerCharacterController>().GameStage = 2;

        GlobalVariables.Fuse = true;
        AlreadyUsed = true;
    }

    // Turns powered fire doors into powered doors
    public void PowerHydro()
    {
        Player = GameObject.Find("Player1");
        Player.GetComponent<PlayerCharacterController>().GameStage = 4;

        ChangeDoors("powered", "poweredFire", poweredPrefab);
        HolodeckWarningChange(HoloDeckManager.WarningState.Quarantine);
    }

    // Makes quarentine doors usable
    public void EndGamePower()
    {
        Player = GameObject.Find("Player1");
        Player.GetComponent<PlayerCharacterController>().GameStage = 5;

        ChangeDoors("powered", "Quarentine", poweredQuarentinePrefab);
        HolodeckWarningChange(HoloDeckManager.WarningState.None);
    }

    public void TryPower()
    {
        Player = GameObject.Find("Player1");
        if (GlobalVariables.Fuse == false)
        {
            // Alien says go get fuse
            text = new string[4] { "Go f", "get f", "the f", "fuse f" };
        }
        if (GlobalVariables.Fuse == true)
        {
            Player.GetComponent<PlayerCharacterController>().GameStage = 3;

            text = new string[4] {"Power f", "is f", "now f", "restored f" };
            ChangeDoors("powered", "unpowered", poweredPrefab);
            ChangeDoors("poweredFire", "unpoweredFire", poweredFirePrefab);
            HolodeckWarningChange(HoloDeckManager.WarningState.Fire);
            GameObject.Find("BackgroundUP").SetActive(false);
        }
    }

    // Responsible for updating the doors when a terminal is activated
    private void ChangeDoors(string newTag, string oldTag, GameObject NewPrefab)
    {
        GameObject[] OldDoors = GameObject.FindGameObjectsWithTag(oldTag);
        foreach (GameObject item in OldDoors)
        {
            GameObject NewDoor = Instantiate(NewPrefab);
            if(oldTag == "Quarentine") { NewDoorSpawnOffset = 0; }
            else { NewDoorSpawnOffset = 0.51f; }
            if(newTag == "powered") { NewDoor.transform.position = item.transform.position + new Vector3(0, NewDoorSpawnOffset, 0); }
            else { NewDoor.transform.position = item.transform.position; }
            NewDoor.tag = newTag;
            Destroy(item);
            AlreadyUsed = true;
        }
    }

    // Changing all holo decks in scene
    private void HolodeckWarningChange(HoloDeckManager.WarningState warningState)
    {
        Holodecks = GameObject.FindGameObjectsWithTag("HoloDeck");
        foreach (GameObject item in Holodecks)
        {
            item.GetComponent<HoloDeckManager>().warningText.gameObject.SetActive(true);
            item.GetComponent<HoloDeckManager>().currentWarningState = warningState;
        }
    }

    // To make the communication box in the bottom left of the screen appear and do its thing
    IEnumerator ComAppear()
    {
        if(text.Length > 0)
        {
            source.Stop();
            ComBox.SetActive(true);
            source.PlayOneShot(Music, 1f);
            yield return new WaitForSeconds(3);
            Textbox.GetComponent<Translator>().SplitAndEncrypt(text);
            yield return new WaitForSeconds(20);
            ComBox.SetActive(false);
        }
    }
}
