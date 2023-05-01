using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInputMonitor : MonoBehaviour
{
    public static TextInputMonitor instance;
    public GameObject mouseBlocker;
    bool monitoring = false;
    bool startMonitoringFrame = false;
    string originalText;
    string text;
    int cursor;
    TextInputHighlightableSegment segment;

    bool cursorVisible = false;

    bool number = false;
    bool negativesAllowed = false;

    int digitLimit = 6;

    const string allLetters = "qwertyuiopasdfghjklzxcvbnm";
    float cursorTime = 0f;

    public DragDocument documentDrag;

    void Awake() {
        instance = this;
    }

    public bool IsMonitoring() {
        return monitoring;
    }

    public void StartMonitoring(TextInputHighlightableSegment seg, bool n, bool m, int limit) {
        segment = seg;
        text = segment.text;
        originalText = text;
        cursor = text.Length;
        monitoring = true;
        startMonitoringFrame = true;
        mouseBlocker.SetActive(true);
        cursorTime = 0f;

        number = n;
        negativesAllowed = m;
        digitLimit = limit;
    }

    void Update() {
        if (monitoring) {
            bool movedCursor = false;
            cursorTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.RightArrow) && cursor < text.Length) {
                cursor++;
                movedCursor = true;
                cursorTime = 0f;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && cursor > 0) {
                cursor--;
                movedCursor = true;
                cursorTime = 0f;
            }

            if (CheckKeyInputs() || cursorVisible != (cursorTime % 1f < 0.5f) || movedCursor || startMonitoringFrame) {
                cursorVisible = (cursorTime % 1f < 0.5f);

                if (cursorVisible) {
                    if (cursor == text.Length) {
                        segment.SetText(text + "<mark=#FFFFFF55><color=#FFFFFF00>a</color></mark>");
                    }
                    else {
                        segment.SetText((text).Insert(cursor+1, "</mark>").Insert(cursor, "<mark=#FFFFFF55>"));
                    }
                }
                else {
                    if (cursor == text.Length) {
                        segment.SetText(text + " ");
                    }
                    else {
                        segment.SetText(text);
                    }
                }
            }

            if (!startMonitoringFrame && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))) {
                if (number) {
                    int i = 0;
                    if (text.Length > 0 && text[0] == '-') {
                        i = 1;
                    }
                    while (text.Length > i && text[i] == '0') {
                        text = text.Remove(i, 1);
                    }
                }

                if (text == "" || text == "-") {
                    text = "0";
                }
                segment.SetText(text);
                
                segment.Confirm();

                monitoring = false;
                mouseBlocker.SetActive(false);
                documentDrag.clickedThisFrame = true;
            }

            startMonitoringFrame = false;
        }
    }

    bool CheckKeyInputs() {
        string textBefore = text;

        string inputString = Input.inputString;

        for (int i = 0; i < inputString.Length; i++) {
            char c = inputString[i];

            if (c == '\b') {
                Debug.Log("Backspace detected");
                if (cursor > 0) {
                    Debug.Log("Removing end character");
                    text = text.Remove(cursor-1, 1);
                    cursor--;
                }
            }
            else if (Char.IsDigit(c) &&
                !(number && cursor == 0 && text.Length > 0 && text[0] == '-') &&
                !(number && ((text.Length == digitLimit && text[0] != '-') || (text.Length == digitLimit+1 && text[0] == '-'))))
            {
                text = text.Insert(cursor, "" + c);
                cursor++;
            }
            else if (Char.IsLetter(c) && !number) {
                text = text.Insert(cursor, "" + c);
                cursor++;
            }
            else if (c == ' ' && !number) {
                text = text.Insert(cursor, "" + c);
                cursor++;
            }
            else if (c == '-' &&
                !(number && cursor > 0) &&
                !(number && cursor == 0 && text.Length > 0 && text[0] == '-') && 
                !(number && !negativesAllowed))
            {
                text = text.Insert(cursor, "" + c);
                cursor++;
            }
        }

        if (text != textBefore) {
            cursorTime = 0f;
        }

        return text != textBefore;
    }
}
