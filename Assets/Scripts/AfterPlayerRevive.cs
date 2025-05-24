using UnityEngine;

public class AfterPlayerRevive : MonoBehaviour
{
    private Vector3 _startCoordination;
    private Quaternion _startRotation;
    private PlayerDamage _playerDamage;

    private void Start()
    {
        _startCoordination = transform.position;
        _startRotation = transform.rotation;
        _playerDamage = FindFirstObjectByType<PlayerDamage>();
    }
    private void Update()
    {
        if (_playerDamage.hitPoints <= 0)
        {
            transform.position = _startCoordination;
            transform.rotation = _startRotation;
        }
    }
}
