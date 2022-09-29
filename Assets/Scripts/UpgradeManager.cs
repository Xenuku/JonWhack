using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static bool upgradeScreenOpen;
    public GameObject upgradeScreen;
    
    public void Update() 
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            // For testing purposes only, show the screen when 'U' is pressed
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
}
