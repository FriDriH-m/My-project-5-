using System.Collections;
using UnityEngine;

public class FireActive : MonoBehaviour
{
    PlayerDamage _playerDamage;
    Coroutine _coroutine;

    [SerializeField] private GameObject _canvasToDisable;
    [SerializeField] private Collider _colliderToDisable;
    private void Start()
    {
        _playerDamage = FindFirstObjectByType<PlayerDamage>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("R_Hand") || other.CompareTag("L_Hand")) && _coroutine == null)
        {
            _coroutine = StartCoroutine(FireActivation());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("R_Hand") || other.CompareTag("L_Hand")) && _coroutine == null)
        {
            StopCoroutine(FireActivation());
            _coroutine = null;
        }
    }

    private IEnumerator FireActivation()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        _playerDamage.reviveCoordination = transform.position + new Vector3(1,0,0);

        if (_canvasToDisable != null)
            _canvasToDisable.SetActive(false);
        if (_colliderToDisable != null)
            _colliderToDisable.enabled = false;
        _coroutine = null;
    }
}
