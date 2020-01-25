using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Core logic of the UI that will be used during gameplay.

public class GameplayUI : MonoBehaviour{

    public GameObject aliveUI;
    public GameObject deadUI;    
    public GameObject progressUI;
    public GameObject transparentUI;

    public GameObject reloadingTxt;    
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI variableStatusText;
    public Image transparentPanel;

    private Color originalColor;
    private Color targetColor;
    private bool colorTransitioning;

    float lerpDuration = 4f; //The length of time your Lerp takes in seconds.
    float currentLerpTime = 0f; //How far into your Lerp you are.


    private void Start(){

        // could probably make the event static so that 
        // the event can be found without the use of 
        // FindGameObjectWithTag
        GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>().healthChanged += UpdatePlayerHealthValue;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerDied += DeadPlayerReaction;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Gun>().reloadingStart += ShowReloadingText;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Gun>().reloadingEnd += HideReloadingText;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Gun>().ammoChanged += UpdatePlayerAmmoValue;

        RetrievalObject.objectPickedUp += ShowPlayerPickUpText;
        SafeArea.missionComplete += showMissionCompleteText;

        SetupUI();

        originalColor = transparentPanel.color;
        targetColor = new Color(0,0,0,1);
        colorTransitioning = true;
    }

    private void SetupUI(){
        aliveUI.SetActive(true);
        deadUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UpdatePlayerHealthValue(int healthDisplayValue){
        healthDisplay.text = healthDisplayValue.ToString();
    }

    private void UpdatePlayerAmmoValue(int ammoDisplayValue){
        ammoDisplay.text = ammoDisplayValue.ToString();
    }

    private void ShowReloadingText(){
        reloadingTxt.SetActive(true);
    }

    private void HideReloadingText(){
        reloadingTxt.SetActive(false);
    }

    private void DeadPlayerReaction(){        
        aliveUI.SetActive(false);
        deadUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void ShowPlayerPickUpText(){
        progressUI.SetActive(true);
        StartCoroutine(HideStatusText());
    }

    IEnumerator HideStatusText(){ 
        yield return new WaitForSeconds(5.0f);
        progressUI.SetActive(false);
    }

    IEnumerator FadeToBlack() {       

        while(colorTransitioning) {
            Debug.Log("Started");

            currentLerpTime += Time.deltaTime;

            if(currentLerpTime > lerpDuration){ 
                currentLerpTime = lerpDuration;
            }

            float lerpPosition = currentLerpTime / lerpDuration;            

            transparentPanel.color = Color.Lerp(originalColor, targetColor, lerpPosition);

            if (Color.Equals(transparentPanel.color, targetColor)) colorTransitioning = false;
            yield return new WaitForEndOfFrame();
        }
    }

    private void showMissionCompleteText() {        
        progressUI.SetActive(true);
        variableStatusText.text = "You did it! Job well done!";
        StartCoroutine(FadeToBlack());
    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
