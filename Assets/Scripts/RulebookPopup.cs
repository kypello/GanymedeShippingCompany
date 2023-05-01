using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RulebookPopup : MonoBehaviour
{
    public UIManager uiManager;

    public Button rulebookButton;
    public Button closeButton;
    public GameObject rules;
    public TMP_Text closeButtonText;

    public AudioSource buttonSound;

    bool rulesOpen = false;

    void Update() {
        if (!rulesOpen) {
            if (rulebookButton.mouseOver) {
                if (Input.GetMouseButtonDown(0)) {
                    buttonSound.Play();
                    rulesOpen = true;
                    rules.SetActive(true);
                    uiManager.EnterRulebookState();
                }
            }
        }
        else {
            if (rulebookButton.mouseOver) {
                if (Input.GetMouseButtonDown(0)) {
                    buttonSound.Play();
                    rulesOpen = false;
                    rules.SetActive(false);
                    uiManager.EnterSelectDocumentState();
                }
            }
        }
    }

    public void InitialOpen() {
        rulesOpen = true;
        rules.SetActive(true);
    }
}
