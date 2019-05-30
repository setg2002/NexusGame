using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject[] OldDoors;

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

    private void Update()
    {
        if (Textbox.GetComponent<Translator>().turnOffTerminal == true)
        {
            ComBox.SetActive(false);
        }
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
                Player = GameObject.Find("Player1");

                if (ThisTerminalLevel == TerminalLevel.Tutorial)
                {
                    UnlockDoor();
                    StartCoroutine(ComAppear());
                    AlreadyUsed = true;
                }
                if (ThisTerminalLevel == TerminalLevel.Generator)
                {
                    PowerDoors();
                    StartCoroutine(ComAppear());
                    AlreadyUsed = true;
                }
                if (ThisTerminalLevel == TerminalLevel.Weapons)
                {
                    EndGamePower();
                    StartCoroutine(ComAppear());
                    AlreadyUsed = true;
                }
                if (ThisTerminalLevel == TerminalLevel.GeneratorActivation)
                {
                    TryPower();
                    StartCoroutine(ComAppear());
                    AlreadyUsed = true;
                }
                if (ThisTerminalLevel == TerminalLevel.Hydroponics && Player.GetComponent<PlayerCharacterController>().GameStage != 3)
                {
                    text = new string[1] { "Power must be restored in order to activate sprinkler system f" };
                    StartCoroutine(ComAppear());
                }
                else if (ThisTerminalLevel == TerminalLevel.Hydroponics && Player.GetComponent<PlayerCharacterController>().GameStage == 3)
                {
                    PowerHydro();
                    text = new string[1] { "Fire sprinklers activated f" };
                    StartCoroutine(ComAppear());
                    AlreadyUsed = true;
                }
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
        if (GlobalVariables.Fuse == false)
        {
            // Alien says go get fuse
            text = new string[1] { "Go replace the fuse in the fuse box f" };
        }
        if (GlobalVariables.Fuse == true)
        {
            Player = GameObject.Find("Player1");
            Player.GetComponent<PlayerCharacterController>().GameStage = 3;

            gameObject.GetComponent<Animator>().Play("Switch");

            text = new string[1] { "Power is now restored f" };
            ChangeDoors("powered", "unpowered", poweredPrefab);
            ChangeDoors("poweredFire", "unpoweredFire", poweredFirePrefab);
            HolodeckWarningChange(HoloDeckManager.WarningState.Fire);
            if(GameObject.Find("BackgroundUP")) { GameObject.Find("BackgroundUP").SetActive(false); }
        }
    }

    // Responsible for updating the doors when a terminal is activated
    private void ChangeDoors(string newTag, string oldTag, GameObject NewPrefab)
    {
        OldDoors = GameObject.FindGameObjectsWithTag(oldTag);

        foreach (GameObject item in OldDoors)
        {
            GameObject NewDoor = Instantiate(NewPrefab);
            NewDoor.transform.parent = GameObject.Find("TheAndromeda").transform.Find("Doors");
            NewDoor.GetComponent<DoorControls>().doorNum = item.GetComponent<DoorControls>().doorNum;
            
            if (oldTag == "Quarentine" || oldTag == "unpoweredFire") { NewDoorSpawnOffset = 0; }
            else { NewDoorSpawnOffset = 0.51f; }

            NewDoor.transform.position = item.transform.position + new Vector3(0, NewDoorSpawnOffset, 0);
            NewDoor.GetComponent<DoorControls>().ClosedPos.y = item.transform.position.y;

            Destroy(item);
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
            Textbox.GetComponent<Translator>().turnOffTerminal = false;
            source.Stop();
            if(Music)
            {
                source.PlayOneShot(Music, 1f);
            }
            ComBox.SetActive(true);
            yield return new WaitForSeconds(3);
            Textbox.GetComponent<Translator>().SplitAndEncrypt(text);
        }
    }
}
