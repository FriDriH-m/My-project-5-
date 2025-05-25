using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EndingStats : MonoBehaviour
{
    public TMP_Text _Deaths;
    public TMP_Text _DamageToNPC;
    public TMP_Text _DamageToPlayer;
    public TMP_Text _Falls;
    public TMP_Text _BowShoots;
    public TMP_Text _TimeToComplete;
    public PlayerDamage _PlayerDamage;
    public HoleScript _HoleScript;
    public BowGrabParenter _BowGrabParenter;
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float currentTime = Time.time - startTime;
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);
        _TimeToComplete.text = $"Время прохождения {minutes}:{seconds}";
        _Deaths.text = $"Смертей {_PlayerDamage.Deaths}";
        _Falls.text = $"Смертей от падения {_HoleScript.Falls/2}";
        _BowShoots.text = $"Выстрелов из лука {_BowGrabParenter.Shoots}";
    }


    public void Stats()
    {
        
    }
}
