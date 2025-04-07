using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private AudioSource _whistleAudioSource;
    
    private int _score;
    public int Score
    {
        get => _score;
        set {
            _score = value;
            OnScoreChanged.Invoke(value);
        }
    }

    private int _highestScore;
    public int HighestScore => _highestScore;

    private float _sessionTime = 30f;
    private float _currentTime;
    public float CurrentTime => _currentTime;

    public event Action<int> OnScoreChanged = delegate { };
    public event Action<int> OnNewHighScore = delegate { };

    public void AddToScore(int score)
    {
        Score += score;
    }

    private void Update()
    {
        if (_currentTime >= _sessionTime)
        {
            _whistleAudioSource.Play();
            SaveScore();
            Score = 0;
            _currentTime = 0;
        }
        
        _currentTime += Time.deltaTime;
    }

    private void SaveScore()
    {
        if (_score > _highestScore)
        {
            _highestScore = _score;
            OnNewHighScore.Invoke(_highestScore);
        }
    }
}
