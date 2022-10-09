using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeScreen;
    public bool upgradeChosen = false;
    public GameObject playerGun;
    public GameObject player;
    public Sprite handGun;
    public Sprite sniperRifle;
    public Sprite assaultRifle;
    public bool bonusScore = false;
    
    public void Start() 
    {
       showScreen();
    }
    // Pause the game and show the weapon choosing screen to apply some upgrades for the player
    // depending on the choice of weapon they choose
    public void showScreen() 
    {
        Time.timeScale = 0;
        upgradeScreen.SetActive(true);
    }
    // Once the weapon has been chosen, resume the game and close the screen
    public void hideScreen()
    {
        Time.timeScale = 1;
        upgradeScreen.SetActive(false);
    }
    // The buttons in the UI will run this function with a gun name
    // depending on which gun the player chose
    public void chooseGun(string gun) 
    {
        hideScreen(); // Hide the screen as the player has chosen
        if (gun == "AssaultRifle") {
            playerGun.GetComponent<SpriteRenderer>().sprite = assaultRifle;
            player.GetComponent<PlayerController>().damage = 5;
            player.GetComponent<PlayerController>().bulletSpeed = 60;
            player.GetComponent<PlayerController>().fireRate = 0.1f;
            player.GetComponent<PlayerController>().health = 80f;
            upgradeChosen = true;
        } else if (gun == "Sniper") {
            playerGun.GetComponent<SpriteRenderer>().sprite = sniperRifle;
            playerGun.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            player.GetComponent<PlayerController>().damage = 50; // Should instant kill most except enchanted
            player.GetComponent<PlayerController>().bulletSpeed = 50;
            player.GetComponent<PlayerController>().fireRate = 1.1f;
            player.GetComponent<PlayerController>().moveSpeed = 4f;
            upgradeChosen = true;
        } else {
            // If something goes wrong or player chooses hand gun
            playerGun.GetComponent<SpriteRenderer>().sprite = handGun;
            player.GetComponent<PlayerController>().damage = 10;
            player.GetComponent<PlayerController>().bulletSpeed = 50;
            player.GetComponent<PlayerController>().fireRate = 0.5f;
            player.GetComponent<PlayerController>().moveSpeed = 6f;
            bonusScore = true;
            upgradeChosen = true;
        }
    }
}
