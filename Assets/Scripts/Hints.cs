using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hints : MonoBehaviour
{
    [SerializeField] GameObject _hint;

    public void OnGrabHint(SelectEnterEventArgs args)
    {
        Debug.Log("≈—“‹!");
        _hint.SetActive(true);
    }
}
