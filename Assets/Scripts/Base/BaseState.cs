using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    [Header("Player Target")]
    public PlayerController player;
    public InputHandler playerInput;
    [Header("Cinemachine Virtual Cameras")]
    public Cinemachine.CinemachineVirtualCamera playerCamera;
    public Cinemachine.CinemachineVirtualCamera baseCamera;
    [Header("Player Distance Trigger")]
    public float baseDistanceTransition = 10f;
    [Header("Base Sprite Renderer")]
    public SpriteRenderer baseRenderer;
    [Header("Base Fade in/out speed")]
    public float fadeSpeed = 1f;
    [Header("Minigame Sine Wave Scene/Canvas Settings")]
    public GameObject sineWaveMinigameSpot;
    public SineWaveMiniGame sineGame;
    public GameObject sineWaveGlow;
    public Cinemachine.CinemachineVirtualCamera sineWaveCamera;
    [Header("Minigame Jigsaw Scene/Canvas Settings")]
    public GameObject jigsawMinigameSpot;
    public MouseMover jigsawGame;
    public GameObject jigsawGameGlow;
    public Cinemachine.CinemachineVirtualCamera jigsawCamera;
    [Header("Inventory")]
    public InventoryManager inventoryManager;

    private Transform playerTransform;
    public bool inBase = false;

    // Start is called before the first frame update
    void Start()
    {
        if (baseRenderer != null)
        {
            baseRenderer.gameObject.SetActive(true);
        }

        if (player == null)
            Debug.LogError("No Player Set - SET BEFORE CONTINUING!");

        playerTransform = player.GetComponent<Transform>();
    }

    public void AddItemToInventory(GameObject currentlyHeldObject)
    {
        inventoryManager.AddItemToInventory(currentlyHeldObject);
        currentlyHeldObject.SetActive(false);
        currentlyHeldObject.transform.parent = null;
        player.isHoldingObject = false;
        currentlyHeldObject = null;
    }

    // Update is called once per frame
    private void Update()
    {
        // Jigsaw Stuff
        {
            // Check if we're close enough for the animation to start playing.
            float dist = Vector3.Distance(jigsawMinigameSpot.transform.position, player.transform.position);

            if (dist < 1f)
            {
                if (jigsawGameGlow.GetComponent<Animator>().GetBool("PlayAnimation") == false)
                {
                    jigsawGameGlow.GetComponent<Animator>().SetBool("PlayAnimation", true);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    // Do the minigame logic.
                    jigsawCamera.Priority = 100;
                    playerInput.enabled = false;
                    jigsawGame.enabled = true;
                    player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                }
            }
            else
            {
                if (jigsawGameGlow.GetComponent<Animator>().GetBool("PlayAnimation") == true)
                {
                    jigsawGameGlow.GetComponent<Animator>().SetBool("PlayAnimation", false);
                }
            }

            if (jigsawGame.Completed)
            {
                // Lets go back and reset it.
                jigsawCamera.Priority = 0;
                playerInput.enabled = true;
                jigsawGame.enabled = false;
            }
        }

        // Sinewave Stuff
        {
            // Check if we're close enough for the animation to start playing.
            float dist = Vector3.Distance(sineWaveMinigameSpot.transform.position, player.transform.position);

            if (dist < 1f)
            {
                if (jigsawGameGlow.GetComponent<Animator>().GetBool("PlayAnimation") == false)
                {
                    jigsawGameGlow.GetComponent<Animator>().SetBool("PlayAnimation", true);
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    // Do the minigame logic.
                    StartCoroutine(FadePlayerOut());
                    sineWaveCamera.Priority = 100;
                    playerInput.enabled = false;
                    player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    sineGame.enabled = true;
                }
            }
            else
            {
                if (jigsawGameGlow.GetComponent<Animator>().GetBool("PlayAnimation") == true)
                {
                    jigsawGameGlow.GetComponent<Animator>().SetBool("PlayAnimation", false);
                }
            }

            if (sineGame.Completed)
            {
                // Lets go back and reset it.
                StartCoroutine(FadePlayerIn());
                sineWaveCamera.Priority = 0;
                playerInput.enabled = true;
                sineGame.enabled = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        inBase = true;
        TriggerCameraTransition();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        inBase = false;
        TriggerCameraTransition();
    }

    void TriggerCameraTransition()
    {
        if (inBase)
        {
            // Trigger some transitions.
            if (playerCamera.Priority == 3)
            {
                baseCamera.Priority = 3;
                playerCamera.Priority = 2;

                // Trigger a fade out of the front base?
                StartCoroutine(FadeBaseOut());
            }
        }
        else
        {
            if (baseCamera.Priority == 3)
            {
                baseCamera.Priority = 2;
                playerCamera.Priority = 3;

                // Trigger a fade in of the front base?
                StartCoroutine(FadeBaseIn());
            }
        }
    }

    IEnumerator FadeBaseOut()
    {
        Color c = baseRenderer.color;
        while (c.a > 0)
        {   
            c.a = c.a - (Time.deltaTime * fadeSpeed);
            baseRenderer.color = c;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator FadeBaseIn()
    {
        Color c = baseRenderer.color;
        while (c.a < 1f)
        {
            c.a = c.a + (Time.deltaTime * fadeSpeed);
            baseRenderer.color = c;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator FadePlayerOut()
    {
        var rend = player.GetComponent<SpriteRenderer>();
        Color c = rend.color;
        while (c.a > 0)
        {
            c.a = c.a - (Time.deltaTime * fadeSpeed);
            rend.color = c;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator FadePlayerIn()
    {
        var rend = player.GetComponent<SpriteRenderer>();
        Color c = rend.color;
        while (c.a < 1f)
        {
            c.a = c.a + (Time.deltaTime * fadeSpeed);
            rend.color = c;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
