using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownHighlightableSegment : HighlightableSegment
{
    public string[] items;

    public override void GetClicked() {
        DropdownMenu.instance.Display(items, Input.mousePosition, this);
    }

    public void ReceiveDropdownInput(string dropdownInput) {
        text = dropdownInput;
        parentDocument.SetData(documentDataFieldKey, text);
    }
}
