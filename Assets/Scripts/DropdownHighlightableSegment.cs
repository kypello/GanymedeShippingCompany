using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownHighlightableSegment : HighlightableSegment
{
    public string[] items;

    public override void GetClicked() {
        DropdownMenu.instance.Display(items, new Vector2(Input.mousePosition.x / Screen.width * 2560f, Input.mousePosition.y / Screen.height * 1440f), this);
    }

    public void ReceiveDropdownInput(string dropdownInput) {
        text = dropdownInput;
        parentDocument.SetData(documentDataFieldKey, text);
    }
}
