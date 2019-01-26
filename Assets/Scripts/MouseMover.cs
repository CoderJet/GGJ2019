    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseMover : MonoBehaviour
{
    public RectTransform targetPositon;
    public EventSystem handler;
    public Canvas canvas;

    public Sprite targetImage;

    private GameObject currentSelection;

    // Start is called before the first frame update
    void Start()
    {
        JigsawPieceComponent[] jigsawPieces = GetComponentsInChildren<JigsawPieceComponent>();
        foreach (JigsawPieceComponent g in jigsawPieces)
        {
            GameObject image = new GameObject();
            image.AddComponent<RectTransform>();
            image.GetComponent<RectTransform>().parent = canvas.transform;            
            image.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
            Image imComp = image.AddComponent<Image>();
            imComp.sprite = targetImage;
            image.GetComponent<RectTransform>().parent = g.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSelection == null && handler.currentSelectedGameObject == null)
        {
            Selectable[] list = GetComponentsInChildren<Selectable>();
            if (list.Length == 0)
            {
                Debug.Log("We did it!");
            }
            else
            {
                handler.SetSelectedGameObject(list[0].gameObject);
            }
        }



        if (handler.enabled)
        {
            GameObject selection = handler.currentSelectedGameObject;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentSelection == null)
                {
                    currentSelection = selection;
                    handler.enabled = false;
                }
            }
        }

        if (currentSelection != null)
        {
            if (Vector2.Distance(currentSelection.GetComponent<RectTransform>().position, targetPositon.position) < 0.2f)
            {
                // we're there
                currentSelection.GetComponent<RectTransform>().position = targetPositon.position;
                Destroy(currentSelection.GetComponent<UnityEngine.UI.Selectable>());
                handler.enabled = true;
                currentSelection = null;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                handler.enabled = true;
                handler.SetSelectedGameObject(currentSelection);
                currentSelection = null;
            }
            else if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                currentSelection.transform.Translate(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            }
        }
    }
}
