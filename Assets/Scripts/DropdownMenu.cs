using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownMenu : MonoBehaviour
{
    public static DropdownMenu instance;

    const float lineHeight = 60f;
    const float characterWidth = 29f;
    public RectTransform canvasRectTransform;
    public Canvas canvas;

    const float rightBound = 2200f;
    const float leftBound = 360f;
    const float bottomBound = 24f;
    //const float leftBound = 0f;
    //const float rightBound = 5000f;
    //const float bottomBound = -5000f;

    RectTransform rectTransform;

    public AudioSource confirmSound;

    int itemCount;
    public List<TMP_Text> itemSlots = new List<TMP_Text>();
    List<MouseOver> itemSlotMouseOvers = new List<MouseOver>();

    bool displaying = false;
    MouseOver mouseOver;

    int currentlyHighlighted = -1;
    const string highlightTag = "<mark=#FFFFFF11>";

    public RectTransform panel;

    DropdownHighlightableSegment highlightableSegment;

    void Awake() {
        instance = this;

        rectTransform = GetComponent<RectTransform>();
        mouseOver = GetComponent<MouseOver>();
        foreach (TMP_Text itemSlot in itemSlots) {
            itemSlotMouseOvers.Add(itemSlot.GetComponent<MouseOver>());
        }

        for (int i = 0; i < itemSlots.Count; i++) {
            itemSlots[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(12f, -12f - i * lineHeight);
        }

        gameObject.SetActive(false);
    }

    public bool IsActive() {
        return displaying;
    }

    public void Display(string[] items, Vector2 position, DropdownHighlightableSegment client) {
        highlightableSegment = client;

        //position = new Vector2(position.x / Screen.width * canvasRectTransform.sizeDelta.x, position.y / Screen.height * canvasRectTransform.sizeDelta.x);
        position /= canvas.scaleFactor;

        gameObject.SetActive(true);
        int longestLineLength = 0;

        int i = 0;
        while (i < items.Length) {
            itemSlots[i].gameObject.SetActive(true);
            itemSlots[i].text = items[i];

            int lineLength = items[i].Length;
            if (lineLength > longestLineLength) {
                longestLineLength = lineLength;
            }

            i++;
        }

        while (i < itemSlots.Count) {
            itemSlots[i].gameObject.SetActive(false);
            i++;
        }

        for (int j = 0; j < items.Length; j++) {
            itemSlots[j].GetComponent<RectTransform>().sizeDelta = new Vector2(longestLineLength * characterWidth, 70f);
        }

        foreach (MouseOver itemMouseOver in itemSlotMouseOvers) {
            itemMouseOver.mouseOver = false;
        }
        mouseOver.mouseOver = true;

        rectTransform.sizeDelta = new Vector2(longestLineLength * characterWidth + 74f, 70f);
        panel.sizeDelta = new Vector2(longestLineLength * characterWidth + 50f, lineHeight * items.Length + 24f);

        if (position.x - rectTransform.sizeDelta.x / 2f < leftBound / canvas.scaleFactor) {
            position.x = leftBound / canvas.scaleFactor + rectTransform.sizeDelta.x / 2f;
        }
        if (position.x + rectTransform.sizeDelta.x / 2f > rightBound / canvas.scaleFactor) {
            position.x = rightBound / canvas.scaleFactor - rectTransform.sizeDelta.x / 2f;
        }
        if (position.y - rectTransform.sizeDelta.y / 2f - panel.sizeDelta.y < bottomBound / canvas.scaleFactor) {
            panel.pivot = new Vector2(0, 0);
            panel.anchoredPosition = new Vector2(12f, 70f);
        }
        else {
            panel.pivot = new Vector2(0, 1);
            panel.anchoredPosition = new Vector2(12f, 0f);
        }
        rectTransform.anchoredPosition = position;

        itemCount = items.Length;
        currentlyHighlighted = -1;
        displaying = true;
    }

    void Update() {
        Debug.Log(canvas.scaleFactor);

        if (displaying) {
            if (!mouseOver.mouseOver) {
                displaying = false;
                gameObject.SetActive(false);
                return;
            }

            for (int i = 0; i < itemCount; i++) {
                if (itemSlotMouseOvers[i].mouseOver) {
                    if (currentlyHighlighted != i) {
                        if (currentlyHighlighted != -1) {
                            itemSlots[currentlyHighlighted].text = itemSlots[currentlyHighlighted].text.Remove(0, 16);
                        }
                        itemSlots[i].text = itemSlots[i].text.Insert(0, highlightTag);
                        currentlyHighlighted = i;
                    }

                    if (Input.GetMouseButtonDown(0)) {
                        confirmSound.Play();
                        highlightableSegment.ReceiveDropdownInput(itemSlots[currentlyHighlighted].text.Remove(0, 16));

                        displaying = false;
                        gameObject.SetActive(false);
                        return;
                    }
                }
                else {
                    if (currentlyHighlighted == i) {
                        itemSlots[i].text = itemSlots[i].text.Remove(0, 16);
                        currentlyHighlighted = -1;
                    }
                }
            }
        }
    }
}
