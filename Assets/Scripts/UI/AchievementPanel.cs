using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanel : MonoBehaviour
{
    [SerializeField] private GameObject achievementPanel;

    private void Start()
    {
        var allAchievements = Resources.LoadAll<DataAchivement>("");
        
        foreach (var achivement in allAchievements)
        {
            if (achivement.Unlocked)
            {
                // если достижение получено, то делается что то
            }            
        }
    }     
}
