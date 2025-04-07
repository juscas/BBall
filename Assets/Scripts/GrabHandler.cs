using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabHandler : MonoBehaviour
{
    [SerializeField] private float _grabRadius;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;
    [SerializeField] private float _velocityMultiplier = 5f;
    [SerializeField] private int _velocitySamples = 5;

    private GameObject _leftHandGrabbedObject;
    private GameObject _rightHandGrabbedObject;

    private readonly List<Vector3> _leftHandCachedPositions = new();
    private readonly List<Vector3> _rightHandCachedPositions = new();

    private InputMaster _inputMaster;
    
    private void Awake()
    {
        _inputMaster = new InputMaster();
        _inputMaster.Enable();
        _inputMaster.BasketBall.Grab_R.performed += OnRightHandGrabbed;
        _inputMaster.BasketBall.Grab_R.canceled += OnRightHandReleased;
        _inputMaster.BasketBall.Grab_L.performed += OnLeftHandGrabbed;
        _inputMaster.BasketBall.Grab_L.canceled += OnLeftHandReleased;
    }

    private void OnLeftHandReleased(InputAction.CallbackContext obj)
    {
        ReleaseFromLeftHand();
    }

    private void OnRightHandReleased(InputAction.CallbackContext obj)
    {
        ReleaseFromRightHand();
    }

    private void OnDestroy()
    {
        _inputMaster.BasketBall.Grab_R.performed -= OnRightHandGrabbed;
        _inputMaster.BasketBall.Grab_R.canceled -= OnRightHandReleased;
        _inputMaster.BasketBall.Grab_L.performed -= OnLeftHandGrabbed;
        _inputMaster.BasketBall.Grab_L.canceled -= OnLeftHandReleased;
    }

    private void OnLeftHandGrabbed(InputAction.CallbackContext obj)
    {
        TryGrab(_leftHand);
    }

    private void OnRightHandGrabbed(InputAction.CallbackContext obj)
    {
        TryGrab(_rightHand);
    }

    private void TryGrab(Transform hand)
    {
        var colliders = Physics.OverlapSphere(_rightHand.position, _grabRadius);

        if (colliders.Length != 0)
        {
            var ball = colliders.FirstOrDefault(x => LayerMask.LayerToName(x.gameObject.layer) == "Ball" && x.gameObject != _leftHandGrabbedObject && x.gameObject != _rightHandGrabbedObject);
            
            if (hand == _rightHand)
            {
                if (_leftHandGrabbedObject == ball.gameObject)
                {
                    return;
                }
                _rightHandGrabbedObject = ball.gameObject;
            }
            else
            {
                if (_rightHandGrabbedObject == ball.gameObject)
                {
                    return;
                }
                _leftHandGrabbedObject = ball.gameObject;
            }
            
            if (ball != null)
            {
                Grab(ball.gameObject, hand);
            }
        }
    }

    private void Grab(GameObject grabbedObject, Transform parent)
    {
        if (grabbedObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
        
        if (grabbedObject.TryGetComponent(out BallSFXController sfxController))
        {
            sfxController.PlayGrabSound();
        }
        
        grabbedObject.transform.SetParent(parent);
        grabbedObject.transform.localPosition = Vector3.zero;
    }

    private Vector3 GetVelocityVector(List<Vector3> positions)
    {
        if (positions.Count < _velocitySamples)
        {
            return Vector3.zero;
        }
        
        var displacement = positions[^1] - positions[0];
        var time = Time.fixedDeltaTime * (positions.Count - 1);
        
        return displacement / time * _velocityMultiplier;
    }
    
    private void ReleaseFromLeftHand()
    {
        if (_leftHandGrabbedObject == null)
        {
            return;
        }
        
        if (_leftHandGrabbedObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.velocity = GetVelocityVector(_leftHandCachedPositions);
        }
        
        _leftHandGrabbedObject.transform.SetParent(null);
        _leftHandGrabbedObject = null;
    }
    
    private void ReleaseFromRightHand()
    {
        if (_rightHandGrabbedObject == null)
        {
            return;
        }
        
        if (_rightHandGrabbedObject.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.velocity = GetVelocityVector(_rightHandCachedPositions);
        }
        
        _rightHandGrabbedObject.transform.SetParent(null);
        _rightHandGrabbedObject = null;
    }

    private void FixedUpdate()
    {
        // Every physics update get positions samples for each hand
        if (_leftHandCachedPositions.Count == _velocitySamples)
        {
            _leftHandCachedPositions.RemoveAt(0);
        }
        _leftHandCachedPositions.Add(_leftHand.gameObject.transform.position);
        
        if (_rightHandCachedPositions.Count == _velocitySamples)
        {
            _rightHandCachedPositions.RemoveAt(0);
        }
        _rightHandCachedPositions.Add(_rightHand.gameObject.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _grabRadius);
    }
}
