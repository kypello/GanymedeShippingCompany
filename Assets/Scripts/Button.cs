using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool mouseOver;
    public Color defaultColor;
    public Color highlightColor;
    public TMP_Text text;

    public bool hasSprite;
    public Image sprite;

    void Update() {
        if (mouseOver) {
            text.color = highlightColor;
            if (hasSprite) {
                sprite.color = highlightColor;
            }
        }
        else {
            text.color = defaultColor;
            if (hasSprite) {
                sprite.color = defaultColor;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
    }
}
