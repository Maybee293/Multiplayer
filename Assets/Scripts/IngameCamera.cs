using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCamera : MonoBehaviour
{
    private const float DampDuration = 0.1f;

    public static IngameCamera Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public Transform targetTransform { get; private set; }
    [SerializeField] public Camera Camera;
    [SerializeField] private AudioListener _audioListener;
    public float Distance { get; set; }

    Transform _transform;
    Vector3 _currentVelocity = Vector3.zero;
    Camera _mainCamera;
    Quaternion _quaternion;

    private void Start()
    {
        _transform = transform;
        _mainCamera = Camera.main;
        if (!_mainCamera) return;
        _mainCamera.GetComponent<AudioListener>().enabled = false;
        Distance = 5;
    }

    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            return;
        }

        var _targetPosXZ = targetTransform.position;
        var pos = _targetPosXZ + _quaternion * Vector3.forward * -Distance;
        var nextPos = Vector3.SmoothDamp(_transform.position, pos, ref _currentVelocity, DampDuration);
        _transform.SetPositionAndRotation(nextPos, _quaternion);

    }
    public void SetTargetTransform(Transform t)
    {
        if (targetTransform == null)
            targetTransform = t;
    }
}