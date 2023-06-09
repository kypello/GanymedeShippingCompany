using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Palette : MonoBehaviour
{
    public UIManager uiManager;

    public RectTransform highlight;
    public PaletteDocument[] paletteDocuments;
    public DragDocument dragDocument;

    public AudioSource documentPickupSound;

    bool visible = true;

    public void SetAvailableDocuments(int documents) {
        for (int i = 0; i < paletteDocuments.Length; i++) {
            if (i >= documents) {
                paletteDocuments[i].gameObject.SetActive(false);
            }
        }
    }

    void Update() {
        bool highlightingDocument = false;
        PaletteDocument documentBeingHighlighted = null;

        foreach (PaletteDocument paletteDocument in paletteDocuments) {
            if (paletteDocument.mouseOver) {
                highlightingDocument = true;
                documentBeingHighlighted = paletteDocument;
                break;
            }
        }

        if (highlightingDocument) {
            highlight.gameObject.SetActive(true);
            highlight.anchoredPosition = documentBeingHighlighted.rectTransform.anchoredPosition;

            if (Input.GetMouseButtonDown(0)) {
                documentPickupSound.Play();
                dragDocument.SpawnNewDocument(documentBeingHighlighted.documentPrefab);
                uiManager.EnterDraggingDocumentState();
            }
        }
        else {
            highlight.gameObject.SetActive(false);
        }
    }

    public void Hide() {
        if (visible) {
            foreach (PaletteDocument paletteDocument in paletteDocuments) {
                paletteDocument.mouseOver = false;
            }
            highlight.gameObject.SetActive(false);
            gameObject.SetActive(false);
            visible = false;
        }
    }

    public void Show() {
        if (!visible) {
            visible = true;
            gameObject.SetActive(true);
        }
    }
}
