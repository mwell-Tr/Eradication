using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameplayUI : MonoBehaviour{

    public GameObject aliveUI;
    public GameObject deadUI;

    public TextMeshProUGUI healthDisplay;

    private void Start(){
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>().healthChanged += UpdatePlayerHealthValue;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().playerDied += DeadPlayerReaction;

        SetupUI();
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

    private void DeadPlayerReaction(){        
        aliveUI.SetActive(false);
        deadUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}