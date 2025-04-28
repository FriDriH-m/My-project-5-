using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Unity.VisualScripting;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] GameObject train;
    [SerializeField] GameObject hint;
    [SerializeField] Vector3 _offset;
    [SerializeField] LayerMask _layerMask;
    ConfigurableJoint configurableJoint;
    Rigidbody rb;
    GameObject joint;

    Transform interactable;
    Transform interactor;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _offset;
    }
    private void OnDrawGizmos()
    {
        rb = GetComponent<Rigidbody>();
        Gizmos.DrawSphere(transform.TransformPoint(rb.centerOfMass), 0.1f);

    }
    public void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
       
        interactable.SetParent(interactor);
        rb = interactable.GetComponent<Rigidbody>();
        

        if (gameObject.CompareTag("Start_Weapon"))
        {
            train.SetActive(true);
        }
        if (gameObject.CompareTag("Secret_Weapon"))
        {
            hint.SetActive(true);
        }
        //ConfigurableJoint haveJoint = interactable.GetComponent<ConfigurableJoint>();
        ////настройка Joint
        //joint = new GameObject("Joint");
        //joint.AddComponent<Rigidbody>();
        //joint.GetComponent<Rigidbody>().isKinematic = true;
        //joint.GetComponent<Rigidbody>().mass = 0.0000001f;
        //joint.transform.SetParent(interactor); // joint становится дочерним
        //joint.transform.localPosition = Vector3.zero; // Локальные координаты относительно родителя
        //joint.transform.localRotation = Quaternion.identity;
        ////конец настройки joint

        //interactable.GetComponent<Rigidbody>().isKinematic = false; // меч настраивается
        //interactable.GetComponent<Rigidbody>().useGravity = false;

        //transform.SetParent(args.interactorObject.transform); // меч становится дочерним руке

        ////настройка компонента Configurable Joint
        //if (haveJoint != null) 
        //{
        //    Destroy(haveJoint);
        //}
        //interactable.AddComponent<ConfigurableJoint>();
        //configurableJoint = interactable.GetComponent<ConfigurableJoint>();

        //configurableJoint.xMotion = ConfigurableJointMotion.Limited;
        //configurableJoint.zMotion = ConfigurableJointMotion.Limited;
        //configurableJoint.yMotion = ConfigurableJointMotion.Limited;

        //configurableJoint.autoConfigureConnectedAnchor = false;

        //configurableJoint.linearLimit = new SoftJointLimit
        //{
        //    limit = 1,
        //    contactDistance = 0.1f
        //};

        //configurableJoint.xDrive = new JointDrive
        //{
        //    positionSpring = 10000,
        //    positionDamper = 1
        //};
        //configurableJoint.zDrive = new JointDrive
        //{
        //    positionSpring = 10000,
        //    positionDamper = 1
        //}; 
        //configurableJoint.yDrive = new JointDrive
        //{
        //    positionSpring = 10000,
        //    positionDamper = 1
        //};

        //configurableJoint.rotationDriveMode = RotationDriveMode.Slerp;

        //configurableJoint.slerpDrive = new JointDrive
        //{
        //    positionSpring = 2000,
        //    positionDamper = 10
        //};

        //configurableJoint.connectedBody = joint.GetComponent<Rigidbody>();
        ////конец настройка компонента Configurable Joint
    }

    public void OnUngrab(SelectExitEventArgs args)
    {
        //Destroy(joint);
        //Destroy(interactable.GetComponent<ConfigurableJoint>());
        transform.SetParent(null);
    }
}

