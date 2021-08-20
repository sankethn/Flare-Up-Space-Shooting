using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    // [SerializeField]
    // private Text _highScoreText;
    [SerializeField] 
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;
    public int currentScore;
    // public int highScore = 0;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        // highScore = PlayerPrefs.GetInt("HighScore", 0);
        // _highScoreText.text = "High Score: " + highScore;

        if(_gameManager == null){
            Debug.LogError("GameManager is NULL");
        }
    }

    public void UpdateScore(){
        currentScore+=10;
        _scoreText.text = "Score: " + currentScore.ToString();
    }

    // public void CheckForHighScore(){
    //     if(currentScore > highScore){
    //         highScore = currentScore;
    //         PlayerPrefs.SetInt("HighScore", highScore);
    //         _highScoreText.text = "High Score: " + highScore;
    //     }
    // }

    public void UpdateLives(int lives){
        _livesImage.sprite = _livesSprites[lives];
    }

    public void GameOver(){
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine());
        _gameManager.GameOver();

    }

    IEnumerator FlickerRoutine(){
        while(true){
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f); 
        }
    }

}
