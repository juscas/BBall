using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHandler : MonoBehaviour
{
    [SerializeField] private float _grabRadius;
    [SerializeField] private Transform _grabParent;

    private InputMaster _inputMaster;
    
    private void Awake()
    {
        _inputMaster = new InputMaster();
        _inputMaster.BasketBall.Grab_R.performed += OnRightHandGrabbed;
        _inputMaster.BasketBall.Grab_L.performed += OnLeftHandGrabbed;
    }

    private void OnDestroy()
    {
        _inputMaster.BasketBall.Grab_R.performed -= OnRightHandGrabbed;
        _inputMaster.BasketBall.Grab_L.performed -= OnLeftHandGrabbed;
    }

    private void OnLeftHandGrabbed(InputAction.CallbackContext obj)
    {
        Debug.Log("Left hand grabbed.");   
    }

    private void OnRightHandGrabbed(InputAction.CallbackContext obj)
    {
        Debug.Log("Right hand grabbed.");
    }

    private void FixedUpdate()
    {
        var colliders = Physics.OverlapSphere(transform.position, _grabRadius);

        if (colliders.Length != 0)
        {
            var collider = colliders.FirstOrDefault(x => LayerMask.LayerToName(x.gameObject.layer) == "Ball");
        }
    }

    private void Grab(GameObject gameobject)
    {
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        
        gameObject.transform.SetParent(_grabParent);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _grabRadius);
    }
}
