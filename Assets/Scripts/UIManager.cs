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

    public enum State {
        SelectDocument, DraggingDocument, PackageMoveCutscene
    }

    public State state;

    public void EnterSelectDocumentState() {
        palette.Show();
        shipButton.clickable = package.ResultStampPresent();
        if (package.Find(Document.Type.Result) != null && package.Find(Document.Type.Result).GetData("approved") == "no") {
            shipButtonText.text = "<<< Next";
        }
        else {
            shipButtonText.text = "Next >>>";
        }
        state = State.SelectDocument;
    }

    public void EnterDraggingDocumentState() {
        palette.Hide();
        shipButton.clickable = false;
        state = State.DraggingDocument;
    }

    public void EnterPackageMoveCutsceneState() {
        palette.Hide();
        shipButton.clickable = false;
        state = State.PackageMoveCutscene;
    }
}
