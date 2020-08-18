using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;

    public static GameManager Instance { set; get; }

    public bool isDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;

    //UI und die UI Felder/fields
    public Text scoreText, coinText, modifierText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    //Death menu
    public Animator deathMenuAnim;
    public Text deathscoreText, deathcoinText;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();

        modifierText.text = "x" + modifierScore.ToString("0.0");
        coinText.text = coinScore.ToString("0");
        scoreText.text = scoreText.text = score.ToString("0");



    }
    private void Update()
    {
        if(MobileInputs.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            motor.StartRunning();
            FindObjectOfType<SiteBuildingsSpawner>().isScrolling = true;

        }

        if (isGameStarted && !isDead)
        {
            //Die Punktzahl erhöhen
            
            score += (Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = score.ToString("0");
            }
            

        }
    }

    public void GetCoin()
    {
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = scoreText.text = score.ToString("0");
    }
   
    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 0.4f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0");
    }
    public void PlayBtn()
    {

    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    public void OnHomeButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void OnDeath()
    {
        isDead = true;
        if(PlayerPrefs.GetFloat("Highscore") > score)
            PlayerPrefs.SetFloat("Highscore", score);

        FindObjectOfType<SiteBuildingsSpawner>().isScrolling = false;

        deathscoreText.text = score.ToString("0");
        deathcoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Death");
       
    }
}
