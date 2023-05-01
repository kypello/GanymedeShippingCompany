using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Palette palette;
    public ShipButton shipButton;
    public TMP_Text shipButtonText;
    public Package package;
    public Button rulebookButton;
    public TMP_Text rulebookButtonText;
    public Button planetButton;
    public TMP_Text planetButtonText;
    public PackageManager packageManager;

    public enum State {
        SelectDocument, DraggingDocument, PackageMoveCutscene, Rulebook, PlanetList
    }

    public State state;

    public void EnterSelectDocumentState() {
        palette.Show();
        shipButton.clickable = package.ResultStampPresent();
        /*
        if (package.Find(Document.Type.Result) != null && package.Find(Document.Type.Result).GetData("approved") == "no") {
            shipButtonText.text = "<<< Next";
        }
        else {
            shipButtonText.text = "Next >>>";
        }
        */
        shipButtonText.text = "Next";

        rulebookButton.gameObject.SetActive(true);
        rulebookButtonText.text = "View Rulebook";
        planetButton.gameObject.SetActive(true);
        planetButtonText.text = "Planet List";
        state = State.SelectDocument;

        if (!packageManager.dayStarted) {
            packageManager.StartDay();
            rulebookButton.mouseOver = false;
        }
    }

    public void EnterDraggingDocumentState() {
        palette.Hide();
        shipButton.clickable = false;
        rulebookButton.gameObject.SetActive(false);
        planetButton.gameObject.SetActive(false);
        state = State.DraggingDocument;
    }

    public void EnterPackageMoveCutsceneState() {
        palette.Hide();
        shipButton.clickable = false;
        rulebookButton.gameObject.SetActive(false);
        planetButton.gameObject.SetActive(false);
        state = State.PackageMoveCutscene;
    }

    public void EnterRulebookState() {
        palette.Hide();
        shipButton.clickable = false;
        planetButton.gameObject.SetActive(false);
        rulebookButton.gameObject.SetActive(true);
        rulebookButtonText.text = "Close";
        state = State.Rulebook;
    }

    public void EnterPlanetListState() {
        palette.Hide();
        shipButton.clickable = false;
        rulebookButton.gameObject.SetActive(false);
        planetButton.gameObject.SetActive(true);
        planetButtonText.text = "Close";
        state = State.PlanetList;
    }
}
