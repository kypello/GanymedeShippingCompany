using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetListPopup : MonoBehaviour
{
    public UIManager uiManager;

    public Button planetListButton;
    public GameObject planetList;

    bool planetListOpen = false;

    public TMP_Text systemTitle;
    public TMP_Text locationList;
    public GameObject[] stamps;

    public DestinationRegistry destinationRegistry;
    public Button rightButton;
    public Button leftButton;

    public AudioSource buttonSound;

    int systemIndex = 0;

    void Update() {
        if (!planetListOpen) {
            if (planetListButton.mouseOver) {
                if (Input.GetMouseButtonDown(0)) {
                    buttonSound.Play();
                    planetListOpen = true;
                    planetList.SetActive(true);
                    OpenPage();
                    uiManager.EnterPlanetListState();
                }
            }
        }
        else {
            if ((rightButton.mouseOver && Input.GetMouseButtonDown(0)) || (systemIndex < 9 && Input.GetKeyDown(KeyCode.RightArrow))) {
                buttonSound.Play();
                systemIndex++;
                OpenPage();
            }
            if ((leftButton.mouseOver && Input.GetMouseButtonDown(0)) || (systemIndex > 0 && Input.GetKeyDown(KeyCode.LeftArrow))) {
                buttonSound.Play();
                systemIndex--;
                OpenPage();
            }

            if (planetListButton.mouseOver) {
                if (Input.GetMouseButtonDown(0)) {
                    buttonSound.Play();
                    planetListOpen = false;
                    planetList.SetActive(false);
                    uiManager.EnterSelectDocumentState();
                }
            }
        }
    }

    void OpenPage() {
        if (systemIndex == 0) {
            leftButton.gameObject.SetActive(false);
            leftButton.mouseOver = false;
        }
        else {
            leftButton.gameObject.SetActive(true);
        }

        if (systemIndex == 9) {
            rightButton.gameObject.SetActive(false);
            rightButton.mouseOver = false;
        }
        else {
            rightButton.gameObject.SetActive(true);
        }

        systemTitle.text = destinationRegistry.systems[systemIndex].name;

        string locationListText = "";

        if (systemIndex == 5) {
            locationListText += "<size=96>Jupiter</size>\n\nGanymede<size=32><color=#FFFF00> <--you are here!</color></size>\n\n";
        }
        else if (systemIndex == 6) {
            locationListText += "<size=96>Saturn</size>\n\n";
        }
        else if (systemIndex == 7) {
            locationListText += "<size=96>Neptune</size>\n\n";
        }
        else if (systemIndex == 8) {
            locationListText += "<size=96>Uranus</size>\n\n";
        }

        foreach (string location in destinationRegistry.systems[systemIndex].locations) {
            if (location == "Mercury" || location == "Venus" || location == "Terra" || location == "Mars") {
                locationListText += "<size=96>" + location + "</size>\n\n";
            }
            else {
                locationListText += location + "\n\n";
            }
        }
        locationList.text = locationListText;

        for (int i = 0; i < 10; i++) {
            stamps[i].SetActive(i == systemIndex);
        }
    }
}
