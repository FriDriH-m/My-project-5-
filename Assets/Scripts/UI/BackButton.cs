using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WristMenuActivator : MonoBehaviour
{

    [SerializeField] private Vector2 xRange = new Vector2(-40, 40);
    [SerializeField] private Vector2 yRange = new Vector2(-70, 70);
    [SerializeField] private Vector2 zRange = new Vector2(50, 150);
    [SerializeField] private Button _ExitButtonToHand;
    [SerializeField] private GameObject wristMenu; // Кнопка
    [SerializeField] private Transform hmdTransform; // Камера

    private bool isMenuActive = false;

    public void Awake()
    {
        _ExitButtonToHand.onClick.AddListener(AfterClickExitOnHand);
    }
    public void AfterClickExitOnHand()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    private void Update()
    {
        Quaternion localRotation = Quaternion.Inverse(hmdTransform.rotation) * transform.rotation;
        Vector3 euler = localRotation.eulerAngles;

        euler.x = (euler.x > 180) ? euler.x - 360 : euler.x;
        euler.y = (euler.y > 180) ? euler.y - 360 : euler.y;
        euler.z = (euler.z > 180) ? euler.z - 360 : euler.z;

        bool isInRange =
            euler.x >= xRange.x && euler.x <= xRange.y &&
            euler.y >= yRange.x && euler.y <= yRange.y &&
            euler.z >= zRange.x && euler.z <= zRange.y;

        if (isInRange && !isMenuActive)
        {
            wristMenu.SetActive(true);
            isMenuActive = true;
        }
        else if (!isInRange && isMenuActive)
        {
            wristMenu.SetActive(false);
            isMenuActive = false;
        }
    }
}