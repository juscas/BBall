using TMPro;
using UnityEngine;

public class ScoreUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager.OnScoreChanged += UpdateUI;
    }

    private void OnDestroy()
    {
        _scoreManager.OnScoreChanged -= UpdateUI;
    }

    private void UpdateUI(int score)
    {
        _scoreText.text = score.ToString();
    }
}
