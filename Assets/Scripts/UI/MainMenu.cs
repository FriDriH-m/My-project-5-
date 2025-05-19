using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _StartButton;
    [SerializeField] Button _AchievementButton;
    [SerializeField] Button _ExitInAchievementButton;
    [SerializeField] Button _ExitButton;
    [SerializeField] GameObject _AchievementPanel;
    [SerializeField] GameObject _MainMenuPanel;

    private void Awake()
    {
        _ExitInAchievementButton.onClick.AddListener(AfterClickAchievement);
        _AchievementButton.onClick.AddListener(AfterClickAchievement);
        _StartButton.onClick.AddListener(AfterClickStart);
        _ExitButton.onClick.AddListener(AfterClickExit);
    }
    public void AfterClickAchievement()
    {
        _AchievementPanel.SetActive(!_AchievementPanel.activeInHierarchy);
        _MainMenuPanel.SetActive(!_MainMenuPanel.activeInHierarchy);
    }

    public void AfterClickStart()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void AfterClickExit()
    {
            UnityEditor.EditorApplication.isPlaying = false;
    }
}
