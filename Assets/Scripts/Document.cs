using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Document : MonoBehaviour
{
    public int width = 1;
    public int height = 1;

    public Vector2Int bottomLeftPosition;

    public bool deleteAllowed = true;

    bool transparent = false;

    public Image[] images;
    public TMP_Text[] texts;

    public enum DocumentClass {
        Free, ResultStamp
    }
    public DocumentClass documentClass;

    public enum Type {
        Stamp, Address, Approved, Denied
    }
    public Type type;

    public void SetTransparent() {
        if (!transparent) {
            foreach (Image element in images) {
                element.color = new Color(element.color.r, element.color.g, element.color.b, 0.4f);
            }
            foreach (TMP_Text element in texts) {
                element.color = new Color(element.color.r, element.color.g, element.color.b, 0.4f);
            }
            transparent = true;
        }
    }

    public void SetOpaque() {
        if (transparent) {
            foreach (Image element in images) {
                element.color = new Color(element.color.r, element.color.g, element.color.b, 1f);
            }
            foreach (TMP_Text element in texts) {
                element.color = new Color(element.color.r, element.color.g, element.color.b, 1f);
            }
            transparent = false;
        }
    }
}
