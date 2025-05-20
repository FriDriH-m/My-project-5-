using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Hints : MonoBehaviour
{
    [SerializeField] GameObject _hint;
    public DataAchievement Icon6;

    public void OnGrabHint(SelectEnterEventArgs args)
    {
        Icon6._unlocked = true;
        _hint.SetActive(true);
    }
}
