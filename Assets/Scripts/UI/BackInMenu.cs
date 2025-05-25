using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BackInMenu : MonoBehaviour
{
    [SerializeField] Button _ExitButton;
    public FireActive _Fire;
    public PlayerDamage _Deaths;
    public DataAchievement Icon1;
    public DataAchievement Icon2;
    public DataAchievement Icon5;
    private void Awake()
    {
        _ExitButton.onClick.AddListener(AfterClickExit);
    }
    
    public void AfterClickExit()
    {
        if (Icon5._unlocked == false)
            if (_Fire.Fire == false)
            {
                Icon5._unlocked = true;
            }
        if (Icon2._unlocked == false)
            if (_Deaths.Damage == false)
            {
                Icon2._unlocked = true;
            }
        if (Icon1._unlocked == false)
            if (_Deaths.Deaths == 0)
            {
                Icon1._unlocked = true;
            }
        SceneManager.LoadScene("MainMenuScene");
    }
}
