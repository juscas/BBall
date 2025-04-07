using System;
using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager.OnScoreChanged += UpdateUI;
        _scoreManager.OnNewHighScore += UpdateHighScore;
    }

    private void OnDestroy()
    {
        _scoreManager.OnScoreChanged -= UpdateUI;
        _scoreManager.OnNewHighScore -= UpdateHighScore;
    }

    private void UpdateUI(int score)
    {
        _scoreText.text = score.ToString();
    }
    
    private void UpdateHighScore(int highScore)
    {
        _highScoreText.text = "High Score: " + highScore;
    }

    private void Update()
    {
        var time = TimeSpan.FromSeconds(_scoreManager.CurrentTime);
        _timerText.text = $"{time.TotalMinutes:00}:{time.TotalSeconds:00}";
    }
}
