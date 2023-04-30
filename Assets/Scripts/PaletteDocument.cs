using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaletteDocument : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Document documentPrefab;
    public RectTransform rectTransform;

    public bool mouseOver = false;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
    }
}
