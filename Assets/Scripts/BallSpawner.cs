using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Cloth _netCloth;

    private InputMaster _inputMaster;
    
    private void Awake()
    {
        _inputMaster = new InputMaster();
        _inputMaster.BasketBall.SpawnBall.performed += OnSpawnBallPressed;
    }

    private void OnSpawnBallPressed(InputAction.CallbackContext obj)
    {
        SpawnBall();
    }

    [ContextMenu("SpawnBall")]
    public void SpawnBall()
    {
        var ball = Instantiate(_ballPrefab, transform.position, transform.rotation);
        ClothSphereColliderPair[] clothColliders = new ClothSphereColliderPair[1];
        clothColliders[0] = new ClothSphereColliderPair(ball.GetComponent<SphereCollider>());
        _netCloth.sphereColliders = clothColliders;
    }
}
