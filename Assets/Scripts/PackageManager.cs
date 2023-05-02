using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public Day endlessDay;
    public Day[] storyDays;
    public Day currentDay;
    
    bool testedRule = false;

    public Button errorContinueButton;
    public GameObject errorMessage;
    public TMP_Text errorMessageText;

    public TMP_Text weightDisplay;
    public TMP_Text dateDisplay;

    public TMP_Text rulebook;

    int packagesSent = 0;

    public Palette palette;
    public Timer timer;

    public bool dayStarted = false;
    public RulebookPopup rulebookPopup;

    public Animation fadeAnim;
    public TMP_Text dayText;
    public Animation dayTextAnim;
    public AudioSource music;

    public AudioSource buttonSound;
    public AudioSource correctSound;
    public AudioSource incorrectSound;
    public Animation camShake;
    public AudioSource conveyorSound;

    public Material conveyor;

    public TMP_Text scoreDisplay;

    public static bool endless = true;
    public static int correct;
    public static int mistakes;

    public ParticleSystem confettiA;
    public ParticleSystem confettiB;
    public ParticleSystem confettiC;

    void Start() {
        if (endless) {
            currentDay = endlessDay;
        }
        else {
            currentDay = storyDays[DateAuthority.date - 1];
        }
        dateDisplay.text = DateAuthority.date + " May 2164 UST";
        UpdateScoreDisplay();

        string rulebookText = "";

        if (!endless) {
            rulebookText += "<color=#AAAAFF50><b>" + currentDay.storyText + "</b></color>\n\n"; 
        }

        foreach (Rule rule in currentDay.passiveRules) {
            if (rule.appearsInRulebook) {
                rulebookText += "- " + rule.rulebookEntry + "\n\n";
            }
        }
        foreach (Rule rule in currentDay.activeRules) {
            if (rule.appearsInRulebook) {
                rulebookText += "- " + rule.rulebookEntry + "\n\n";
            }
        }
        rulebook.text = rulebookText;

        palette.SetAvailableDocuments(currentDay.availableDocuments);

        rulebookPopup.InitialOpen();
        uiManager.EnterRulebookState();
        StartCoroutine(FadeIn());
    }

    void UpdateScoreDisplay() {
        scoreDisplay.text = "Correct: " + correct + "\nMistakes: " + mistakes;
    }

    public void StartDay() {
        dayStarted = true;
        timer.StartTiming();
        music.Play();
        StartCoroutine(Receive());
    }

    public IEnumerator Receive() {
        uiManager.EnterPackageMoveCutsceneState();

        SetUpPackage();

        conveyorSound.Play();

        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2f;
            package.transform.position = Vector3.Lerp(packageSpawn, packageRest, t);
            conveyor.SetTextureOffset("_MainTex", Vector2.right * Mathf.Lerp(0f, 7f, t));
            yield return null;
        }

        conveyorSound.Stop();

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
        switch (currentDay.legalDestinations) {
            case Day.LegalDestinations.Jovian:
                systemIndex = 5;
                break;
            case Day.LegalDestinations.GasGiants:
                systemIndex = Random.Range(5, 10);
                break;
            case Day.LegalDestinations.All:
                systemIndex = Random.Range(0, 10);
                break;
        }

        addressDocument.SetData("system", destinationRegistry.systems[systemIndex].name);
        dummyStamp.SetData("system", destinationRegistry.systems[systemIndex].name);
        addressDocument.SetData("location", destinationRegistry.systems[systemIndex].locations[Random.Range(0, destinationRegistry.systems[systemIndex].locations.Length)]);

        if (addressDocument.GetData("system") == "Uranian System") {
            addressDocument.SetData("weight", "" + Random.Range(5, 15));
        }
        else {
            addressDocument.SetData("weight", "" + Random.Range(5, 40));
        }
        addressDocument.SetData("actual weight", addressDocument.GetData("weight"));
        addressDocument.SetData("id", "" + Random.Range(100000, 999999));

        
        if (currentDay.doRuleTest && packagesSent == currentDay.testRulePackageNum && !testedRule) {
            currentDay.testRule.Test(package);
            testedRule = false;
        }
        else if (Random.Range(0, 2) == 0) {
            //passiveRules[Random.Range(0, passiveRules.Length)].Break(package);
            currentDay.passiveRules[Random.Range(0, currentDay.passiveRules.Length)].Break(package);
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

        conveyorSound.Play();

        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 2f;
            if (denied) {
                package.transform.position = Vector3.Lerp(packageRest, packageSpawn, t);
                conveyor.SetTextureOffset("_MainTex", Vector2.right * Mathf.Lerp(7f, 0f, t));
            }
            else {
                package.transform.position = Vector3.Lerp(packageRest, packageExit, t);
                conveyor.SetTextureOffset("_MainTex", Vector2.right * Mathf.Lerp(0f, 7f, t));
            }
            
            yield return null;
        }

        conveyorSound.Stop();

        yield return CheckPackage();

        if (timer.timing) {
            StartCoroutine(Receive());
        }
        else {
            DateAuthority.date++;
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn() {
        fadeAnim.gameObject.SetActive(true);
        dayText.text = "Day " + DateAuthority.date;
        dayTextAnim.Play();
        yield return new WaitForSeconds(2f);
        fadeAnim.Play("FadeIn");
        yield return new WaitForSeconds(0.5f);
        fadeAnim.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut() {
        fadeAnim.gameObject.SetActive(true);
        fadeAnim.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);

        if (endless || DateAuthority.date <= storyDays.Length) {
            SceneManager.LoadScene(1);
        }
        else {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator CheckPackage() {
        bool error = false;
        string errorText = "";

        List<Rule> brokenRules = new List<Rule>();

        foreach (Rule rule in currentDay.passiveRules) {
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
            foreach (Rule rule in currentDay.activeRules) {
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
            incorrectSound.Play();
            mistakes++;
            UpdateScoreDisplay();
            camShake.Play();
            errorContinueButton.gameObject.SetActive(true);
            errorContinueButton.mouseOver = false;
            errorMessage.SetActive(true);
            errorMessageText.text = errorText;

            while (!errorContinueButton.mouseOver || !Input.GetMouseButtonDown(0)) {
                yield return null;
            }

            buttonSound.Play();

            errorContinueButton.gameObject.SetActive(false);
            errorMessage.SetActive(false);
        }
        else {
            correctSound.Play();
            correct++;
            confettiA.Play();
            confettiB.Play();
            confettiC.Play();
            UpdateScoreDisplay();
        }

        packagesSent++;
    }
}
