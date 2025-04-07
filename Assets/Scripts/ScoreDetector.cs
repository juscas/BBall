using UnityEngine;

public class ScoreDetector : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ball")
        {
            _scoreManager.AddToScore(1);
        }
    }
}
