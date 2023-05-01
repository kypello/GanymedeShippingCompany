using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddressDocument : Document
{
    public TMP_Text mainText;
    public TMP_Text idText;
    public RectTransform barcode;

    public override void UpdateText() {
        mainText.text = "<b>SHIP TO:</b>\n" + GetData("location") + "\n" + GetData("system") + "\n\n<b>WEIGHT:</b> " + GetData("weight") + "kg";
        idText.text = "ID: " + GetData("id");

        barcode.anchoredPosition = Vector3.right * Random.Range(-560f, 560f);
    }
}
