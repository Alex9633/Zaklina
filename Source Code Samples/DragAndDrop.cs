using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 iniPos;
    private bool isDragging = false;

    public GameObject selected, dropArea, nextArrow;
    public Talk talk;
    public List<GameObject> resetCosmetics = new(), removeCosmetics = new();
    public string sound;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        iniPos = gameObject.GetComponent<RectTransform>().localPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
            {
                isDragging = true;
            }
        }

        if (isDragging)
        {
            Vector2 mousePosition = Input.mousePosition;
            rectTransform.position = mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;

                if (RectTransformUtility.RectangleContainsScreenPoint(dropArea.GetComponent<RectTransform>(), mousePosition) && dropArea.gameObject.activeSelf)
                {
                    if (selected != null) selected.SetActive(true);
                    if (nextArrow != null) nextArrow.SetActive(true);

                    foreach (GameObject g in resetCosmetics) g.SetActive(true);
                    foreach (GameObject g in removeCosmetics)
                    {
                        if(g.GetComponent<FadeInImage>() != null) g.GetComponent<FadeInImage>().MakeFadeOut();
                        else g.SetActive(false);
                    }

                    rectTransform.localPosition = iniPos;
                    if (talk != null) talk.MakeTalk(sound);
                    gameObject.SetActive(false);
                }
                else
                {
                    rectTransform.localPosition = iniPos;
                }
            }
        }
    }
}
