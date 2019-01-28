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

    public JigsawSet[] JigsawSet;

    public GameObject leftHandHolder;
    public GameObject rightHandHolder;

    public Sprite targetSprite;

    private GameObject currentSelection;

    private float defaultAlpha;

    public bool Completed = false;

    private void OnEnable()
    {
        Completed = false;
        targetImage.enabled = true;

        //JigsawPieceComponent[] jigsawPieces = GetComponentsInChildren<JigsawPieceComponent>();
        var jigsawTarget = JigsawSet[Random.Range(0, JigsawSet.Length)];

        targetSprite = jigsawTarget.jigsawImage;
        Debug.Log("Jigsaw Length: " + jigsawTarget.jigsawPieces.Length);


        foreach (GameObject g in jigsawTarget.jigsawPieces)
        {
            if (g == null)
                continue;

            Debug.Log("Creating object: " + g);
            GameObject instantiated_obj = Instantiate(g, canvas.transform, false);
            Debug.Log("Created object: " + instantiated_obj);

            GameObject image = new GameObject();

            Image imComp = image.AddComponent<Image>();
            imComp.sprite = targetSprite;

            RectTransform rectTransform = image.GetComponent<RectTransform>();
            rectTransform.SetParent(instantiated_obj.transform);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.offsetMin = new Vector2(0, 0);
            rectTransform.offsetMax = new Vector2(0, 0);
            rectTransform.localRotation = Quaternion.identity;

            instantiated_obj.GetComponent<Selectable>().targetGraphic = imComp;
            defaultAlpha = instantiated_obj.GetComponent<Selectable>().colors.normalColor.a;

            if (UnityEngine.Random.value > 0.5f)
            {
                var transform = leftHandHolder.GetComponent<RectTransform>();

                var trans = instantiated_obj.GetComponent<RectTransform>();

                trans.localScale = new Vector3(1, 1, 1);
                Vector2 startPoint = transform.anchoredPosition;
                startPoint.x = Random.Range(transform.offsetMin.x + 150, transform.offsetMax.x - 150);
                startPoint.y = Random.Range(transform.offsetMin.y + 150, transform.offsetMax.y - 150);

                trans.anchoredPosition = startPoint;

                instantiated_obj.transform.SetParent(leftHandHolder.transform);
            }
            else
            {
                var transform = rightHandHolder.GetComponent<RectTransform>();

                var trans = instantiated_obj.GetComponent<RectTransform>();

                trans.localScale = new Vector3(1, 1, 1);
                Vector2 startPoint = transform.anchoredPosition;
                startPoint.x = Random.Range(transform.offsetMin.x + 150, transform.offsetMax.x - 150);
                startPoint.y = Random.Range(transform.offsetMin.y + 150, transform.offsetMax.y - 150);

                trans.anchoredPosition = startPoint;

                instantiated_obj.transform.SetParent(rightHandHolder.transform);
            }
        }

        if (targetImage != null)
            targetImage.sprite = targetSprite;
    }

    private void OnDisable()
    {
        targetImage.enabled = false;
        for (int i = 0; i < leftHandHolder.transform.childCount; i++)
        {
            Destroy(leftHandHolder.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < rightHandHolder.transform.childCount; i++)
        {
            Destroy(rightHandHolder.transform.GetChild(i).gameObject);
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
                Completed = true;
            }
            else
            {
                handler.SetSelectedGameObject(list[UnityEngine.Random.Range(0, list.Length)].gameObject);
            }
        }
        
        if (handler.enabled)
        {
            GameObject selection = handler.currentSelectedGameObject;
            if (Input.GetButtonDown("Fire1"))
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
            else if (Input.GetButtonDown("Cancel"))
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
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                currentSelection.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * 3f, Input.GetAxis("Vertical") * Time.deltaTime * 3f, 0);
            }
        }
    }
}
