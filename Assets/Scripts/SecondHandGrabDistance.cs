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

    public Transform _firstInteractor = null;
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if(TwoHandGrabbing)
        {
            if (_firstInteractor == null)
            {
                Debug.Log("_firstInteractor - null");
                return base.IsSelectableBy(interactor);
            }
            //Debug.Log(Vector3.Distance(interactor.transform.position, _secondAttachPoint.position));
            if (interactor.transform != _firstInteractor)
            {
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
