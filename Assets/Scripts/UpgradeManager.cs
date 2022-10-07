using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static bool upgradeScreenOpen;
    public GameObject upgradeScreen;
    public bool upgradeChosen = false;
    public GameObject playerGun;
    public GameObject player;
    public Sprite handGun;
    public Sprite sniperRifle;
    public Sprite assaultRifle;
    public bool bonusScore = false;
    
    public void Update() 
    {
        // Show the upgrade screen if the user has not picked an upgrade to start with
        if (upgradeChosen) 
        {
            hideScreen();
        } else {
            showScreen();
        }
    }

    public void showScreen() 
    {
        Time.timeScale = 0;
        upgradeScreenOpen = !upgradeScreenOpen;
        upgradeScreen.SetActive(true);
    }

    public void hideScreen()
    {
        Time.timeScale = 1;
        upgradeScreenOpen = !upgradeScreenOpen;
        upgradeScreen.SetActive(false);
    }

    public void chooseGun(string gun) 
    {
        if (gun == "AssaultRifle") {
            playerGun.GetComponent<SpriteRenderer>().sprite = assaultRifle;
            player.GetComponent<PlayerController>().damage = 5;
            player.GetComponent<PlayerController>().bulletSpeed = 10;
            player.GetComponent<PlayerController>().fireRate = 0.1f;
            player.GetComponent<PlayerController>().health = 80f;
            upgradeChosen = true;
        } else if (gun == "Sniper") {
            playerGun.GetComponent<SpriteRenderer>().sprite = sniperRifle;
            playerGun.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            player.GetComponent<PlayerController>().damage = 50; // Should instant kill most except enchanted
            player.GetComponent<PlayerController>().bulletSpeed = 20;
            player.GetComponent<PlayerController>().fireRate = 1.1f;
            player.GetComponent<PlayerController>().moveSpeed = 4f;
            upgradeChosen = true;
        } else {
            // If something goes wrong or player chooses hand gun
            playerGun.GetComponent<SpriteRenderer>().sprite = handGun;
            player.GetComponent<PlayerController>().damage = 10;
            player.GetComponent<PlayerController>().bulletSpeed = 5;
            player.GetComponent<PlayerController>().fireRate = 0.5f;
            player.GetComponent<PlayerController>().moveSpeed = 6f;
            bonusScore = true;
            upgradeChosen = true;
        }
    }
}
