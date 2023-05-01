using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInputHighlightableSegment : HighlightableSegment
{
    bool defaultText = true;
    public int characterLimit;
    public bool ensureCharacterCount;
    public int max;

    public override void GetClicked() {
        if (defaultText) {
            text = "";
            defaultText = false;
        }
        TextInputMonitor.instance.StartMonitoring(this, true, false, characterLimit);
    }

    public void SetText(string t) {
        text = t;
    }

    public void Confirm() {
        if (int.Parse(text) > max) {
            text = "" + max;
        }

        if (ensureCharacterCount) {
            while (text.Length < characterLimit) {
                text = text.Insert(0, "0");
            }
        }

        parentDocument.SetData(documentDataFieldKey, text);
    }
}
