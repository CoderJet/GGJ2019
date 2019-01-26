using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    [Header("Player Target")]
    public PlayerController player;
    [Header("Cinemachine Virtual Cameras")]
    public Cinemachine.CinemachineVirtualCamera playerCamera;
    public Cinemachine.CinemachineVirtualCamera baseCamera;
    [Header("Player Distance Trigger")]
    public float baseDistanceTransition = 10f;
    [Header("Base Sprite Renderer")]
    public SpriteRenderer baseRenderer;
    [Header("Base Fade in/out speed")]
    public float fadeSpeed = 1f;
    [Header("Minigame Scene/Canvas Settings")]
    public GameObject sineWaveMinigameSpot;
    public GameObject sineGame;
    public GameObject jigsawMinigameSpot;
    public GameObject jigsawGame;
    
    private Transform playerTransform;
    [SerializeField]
    private bool inBase = false;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            Debug.LogError("No Player Set - SET BEFORE CONTINUING!");

        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //inBase = (Vector2.Distance(transform.position, playerTransform.position) < baseDistanceTransition);

        //TriggerCameraTransition();
        
        // We can do some other stuff with minigames in here possibly?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inBase = true;
        TriggerCameraTransition();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
}
