using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackInMenu : MonoBehaviour
{
    [SerializeField] Button _ExitButton;
    public TextMeshProUGUI Deaths;
    private void Awake()
    {
        _ExitButton.onClick.AddListener(AfterClickExit);
    }

    public void AfterClickExit()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
