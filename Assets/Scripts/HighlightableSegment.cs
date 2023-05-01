using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public abstract class HighlightableSegment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver;
    public TMP_Text textBox;
    public bool highlightActive = false;
    public Document parentDocument;
    public string documentDataFieldKey;
    
    public string text;

    void Update() {
        if (highlightActive) {
            textBox.text = "<mark=#FFFFFF50>" + text;

            if (Input.GetMouseButtonDown(0)) {
                GetClicked();
            }
        }
        else {
            textBox.text = text;
        }

        highlightActive = false;
    }

    public abstract void GetClicked();

    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
    }
}
