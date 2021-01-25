using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerCharacterController : MonoBehaviour
{
    // Variables for movement inputs
    private float horizontal;
    private float inputDirection;
    private float verticalVelocity;

    // Variables for movement speeds
    public float runSpeed = 275f;
    public float jumpForce = 475f;
    private float gravity = 30f;
    private float lowGravity = 10f;

    // For X offset of held items
    private float itmXoffset;

    // Variables defining player collision boundaries for jumping
    private float distToGround;
    private float playerXBoundary;

    // The vector that tells the player object where to move
    private Vector2 moveVector;

    // Variables for components
    private Rigidbody2D rb2d;
    private Collider2D capCollider;

    // Raycasts used for left and right "foot" collisions with the floor
    private RaycastHit2D RaycastLeft;
    private RaycastHit2D RaycastRight;

    // Animation variables
    Animator ani;
    public AnimationClip jumpAnim;

    // Sound variables
    AudioSource Source;
    public AudioClip[] Footsteps;
    private bool StepFinished;

    // For roof collision, y velocity reset
    bool DoOnce;

    // True when player is standing over a terminal
    private bool bTerminal;

    // True when player can use an elevator
    private bool bElevator;

    // True when player is standing over an audio log
    private bool bAudioLog;

    // True when the jump animation is finished. This is to make animations play in the correct sequence
    private bool animationFinished;

    // True when the player has the black box item and determines the end credits that play
    public bool HasBlackBox;

    float time;

    // The number of audio logs the player has picked up for achievements
    public int logCount;

    // The number of the audio log the player is standing over, and is sent to AudioManager
    private int LogNum;

    // The part of the gamethe player is at for saving
    public int GameStage = 0;

    // Array for storing all items that can be picked up by the player
    public GameObject[] items = new GameObject[3];

    // The item the player currently has
    public GameObject currentItemGO;
    public string currentItem;

    // To set when the player is standing over a terminal
    GameObject Terminal;

    GameObject Elevator;

    public GameObject MasterTerminal;

    // The item the player must have to activate a terminal
    GameObject requiredItem;

    // The audio log to et when a player is standing over an audio log
    GameObject AudioLog;

    // The sound to play by a trigger
    AudioClip TriggerSound;





    // Sets values for player at start of game
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        PlayerData data = SaveSystem.LoadPlayer();

        if (data == null)
        {
            logCount = 0;
            GameStage = 0;
            HasBlackBox = false;
            currentItem = null;

            transform.position = new Vector2(-61.5f, -10);

            Debug.Log("Player variables were set to default");
        }
        else
        {
            logCount = data.logCount;
            GameStage = data.GameStage;
            HasBlackBox = data.HasBlackBox;
            currentItem = data.currentItem;

            gameObject.GetComponent<SpriteRenderer>().flipX = data.flipX;

            transform.position = new Vector2(data.position[0], data.position[1]);
        }


        rb2d = GetComponent<Rigidbody2D>();
        capCollider = GetComponent<Collider2D>();
        ani = GetComponent<Animator>();
        Source = GetComponent<AudioSource>();


        animationFinished = true;
        StepFinished = true;

        logCount = 0;

        distToGround = capCollider.bounds.extents.y;
        playerXBoundary = capCollider.bounds.extents.x;

        DoOnce = true;

        if (GameStage == 1) { MasterTerminal.GetComponent<TerminalManager>().UnlockDoor(); }
        if (GameStage == 2) { MasterTerminal.GetComponent<TerminalManager>().UnlockDoor(); MasterTerminal.GetComponent<TerminalManager>().PowerDoors(); }
        if (GameStage == 3) { MasterTerminal.GetComponent<TerminalManager>().UnlockDoor(); MasterTerminal.GetComponent<TerminalManager>().PowerDoors(); MasterTerminal.GetComponent<TerminalManager>().TryPower(); }
        if (GameStage == 4) { MasterTerminal.GetComponent<TerminalManager>().UnlockDoor(); MasterTerminal.GetComponent<TerminalManager>().PowerDoors(); MasterTerminal.GetComponent<TerminalManager>().TryPower(); MasterTerminal.GetComponent<TerminalManager>().PowerHydro(); }
        if (GameStage == 5) { MasterTerminal.GetComponent<TerminalManager>().UnlockDoor(); MasterTerminal.GetComponent<TerminalManager>().PowerDoors(); MasterTerminal.GetComponent<TerminalManager>().TryPower(); MasterTerminal.GetComponent<TerminalManager>().PowerHydro(); MasterTerminal.GetComponent<TerminalManager>().EndGamePower(); }
    }
    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        time += Time.deltaTime;

        if(!GameObject.Find("Canvas").GetComponentInChildren<PauseMenu>().GamePaused)
        {
            // Constantly checking for an E key press
            keyPressE();

            if(currentItem != null && currentItemGO == null)
            {
                currentItemGO = GameObject.Find(currentItem);
            }

            if (currentItemGO && currentItemGO.CompareTag("HeavyItm"))
            {
                currentItemGO.transform.position = gameObject.transform.position + new Vector3(0, distToGround - 0.8f, 0);
            }
            else if (currentItemGO && currentItemGO.CompareTag("Captn"))
            {
                currentItemGO.transform.position = gameObject.transform.position + new Vector3(0, distToGround - 1, 0);
            }
            else if(currentItemGO)
            {
                if (GetComponent<SpriteRenderer>().flipX == true) { itmXoffset = 0.15f; }
                else { itmXoffset = -0.15f; }
                currentItemGO.transform.position = gameObject.transform.position + new Vector3(itmXoffset, distToGround - 2.25f, 0);
                currentItemGO.transform.localScale = new Vector2(0.5f, 0.5f);
            }

            inputDirection = Input.GetAxis("Horizontal") * runSpeed;

            /*
            Setting the animation sequence

            So if you aren't on the ground and aren't jumping
                then you're falling
            Otherwise if you're moving and on the ground
                you're running
            If everything else fails
                you're idle

            Jumping overrides all other animations
            */

            if (!IsGrounded() && animationFinished)
            {
                if (currentItemGO && (currentItemGO.CompareTag("HeavyItm") || currentItemGO.CompareTag("Captn")))
                {
                    ani.Play("HeavyItemFall");
                }
                else
                {
                    ani.Play("Fall");
                }
            }
            else if (inputDirection != 0 && IsGrounded() && animationFinished)
            {
                if (currentItemGO && (currentItemGO.CompareTag("HeavyItm") || currentItemGO.CompareTag("Captn")))
                {
                    ani.Play("HeavyItemWalk");
                    if (currentItemGO.GetComponent<Animator>())
                    {
                        currentItemGO.GetComponent<Animator>().Play("CaptainAnimation");
                    }
                }
                else
                {
                    ani.Play("PlayerJog");
                }
            }
            else if (animationFinished)
            {
                if (currentItemGO && (currentItemGO.CompareTag("HeavyItm") || currentItemGO.CompareTag("Captn")))
                {
                    ani.Play("HeavyItemIdle");
                }
                else
                {
                    ani.Play("Idle");
                }
            }
            if (inputDirection == 0 && currentItemGO && currentItemGO.GetComponent<Animator>())
            {
                currentItemGO.GetComponent<Animator>().Play("None");
            }
            if (inputDirection != 0 && IsGrounded())
            {
                StartCoroutine(WalkAudio());
            }
            if (!IsGrounded() && currentItemGO && currentItemGO.GetComponent<Animator>())
            {
                currentItemGO.GetComponent<Animator>().Play("None");
            }


            // Logic for jumping
            if (IsGrounded() && animationFinished)
            {
                verticalVelocity = 0;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    verticalVelocity = jumpForce;

                    if (currentItemGO && (currentItemGO.CompareTag("HeavyItm") || currentItemGO.CompareTag("Captn")))
                    {
                        ani.Play("HeavyItemJump");
                    }
                    else
                    {
                        ani.Play("Jump");
                    }
                    StartCoroutine(FinishAnimation(jumpAnim));
                    animationFinished = false;
                }
            }

            // If you aren't on the ground, a contstant negative vertical velocity is applied
            if (!IsGrounded())
            {
                if (TouchingRoof())
                {
                    if (DoOnce)
                    {
                        verticalVelocity = -gravity;
                        DoOnce = false;
                    }
                    verticalVelocity -= gravity;
                }
                if (Input.GetKey(KeyCode.Space) && rb2d.velocity.y > 0)
                {
                    verticalVelocity -= lowGravity + Time.deltaTime;
                }
                else
                {
                    verticalVelocity -= gravity;
                    DoOnce = true;
                }
                verticalVelocity = Mathf.Clamp(verticalVelocity, -1000, 1000);
            }


            // Changes the direction the character is facing based on input
            if (inputDirection < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (currentItemGO && currentItemGO.CompareTag("Captn"))
                {
                    currentItemGO.GetComponent<SpriteRenderer>().flipY = true;
                }
            }
            else if (inputDirection > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (currentItemGO && currentItemGO.CompareTag("Captn"))
                {
                    currentItemGO.GetComponent<SpriteRenderer>().flipY = false;
                }
            }


            // Moving the player
            moveVector = new Vector2(inputDirection, verticalVelocity);
            rb2d.velocity = (moveVector * Time.deltaTime);
        }

        // Update UI
        GameObject.Find("Canvas").GetComponent<UIManager>().updateItem(currentItem);

        //Debug.Log();
    }


    // Detcting trigger collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision check for items
        if (!currentItemGO)
        {
            // If the player doesn't have an item, then it will loop through all items in the "items" array.
            // And if the trigger you have entered is one of those items, the player will pick it up and update the UI
            foreach (GameObject item in items)
            {
                if (other.CompareTag("Captn"))
                {
                    currentItemGO = other.gameObject;
                    currentItem = other.gameObject.name;
                    return;
                }
                // If the item is the black box
                if(other.gameObject.CompareTag("BlackBox"))
                {
                    HasBlackBox = true;
                    other.gameObject.SetActive(false);
                    return;
                }
                // All other items
                if (other.gameObject == item)
                {
                    // For large items
                    if(other.CompareTag("HeavyItm"))
                    {
                        currentItemGO = other.gameObject;
                        currentItem = other.gameObject.name;
                    }
                    // For small items
                    else
                    {
                        currentItemGO = other.gameObject;
                        currentItem = other.gameObject.name;
                    }
                }
            }
        }


        // Collision check for terminals
        if (other.CompareTag("Terminal"))
        {
            Terminal = other.gameObject;

            // Setting the required item
            requiredItem = Terminal.GetComponent<TerminalManager>().requiredItem;

            bTerminal = true;

            if(Terminal.GetComponent<TerminalManager>().AlreadyUsed == true)
            {
                return;
            }
            if(Terminal.GetComponent<TerminalManager>().AlreadyUsed == false && Terminal.GetComponent<TerminalManager>().requiresObject)
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().displayRequiredItem(requiredItem, currentItem);
            }
            else
            {
                GameObject.Find("Canvas").GetComponent<UIManager>().displayRequiredItem("");
            }
        }

        if(other.CompareTag("Elevator"))
        {
            Elevator = other.gameObject;
            bElevator = true;
            GameObject.Find("Canvas").GetComponent<UIManager>().displayRequiredItem("");
        }

        // Collision check for audio logs
        if (other.CompareTag("TextLog"))
        {
            GameObject.Find("Canvas").GetComponent<UIManager>().displayAudioLog();
            LogNum = other.GetComponent<TextLog>().LogNum;
            bAudioLog = true;
            AudioLog = other.gameObject;
        }

        // Collision check for audio triggers
        if (other.CompareTag("AudioTrigger"))
        {
            TriggerSound = other.GetComponent<AudioTrigger>().TriggerSound;
            GameObject.Find("AudioManager").GetComponent<AudioManager>().TriggerSound(TriggerSound);
        }

        // Set current room for boundary manager
        if(other.CompareTag("Bound") || other.CompareTag("BoundLrg"))
        {
            other.GetComponent<Boundary>().through = true;
            GameObject.Find("Main_Camera").GetComponent<MetroidCameraController>().currentBoundary = other.gameObject;
            GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().UpdateBoundary(other.gameObject, null);
            GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().currentBoundary = other.gameObject.transform;
        }

        // For impossible jump achievement
        if(other.CompareTag("ImpossibleJump"))
        {
            GetComponent<AchievementManager>().AchievementGet(AchievementManager.Achievements.ImpossibleJump);
        }
    }

    // Checking for trigger stay on audio logs to fix issue where the UI wouldn't display audio log 
    // pickup text while the player was standing over an audio log
    private void OnTriggerStay2D(Collider2D other)
    {
        // Set current room for boundary manager
        if (other.CompareTag("Bound") || other.CompareTag("BoundLrg"))
        {
            GameObject.Find("Main_Camera").GetComponent<MetroidCameraController>().currentBoundary = other.gameObject;
            GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().UpdateBoundary(other.gameObject, null);
            GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().currentBoundary = other.gameObject.transform;
        }
    }

    // To get rid of UI elements when you leave the trigger of an audio log and terminal
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Terminal") || other.gameObject.CompareTag("Elevator")) { GameObject.Find("Canvas").GetComponent<UIManager>().hideRequiredItem(); }
        if (other.gameObject.CompareTag("TextLog")) { GameObject.Find("Canvas").GetComponent<UIManager>().hideAudioLog(); }
        bAudioLog = false;
        bTerminal = false;
        bElevator = false;

        if(other.gameObject.CompareTag("Bound") || other.gameObject.CompareTag("BoundLrg"))
        {
            GameObject.Find("MinimapCamera").GetComponent<MinimapCameraController>().UpdateBoundary(null, other.gameObject);
        }
    }


    // For E key interaction
    private void keyPressE()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // For interacting with terminals
            if (bTerminal && (GameObject.Find("Canvas").transform.Find("CommunicationBox").gameObject.activeSelf == false))
            {
                if (Terminal.GetComponent<TerminalManager>().requiresObject && currentItem == Terminal.GetComponent<TerminalManager>().requiredItem.name)
                {
                    GameObject.Find("Canvas").transform.Find("CommunicationBox").GetComponentInChildren<Translator>().text.text = "";
                    if (currentItemGO)
                    {
                        currentItemGO.SetActive(false);
                    }
                    Terminal.GetComponent<TerminalManager>().activateTerminal(currentItem);
                    currentItemGO = null;
                    currentItem = null;
                    GameObject.Find("Canvas").GetComponent<UIManager>().updateItem(currentItem);
                    GameObject.Find("Canvas").GetComponent<UIManager>().hideRequiredItem();
                    GameObject.Find("AudioManager").GetComponent<AudioManager>().TriggerSound(TriggerSound);
                }
                else if (currentItem == null && Terminal.GetComponent<TerminalManager>().requiresObject == false)
                {
                    GameObject.Find("Canvas").transform.Find("CommunicationBox").GetComponentInChildren<Translator>().text.text = "";
                    Terminal.GetComponent<TerminalManager>().activateTerminal(null);
                    GameObject.Find("Canvas").GetComponent<UIManager>().updateItem(null);
                    GameObject.Find("Canvas").GetComponent<UIManager>().hideRequiredItem();
                }
            }

            // For picking up audio logs
            if (bAudioLog && !GameObject.Find("Canvas").transform.Find("CommunicationBox").gameObject.activeSelf)
            {
                AudioLog.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                AudioLog.gameObject.GetComponent<Collider2D>().enabled = false;
                GameObject.Find("Canvas").transform.Find("CommunicationBox").GetComponentInChildren<Translator>().text.text = "";
                AudioLog.GetComponent<TextLog>().StartCoroutine("ComAppear");
                GameObject.Find("Canvas").GetComponent<UIManager>().audioText.gameObject.SetActive(false);

                logCount++;

                bAudioLog = false;
            }

            // For using elevators
            if(bElevator)
            {
                bElevator = false;
                Elevator.GetComponent<Elevator>().UseElevator(gameObject, GameObject.Find("Main_Camera"));
                GameObject.Find("Canvas").GetComponent<UIManager>().hideRequiredItem();
            }
        }
    }

    // Waiting for an animation to finish
    private IEnumerator FinishAnimation(AnimationClip animToFinish)
    {
        yield return new WaitForSeconds(animToFinish.length - Time.deltaTime);
        animationFinished = true;
    }

    // Handles footstep sounds
    private IEnumerator WalkAudio()
    {
        if(StepFinished == true)
        {
            StepFinished = false;

            AudioClip RndmFootstep = Footsteps[Random.Range(0, Footsteps.Length)];

            Source.pitch = Random.Range(0.6f, 0.8f);

            Source.PlayOneShot(RndmFootstep, Random.Range(0.2f, 0.7f));

            yield return new WaitForSeconds(.2f);

            StepFinished = true;
        }
    }


    // For grounding checks
    private bool IsGrounded()
    {
        // Debug rays \\
        Debug.DrawRay(transform.position + new Vector3(-playerXBoundary, -distToGround - 1, 0), Vector2.down, Color.red);
        Debug.DrawRay(transform.position + new Vector3(playerXBoundary, -distToGround - 1, 0), Vector2.down, Color.red);


        // There are two raycasts that come out of the bottom left and right of the players collision to detect grounding
        // This had to be done to prevent the player from getting stuck on the corner of a block
        RaycastLeft = Physics2D.Raycast(transform.position + new Vector3(-playerXBoundary, -distToGround - 0.95f, 0), Vector2.down, 0.07f);
        RaycastRight = Physics2D.Raycast(transform.position + new Vector3(playerXBoundary, -distToGround - 0.95f, 0), Vector2.down, 0.07f);

        // If at least one "foot" is touching the ground then the player is grounded
        if (RaycastLeft == false)
        {
            if (RaycastRight == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }


    // For checking if the player is hitting the roof
    private bool TouchingRoof()
    {
        // Debug ray \\
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 1), Vector2.up, Color.red);

        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1), Vector2.up, distToGround + 0.1f);
    }
        
}
