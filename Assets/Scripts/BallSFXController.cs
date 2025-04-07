using UnityEngine;
using Random = UnityEngine.Random;

public class BallSFXController : MonoBehaviour
{
    [SerializeField] private AudioSource _netAudioSource;
    [SerializeField] private AudioSource _bounceAudioSource;
    [SerializeField] private AudioSource _rimAudioSource;
    [SerializeField] private Rigidbody _ballRigidbody;

    private const float PITCH_LOWER_BOUND = 0.95f;
    private const float PITCH_UPPER_BOUND = 1.05f;
    private const float MAXIMUM_VELOCITY = 10f;
    
    private void OnCollisionEnter(Collision other)
    {
        var volume = _ballRigidbody.velocity.magnitude / MAXIMUM_VELOCITY;
        var layer = LayerMask.LayerToName(other.gameObject.layer);
        
        if (layer is "Ground" or "Ball")
        {
            _bounceAudioSource.pitch = Random.Range(PITCH_LOWER_BOUND, PITCH_UPPER_BOUND);
            _bounceAudioSource.volume = volume;
            _bounceAudioSource.Play();
        }
        
        if (layer is "Rim")
        {
            _rimAudioSource.pitch = Random.Range(PITCH_LOWER_BOUND, PITCH_UPPER_BOUND);
            _rimAudioSource.volume = volume;
            _rimAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var volume = _ballRigidbody.velocity.magnitude / MAXIMUM_VELOCITY;
        
        if (LayerMask.LayerToName(other.gameObject.layer) == "Net")
        {
            _netAudioSource.pitch = Random.Range(PITCH_LOWER_BOUND, PITCH_UPPER_BOUND);
            _netAudioSource.volume = volume;
            _netAudioSource.Play();
        }
    }
}
