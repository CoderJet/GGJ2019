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
    public Image targetImage;

    public GameObject leftHandHolder;
    public GameObject rightHandHolder;

    public Sprite targetSprite;

    private GameObject currentSelection;

    private float defaultAlpha;

    // Start is called before the first frame update
    void Start()
    {
        JigsawPieceComponent[] jigsawPieces = GetComponentsInChildren<JigsawPieceComponent>();
        foreach (JigsawPieceComponent g in jigsawPieces)
        {
            GameObject image = new GameObject();

            Image imComp = image.AddComponent<Image>();
            imComp.sprite = targetSprite;

            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.parent = g.transform;
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);

            g.GetComponent<Selectable>().targetGraphic = imComp;
            defaultAlpha = g.GetComponent<Selectable>().colors.normalColor.a;

            if (UnityEngine.Random.value > 0.5f)
            {
                var transform = leftHandHolder.GetComponent<RectTransform>();

                var trans = g.GetComponent<RectTransform>();

                trans.localScale = new Vector3(1, 1, 1);
                Vector2 startPoint = transform.anchoredPosition;
                startPoint.x = Random.Range(transform.offsetMin.x + 150, transform.offsetMax.x - 150);
                startPoint.y = Random.Range(transform.offsetMin.y + 150, transform.offsetMax.y - 150);

                trans.anchoredPosition = startPoint;
                
                //rectTransform.anchorMin = new Vector2(0, 0);
                //rectTransform.anchorMax = new Vector2(1, 1);
                //rectTransform.pivot = new Vector2(0.5f, 0.5f);
                //rectTransform.offsetMin = new Vector2(0, 0);
                //rectTransform.offsetMax = new Vector2(0, 0);

                g.transform.parent = leftHandHolder.transform;
            }
            else
            {
                var transform = rightHandHolder.GetComponent<RectTransform>();

                var trans = g.GetComponent<RectTransform>();

                trans.localScale = new Vector3(1, 1, 1);
                Vector2 startPoint = transform.anchoredPosition;
                startPoint.x = Random.Range(transform.offsetMin.x + 150, transform.offsetMax.x - 150);
                startPoint.y = Random.Range(transform.offsetMin.y + 150, transform.offsetMax.y - 150);

                trans.anchoredPosition = startPoint;

                //rectTransform.anchorMin = new Vector2(0, 0);
                //rectTransform.anchorMax = new Vector2(1, 1);
                //rectTransform.pivot = new Vector2(0.5f, 0.5f);
                //rectTransform.offsetMin = new Vector2(0, 0);
                //rectTransform.offsetMax = new Vector2(0, 0);

                g.transform.parent = rightHandHolder.transform;
            }
        }

        if (targetImage != null)
            targetImage.sprite = targetSprite;
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
                handler.SetSelectedGameObject(list[UnityEngine.Random.Range(0, list.Length)].gameObject);
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
                    currentSelection.transform.SetSiblingIndex(canvas.transform.childCount-1);
                    currentSelection.transform.parent.SetSiblingIndex(currentSelection.transform.parent.GetSiblingIndex() + 1);
                    ColorBlock c = currentSelection.GetComponent<Selectable>().colors;
                    Color cn = c.normalColor;
                    cn.a = 1f;
                    c.normalColor = cn;
                    currentSelection.GetComponent<Selectable>().colors = c;
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
                DestroyImmediate(currentSelection.GetComponent<UnityEngine.UI.Selectable>());
                handler.enabled = true;
                currentSelection = null;

                // Recalulate neighbours.
                Selectable[] list = GetComponentsInChildren<Selectable>();
                for (int i = 0; i < list.Length; i++)
                {
                    if (list.Length < 2)
                        break;

                    if (i == 0)
                    {
                        // Special handling.
                        int neighbour_right = i + 1;
                        int neighbour_left = list.Length - 1;
                        var nav = list[i].navigation;
                        nav.selectOnLeft = list[neighbour_left];
                        nav.selectOnRight = list[neighbour_right];
                        list[i].navigation = nav;
                    }
                    else if (i == list.Length - 1)
                    {
                        // Special handling.
                        int neighbour_right = 0;
                        int neighbour_left = i - 1;
                        var nav = list[i].navigation;
                        nav.selectOnLeft = list[neighbour_left];
                        nav.selectOnRight = list[neighbour_right];
                        list[i].navigation = nav;
                    }
                    else
                    {
                        var nav = list[i].navigation;
                        nav.selectOnLeft = list[i - 1];
                        nav.selectOnRight = list[i + 1];
                        list[i].navigation = nav;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ColorBlock c = currentSelection.GetComponent<Selectable>().colors;
                Color cn = c.normalColor;
                cn.a = defaultAlpha;
                c.normalColor = cn;
                currentSelection.GetComponent<Selectable>().colors = c;

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
