using UnityEngine;
using UnityEngine.XR;
public class BowScript : MonoBehaviour
{    
    Animator animator; // аниматор лука
    public bool isGrabbed; // взял ли игрок лук в руки. в GrabParenter задается true
    [SerializeField] GameObject tetiva; // тетива лука
    [SerializeField] GameObject arrow; // префаб стрелы чтобы создавать 
    GameObject spawnedArrow; // созданная стрела
    Rigidbody rb; // ригидбади стрелы
    private bool _wasPressed = false;
    private void Start()
    {
        animator = transform.GetComponent<Animator>();
    }    
    private void Update()
    {
        if (isGrabbed) // если лук в руках
        {
            if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed)) // твоя фигня. с скрипта inventoryVR
            {
                if (isPressed && !_wasPressed)
                {
                    animator.SetBool("Shoot", true);
                    spawnedArrow = Instantiate(arrow, transform.position, Quaternion.identity); // создается стрела, (префаб, позиция спавна, поворот)
                    rb = spawnedArrow.GetComponent<Rigidbody>();
                    spawnedArrow.transform.SetParent(transform); // задается родитель в виде тетивы
                }

                if (!isPressed)
                {
                    _wasPressed = false;
                }
                else
                {
                    _wasPressed = true;
                }
            }
        }
    }
    private void EndShoot() // animation event в конце анимации натягивания тетивы 
    {
        animator.SetBool("Shoot", false);
        rb.linearVelocity += transform.forward * 5; // задается скорость стреле
        Debug.Log("Working");
    }
}
// по идее все должно сработать, но я уверен что некоторые мелочи не учел или какие то моменты с позиционированием.