using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GrabParenter : MonoBehaviour
{
    [SerializeField] GameObject train;
    [SerializeField] GameObject hint;

    public void OnGrab(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.SetParent(args.interactorObject.transform);
        if (args.interactableObject.transform.gameObject.CompareTag("Start_Weapon"))
        {
            train.SetActive(true);
        }
        if (args.interactableObject.transform.gameObject.CompareTag("Secret_Weapon"))
        {
            hint.SetActive(true);
        }
    }
    public void OnUngrab(SelectExitEventArgs args)
    {
        args.interactableObject.transform.SetParent(null);
    }
}
