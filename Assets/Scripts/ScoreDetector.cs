using UnityEngine;

public class ScoreDetector : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Ball")
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                if (rb.velocity.y < 0f)
                {
                    _scoreManager.AddToScore(2);
                }
            }
        }
    }
}
