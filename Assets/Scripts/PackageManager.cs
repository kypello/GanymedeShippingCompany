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
    public Document dummyStampPrefab;
    public Document addressDocumentPrefab;

    public Rule[] passiveRules;
    public Rule[] activeRules;

    public Rule testRule;
    bool testedRule = false;

    public Button errorContinueButton;
    public GameObject errorMessage;
    public TMP_Text errorMessageText;

    public TMP_Text weightDisplay;

    public enum LegalDestinations {Jovian, GasGiants, All}
    public LegalDestinations legalDestinations = LegalDestinations.Jovian;

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

        if (package.Find(Document.Type.Address) != null) {
            weightDisplay.text = "<b>Weight:</b> " + package.Find(Document.Type.Address).GetData("actual weight") + "kg";
        }
        else {
            weightDisplay.text = "<b>Weight:</b> " + Random.Range(5, 40) + "kg";
        }

        uiManager.EnterSelectDocumentState();
    }

    void SetUpPackage() {
        package.Clear();

        Document addressDocument = Instantiate(addressDocumentPrefab);
        package.PlaceDocument(addressDocument);

        Document dummyStamp = Instantiate(dummyStampPrefab);
        package.PlaceDocument(dummyStamp);

        int systemIndex = 0;
        switch (legalDestinations) {
            case LegalDestinations.Jovian:
                systemIndex = 5;
                break;
            case LegalDestinations.GasGiants:
                systemIndex = Random.Range(5, 10);
                break;
            case LegalDestinations.All:
                systemIndex = Random.Range(0, 10);
                break;
        }

        addressDocument.SetData("system", destinationRegistry.systems[systemIndex].name);
        dummyStamp.SetData("system", destinationRegistry.systems[systemIndex].name);
        addressDocument.SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);
        addressDocument.SetData("weight", "" + Random.Range(5, 40));
        addressDocument.SetData("actual weight", addressDocument.GetData("weight"));
        addressDocument.SetData("id", "" + Random.Range(100000, 999999));

        
        if (!testedRule) {
            testRule.Test(package);
            testedRule = false;
        }
        else if (false) {
            //passiveRules[Random.Range(0, passiveRules.Length)].Break(package);
            passiveRules[0].Break(package);
        }

        if (package.Find(Document.Type.Stamp) != null) {
            string stampSystem = dummyStamp.GetData("system");
            package.RemoveDocument(dummyStamp, true);
            Document realStamp = null;

            foreach (Document stampPrefab in stampPrefabs) {
                if (stampPrefab.GetData("system") == stampSystem) {
                    realStamp = Instantiate(stampPrefab);
                    break;
                }
            }

            package.PlaceDocument(realStamp);
        }
        

        addressDocument.UpdateText();
    }

    public IEnumerator Ship() {
        uiManager.EnterPackageMoveCutsceneState();

        bool denied = package.Find(Document.Type.Result).GetData("approved") == "no";

        weightDisplay.text = "<b>Weight:</b> 0kg";

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
                break;
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

        if (package.Find(Document.Type.Result).GetData("approved") == "yes" && brokenRules.Count == 0) {
            foreach (Rule rule in activeRules) {
                if (!rule.Verify(package)) {
                    brokenRules.Add(rule);
                }
            }

            if (brokenRules.Count > 0) {
                error = true;
                foreach (Rule brokenRule in brokenRules) {
                    errorText += "-" + brokenRule.errorText + "\n";
                }
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
