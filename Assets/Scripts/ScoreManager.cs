using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _score;
    public int Score
    {
        get => _score;
        set {
            _score = value;
            OnScoreChanged.Invoke(_score);
        }
    }

    public event Action<int> OnScoreChanged = delegate { };

    public void AddToScore(int score)
    {
        Score += score;
    }
}
