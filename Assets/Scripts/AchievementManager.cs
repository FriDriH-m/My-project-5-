using UnityEngine;
using UnityEngine.UI;
public class AchievementManager : MonoBehaviour
{
    public DataAchievement[] JustIcons;
    public DataAchievement[] SpecialyIcons;
    public Image[] JustImages;
    public GameObject[] SpecialyImages;
    int i = 0;
    private void Start()
    {
        foreach (DataAchievement icon in JustIcons)
        {
            if (icon._unlocked == true)
            {
                JustImages[i].color = new Color('F', 'F', 'F', 'F');
                JustImages[i + 1].color = new Color('F', 'F', 'F', 'F');
            }
            i += 2;
        }
        i = 0;
        foreach (DataAchievement icon in SpecialyIcons)
        {
            if (icon._unlocked == true)
            {
                SpecialyImages[i].SetActive(false);
            }
            i += 1;
        }
        i = 0;
    }
}
