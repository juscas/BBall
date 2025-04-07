using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Rigidbody _ballPrefab;
    [SerializeField] private Cloth _netCloth;

    [SerializeField] private List<Transform> _passPoints;
    [SerializeField] private Transform _player;

    private InputMaster _inputMaster;
    
    private void Awake()
    {
        _inputMaster = new InputMaster();
        _inputMaster.Enable();
        _inputMaster.BasketBall.SpawnBall.performed += OnSpawnBallPressed;
    }

    private void OnSpawnBallPressed(InputAction.CallbackContext obj)
    {
        SpawnBall();
    }

    [ContextMenu("SpawnBall")]
    public void SpawnBall()
    {
        var spawnPos = _passPoints[Random.Range(0, _passPoints.Count)].position;
        var ball = Instantiate(_ballPrefab, spawnPos, transform.rotation);
        ClothSphereColliderPair[] clothColliders = new ClothSphereColliderPair[1];
        clothColliders[0] = new ClothSphereColliderPair(ball.GetComponent<SphereCollider>());
        _netCloth.sphereColliders = clothColliders;

        ball.velocity = CalculateTrajectory(spawnPos,
            _player.position, Random.Range(2f, 3f), Mathf.Abs(Physics.gravity.y));
    }

    private Vector3 CalculateTrajectory(Vector3 from, Vector3 to, float parabolaHeight, float gravity)
    {
        Vector3 displacement = to - from;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        float verticalDisplacement = displacement.y;
        float horizontalDistance = displacementXZ.magnitude;
        
        float timeToHeight = Mathf.Sqrt(2 * (parabolaHeight - from.y) / gravity);
        float timeFromHeight = Mathf.Sqrt(2 * (parabolaHeight - to.y) / gravity);

        float totalTime = timeToHeight + timeFromHeight;
        Vector3 velocityXZ = displacementXZ / totalTime;
        
        float velocityY = Mathf.Sqrt(2 * gravity * (parabolaHeight - from.y));

        Vector3 result = velocityXZ + Vector3.up * velocityY;
        return result;
    }
}
