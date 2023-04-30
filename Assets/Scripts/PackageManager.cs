using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PackageManager : MonoBehaviour
{
    public UIManager uiManager;

    public Package package;
    public Vector3 packageSpawn;
    public Vector3 packageRest;
    public Vector3 packageExit;

    public Document stamp;
    public Document address;

    public Rule[] passiveRules;

    public Button errorContinueButton;
    public GameObject errorMessage;
    public TMP_Text errorMessageText;

    void Start() {
        StartCoroutine(Receive());
    }

    public IEnumerator Receive() {
        uiManager.EnterPackageMoveCutsceneState();

        SetUpPackage();

        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2f;
            package.transform.position = Vector3.Lerp(packageSpawn, packageRest, t);
            yield return null;
        }

        uiManager.EnterSelectDocumentState();
    }

    void SetUpPackage() {
        package.Clear();
        package.PlaceDocument(Instantiate(address));
        package.PlaceDocument(Instantiate(stamp));

        if (Random.Range(0, 2) == 0) {
            passiveRules[Random.Range(0, passiveRules.Length)].Break(package);
        }
    }

    public IEnumerator Ship() {
        uiManager.EnterPackageMoveCutsceneState();

        bool denied = package.Find(Document.Type.Denied) != null;

        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2f;
            if (denied) {
                package.transform.position = Vector3.Lerp(packageRest, packageSpawn, t);
            }
            else {
                package.transform.position = Vector3.Lerp(packageRest, packageExit, t);
            }
            
            yield return null;
        }

        StartCoroutine(CheckPackage());
    }

    IEnumerator CheckPackage() {
        bool error = false;
        string errorText = "";

        List<Rule> brokenRules = new List<Rule>();

        foreach (Rule rule in passiveRules) {
            if (!rule.Verify(package)) {
                brokenRules.Add(rule);
            }
        }

        if (package.Find(Document.Type.Denied) != null && brokenRules.Count == 0) {
            error = true;
            errorText = "-Package denied for no reason";
        }

        if (package.Find(Document.Type.Approved) != null && brokenRules.Count > 0) {
            error = true;
            foreach (Rule brokenRule in brokenRules) {
                errorText += "-" + brokenRule.errorText + "\n";
            }
        }

        if (error) {
            errorContinueButton.gameObject.SetActive(true);
            errorContinueButton.mouseOver = false;
            errorMessage.SetActive(true);
            errorMessageText.text = errorText;

            while (!errorContinueButton.mouseOver || !Input.GetMouseButtonDown(0)) {
                yield return null;
            }

            errorContinueButton.gameObject.SetActive(false);
            errorMessage.SetActive(false);
        }

        StartCoroutine(Receive());
    }
}
