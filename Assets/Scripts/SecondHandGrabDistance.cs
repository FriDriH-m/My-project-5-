using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SecondHandGrabDistance : XRGrabInteractable
{
    [SerializeField] private float _maxGrabDistance = 0.15f;
    [SerializeField] private Transform _secondAttachPoint;
    [SerializeField] private bool TwoHandGrabbing;

    public Transform handModel = null;

    private IXRSelectInteractor _firstInteractor = null;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (_firstInteractor == null && TwoHandGrabbing)
        {
            _firstInteractor = args.interactorObject;
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (args.interactorObject == _firstInteractor && TwoHandGrabbing)
        {
            _firstInteractor = null;
        }
    }
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if(TwoHandGrabbing)
        {
            if (_firstInteractor == null)
            {
                return base.IsSelectableBy(interactor);
            }
            //Debug.Log(Vector3.Distance(interactor.transform.position, _secondAttachPoint.position));
            if (interactor != _firstInteractor)
            {
                //Transform[] allChildrens = interactor.transform.GetComponentsInChildren<Transform>();
                //if (handModel == null)
                //{
                //    foreach (Transform child in allChildrens)
                //    {
                //        if (child.CompareTag("R_Hand") || child.CompareTag("L_Hand"))
                //        {
                //            handModel = child;
                //        }
                //    }
                //}
                Debug.Log(Vector3.Distance(handModel.position, _secondAttachPoint.position) + " - " + handModel.name);

                float distance = Vector3.Distance(handModel.position, _secondAttachPoint.position);

                if (distance > _maxGrabDistance)
                {
                    return false;
                }
                else
                {
                    return base.IsSelectableBy(interactor);
                }
            }
            return base.IsSelectableBy(interactor);
        }
        else return base.IsSelectableBy(interactor);
    }
}
