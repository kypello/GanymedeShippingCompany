using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    bool popupOpen = false;
    bool loading = false;
    public Button storyButton;
    public Button endlessButton;
    public GameObject popup;
    public Camera cam;

    public Animation fadeAnim;

    public TMP_Text playButton;
    public Color playButtonDefault;
    public Color playButtonHighlight;

    public AudioSource buttonSound;
    
    void Update()
    {
        if (popupOpen) {
            if (storyButton.mouseOver) {
                if (Input.GetMouseButtonDown(0) && !loading) {
                    DateAuthority.date = 1;
                    PackageManager.endless = false;
                    buttonSound.Play();
                    StartCoroutine(Load());
                }
            }
            if (endlessButton.mouseOver) {
                if (Input.GetMouseButtonDown(0) && !loading) {
                    DateAuthority.date = 1;
                    PackageManager.endless = true;
                    buttonSound.Play();
                    StartCoroutine(Load());
                }
            }
        }
        else {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 20f, 1<<8)) {
                playButton.color = playButtonHighlight;

                if (Input.GetMouseButtonDown(0)) {
                    buttonSound.Play();
                    popupOpen = true;
                    popup.SetActive(true);
                }
            }
            else {
                playButton.color = playButtonDefault;
            }
        }
    }

    IEnumerator Load() {
        loading = true;
        fadeAnim.Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
