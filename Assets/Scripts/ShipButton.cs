using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color disabledColor;
    public Color defaultColor;
    public Color highlightColor;

    public TMP_Text text;
    public GameObject buttonGraphic;
    public PackageManager packageManager;

    public bool clickable;
    public bool mouseOver;

    public AudioSource buttonSound;

    void Update() {
        if (clickable) {
            buttonGraphic.SetActive(true);
            if (mouseOver) {
                text.color = highlightColor;

                if (Input.GetMouseButtonDown(0)) {
                    buttonSound.Play();
                    packageManager.StartCoroutine(packageManager.Ship());
                }
            }
            else {
                text.color = defaultColor;
            }
        }
        else {
            buttonGraphic.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;
    }
}
