using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Document : MonoBehaviour
{
    [System.Serializable]
    public struct Data {
        public string key;
        public string value;

        public void Set(string v) {
            value = v;
        }
    }
    public Data[] dataFields;

    public int width = 1;
    public int height = 1;

    public Vector2Int bottomLeftPosition;

    public bool deleteAllowed = true;

    public enum TransparentState {Transparent, SemiTransparent, Opaque}
    TransparentState transparentState = TransparentState.Opaque;

    public Image[] images;
    public TMP_Text[] texts;

    public HighlightableSegment[] highlightableSegments;

    public enum Type {
        Free, Stamp, Address, Result, Visa, AsteroidPass, QuarantineMandate
    }
    public Type type;

    public virtual void UpdateText() {}

    public HighlightableSegment CheckHighlightableSegments() {
        foreach (HighlightableSegment highlightableSegment in highlightableSegments) {
            if (highlightableSegment.mouseOver) {
                highlightableSegment.highlightActive = true;
                return highlightableSegment;
            }
        }
        return null;
    }

    public void SetTransparentState(TransparentState state) {
        if (state != transparentState) {
            float alpha;
            switch (state) {
                case TransparentState.Transparent:
                    alpha = 0.4f;
                    break;
                case TransparentState.SemiTransparent:
                    alpha = 0.9f;
                    break;
                default:
                    alpha = 1f;
                    break;
            }

            foreach (Image element in images) {
                element.color = new Color(element.color.r, element.color.g, element.color.b, alpha);
            }
            foreach (TMP_Text element in texts) {
                element.color = new Color(element.color.r, element.color.g, element.color.b, alpha);
            }

            transparentState = state;
        }
    }

    public void SetData(string key, string value) {
        for (int i = 0; i < dataFields.Length; i++) {
            if (dataFields[i].key == key) {
                dataFields[i].Set(value);
                return;
            }
        }
        Debug.Log("Key not found");
    }

    public string GetData(string key) {
        foreach (Data data in dataFields) {
            if (data.key == key) {
                return data.value;
            }
        }
        return "";
    }
}
