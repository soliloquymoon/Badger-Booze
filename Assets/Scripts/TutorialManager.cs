using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Button okButton;
    private CanvasGroup dimFadeGroup;
    public GameObject WelcomePanel;
    public GameObject WindowTutorial;
    public GameObject Dimmer;
    // Start is called before the first frame update
    private void Start()
    {
        dimFadeGroup = GameObject.Find("DimB").GetComponent<CanvasGroup>();
        //if(PlayerPrefs.GetString("unity.player_session_count") == "1"){
        //    Debug.Log("Meow");
        //    WelcomePanel.SetActive(false);
        //    ShowWelcomeMessage(3F);
       // }

        if (PlayerPrefs.HasKey("FIRSTTIMEOPENING")) {
            Debug.Log("FIRSTTIMEOPENING CREATED");
        }


       if (PlayerPrefs.GetInt("FIRSTTIMEOPENING") == 1) {
            WelcomePanel.SetActive(false);
            StartCoroutine(ShowWelcomeMessage(1.5F));
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
       } 

    }
    

    public void OnOKClick() {
        Dimmer.SetActive(false);
        WelcomePanel.SetActive(false);
        StartCoroutine(ShowWindowTutorials(1.5F));
    }

    public void OnOKWTClick() {
        dimFadeGroup.alpha = 0;
        WindowTutorial.SetActive(false);
    }

    IEnumerator ShowWelcomeMessage(float duration) {
        yield return new WaitForSeconds(duration);

        
        Dimmer.SetActive(true);
        WelcomePanel.SetActive(true);
    }


    IEnumerator ShowWindowTutorials(float duration) {
        yield return new WaitForSeconds(duration);

        
        dimFadeGroup.alpha = 0.8F;
        WindowTutorial.SetActive(true);
    }



    
}
