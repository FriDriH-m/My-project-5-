using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EngingUI : MonoBehaviour
{
    public GameObject _EndingCanvas;
    public DamageCount _DamageCount;
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
    private float MegaSuperTime;
    public void Start()
    {
        startTime = Time.time;
        MegaSuperTime = 0f;
        _EndingCanvas.SetActive(false);
    }
    public void EngingCanvas()
    {
        if (!_EndingCanvas.activeSelf)
        {
            _EndingCanvas.SetActive(!_EndingCanvas.activeInHierarchy);
            MegaSuperTime = Time.time - startTime;
            int minutes = (int)(MegaSuperTime / 60);
            int seconds = (int)(MegaSuperTime % 60);
            _TimeToComplete.text = $"Время прохождения {minutes}:{seconds}";
            _Deaths.text = $"Смертей {_PlayerDamage.Deaths}";
            _Falls.text = $"Смертей от падения {_HoleScript.Falls / 2}";
            _BowShoots.text = $"Выстрелов из лука {_BowGrabParenter.Shoots}";
        }
    }

    public void Update()
    {
        if (_DamageCount.hitPoints <= 0)
        {
            EngingCanvas();
            FindObjectOfType<BossSound>().StartFadeOut();
        }
    }
}


