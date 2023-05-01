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

    public DestinationRegistry destinationRegistry;

    public Document[] stampPrefabs;
    public Document addressDocumentPrefab;

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
        Document addressDocument = Instantiate(addressDocumentPrefab);
        package.PlaceDocument(addressDocument);

        int systemIndex = Random.Range(0, destinationRegistry.systems.Length);
        addressDocument.SetData("system", destinationRegistry.systems[systemIndex].name);
        addressDocument.SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        addressDocument.SetData("weight", "" + Random.Range(5, 40));
        addressDocument.SetData("actual weight", addressDocument.GetData("weight"));
        addressDocument.SetData("id", "" + Random.Range(100000, 999999));

        foreach (Document stampPrefab in stampPrefabs) {
            if (stampPrefab.GetData("system") == addressDocument.GetData("system")) {
                package.PlaceDocument(Instantiate(stampPrefab));
                break;
            }
        }

        if (Random.Range(1, 2) == -1) {
            passiveRules[Random.Range(0, passiveRules.Length)].Break(package);
        }

        addressDocument.UpdateText();
    }

    public IEnumerator Ship() {
        uiManager.EnterPackageMoveCutsceneState();

        bool denied = package.Find(Document.Type.Result).GetData("approved") == "no";

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

        if (package.Find(Document.Type.Result).GetData("approved") == "no" && brokenRules.Count == 0) {
            error = true;
            errorText = "-Package denied for no reason";
        }

        if (package.Find(Document.Type.Result).GetData("approved") == "yes" && brokenRules.Count > 0) {
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
