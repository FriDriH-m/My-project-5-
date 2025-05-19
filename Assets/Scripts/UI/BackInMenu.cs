using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackInMenu : MonoBehaviour
{
    [SerializeField] Button _ExitButton;
    public PlayerDamage _Deaths;
    public DataAchievement Icon1;
    private void Awake()
    {
        _ExitButton.onClick.AddListener(AfterClickExit);
    }

    public void AfterClickExit()
    {
        if (Icon1._unlocked == false)
            if (_Deaths.Deaths == 0)
            {
                Icon1._unlocked = true;
            }
        SceneManager.LoadScene("MainMenuScene");
    }
}
