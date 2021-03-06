using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToLoad = 4f;

    public string nextLevel;

    public bool isPause;

    public int currentCoins;

    public Transform startPoint;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.transform.position = startPoint.position;

        PlayerController.instance.canMove = true;

        currentCoins = Tracker.instance.currentCoins;

        Time.timeScale = 1f;

        UIController.instance.coinText.text = currentCoins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            nextLevel = "Boss 1";
        }
    }

    public IEnumerator levelEnd()
    {
        AudioManager.instance.PlayLevelWin();

        PlayerController.instance.canMove = false;

        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        Tracker.instance.currentCoins = currentCoins;
        Tracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        Tracker.instance.maxHealth= PlayerHealthController.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }

    public void PauseUnPause()
    {
        if (!isPause)
        {
            UIController.instance.pauseMenu.SetActive(true);

            isPause = true;

            Time.timeScale = 0f;
        } else
        {
            UIController.instance.pauseMenu.SetActive(false);

            isPause = false;

            Time.timeScale = 1f;
        }
    }

    public void GetCoints(int amount)
    {
        currentCoins += amount;

        UIController.instance.coinText.text = currentCoins.ToString();
    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.instance.coinText.text = currentCoins.ToString();
    }
}
