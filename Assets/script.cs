using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _StartButton;
    [SerializeField] Button _AchievementButton;
    [SerializeField] Button _ExitButton;
    [SerializeField] GameObject _AchievementPanel;
    [SerializeField] GameObject _MainMenuPanel;
    public void AfterClickAchievement()
    {

        _AchievementPanel.SetActive(!_AchievementPanel.activeInHierarchy);
        _MainMenuPanel.SetActive(!_MainMenuPanel.activeInHierarchy);
    }

    private void Awake()
    {
        _AchievementButton.onClick.AddListener(AfterClickAchievement);
        _StartButton.onClick.AddListener(AfterClickStart);
        _ExitButton.onClick.AddListener(AfterClickExit);
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
