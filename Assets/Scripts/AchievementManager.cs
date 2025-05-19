// AchievementManager.cs
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    // Массив всех ScriptableObject с достижениями
    public DataAchievement[] allAchievements;

    // Метод для разблокировки по индексу (0-15)
    public void UnlockAchievement(int achievementIndex)
    {
        if (achievementIndex >= 0 && achievementIndex < allAchievements.Length)
        {
            if (!allAchievements[achievementIndex]._unlocked)
            {
                allAchievements[achievementIndex]._unlocked = true;
                Debug.Log($"Достижение {achievementIndex + 1} разблокировано!");
                SaveAchievements(); // Сохраняем прогресс
            }
        }
        else
        {
            Debug.LogError("Неверный индекс достижения!");
        }
    }

    // Сохранение прогресса в PlayerPrefs
    private void SaveAchievements()
    {
        for (int i = 0; i < allAchievements.Length; i++)
        {
            PlayerPrefs.SetInt($"Achievement_{i}", allAchievements[i]._unlocked ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    // Загрузка прогресса при старте игры
    private void Start()
    {
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        for (int i = 0; i < allAchievements.Length; i++)
        {
            allAchievements[i]._unlocked = PlayerPrefs.GetInt($"Achievement_{i}", 0) == 1;
        }
    }
}