using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// Core logic of the UI that will be used during gameplay.

public class GameplayUI : MonoBehaviour{

    public GameObject aliveUI;
    public GameObject deadUI;    
    public GameObject objectiveUI;
    public GameObject transparentUI;

    public GameObject reloadingTxt;    
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI objectiveText;
    public Image transparentPanel;
    
    private bool fading;
    private float objectiveDisplayTimer;
    private float lerpDuration;     
    private float lerpPosition;
    private float currentLerpTime;
    private Color originalColor;
    private Color targetColor;

    private void Start(){

        GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>().healthChanged += UpdatePlayerHealthValue;        

        Gun.reloadingStart += ShowReloadingText;
        Gun.reloadingEnd += HideReloadingText;
        Gun.ammoChanged += UpdatePlayerAmmoValue;
        PlayerMovement.playerDied += DeadPlayerReaction;
        RetrievalObject.objectPickedUp += showMissionPickUpText;
        SafeArea.missionComplete += showMissionCompleteText;

        SetupUI();

        lerpPosition = 0.0f;
        lerpDuration = 4.0f;
        currentLerpTime = 0.0f;
        objectiveDisplayTimer = 5.0f;
        originalColor = transparentPanel.color;
        targetColor = Color.black;
        fading = false;

        showMissionStartText();
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

    private void ShowObjectiveUI(){
        objectiveUI.SetActive(true);
        objectiveText.gameObject.SetActive(true);
        StartCoroutine(HideObjectiveText());
    }

    IEnumerator HideObjectiveText(){ 
        yield return new WaitForSeconds(objectiveDisplayTimer);
        objectiveUI.SetActive(false);
    }

    IEnumerator FadeToBlack(){       

        fading = true;

        while(fading) {

            currentLerpTime += Time.deltaTime;

            if(currentLerpTime > lerpDuration) currentLerpTime = lerpDuration;

            lerpPosition = currentLerpTime / lerpDuration;            

            transparentPanel.color = Color.Lerp(originalColor, targetColor, lerpPosition);

            if (Color.Equals(transparentPanel.color, targetColor)) fading = false;
            yield return new WaitForEndOfFrame();
        }

        // add trigger for end game actions here
        // maybe send to menu or stat screen or something. 
    }

    private void showMissionStartText(){
        ShowObjectiveUI();
        objectiveText.text = "Go out there and find the super important gray cube. We need it if we are going to stop the invasion.";
    }

    private void showMissionCompleteText() {        
        ShowObjectiveUI();
        objectiveText.text = "You did it! Job well done!";
        StartCoroutine(FadeToBlack());
    }

    private void showMissionPickUpText(){
        ShowObjectiveUI();
        objectiveText.text = "Great! You have the cube now. Bring it back to the drop zone and you will have saved humanity.";

    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}