using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour{

    public GameObject playerRef;
    public TextMeshProUGUI healthDisplay;

    private Damageable playerDamageable;

    private void Start(){
        playerDamageable = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();

        playerDamageable.damageTaken += UpdatePlayerHealthValue;
    }

    private void UpdatePlayerHealthValue(int healthDisplayValue){
        healthDisplay.text = healthDisplayValue.ToString();
    }
}