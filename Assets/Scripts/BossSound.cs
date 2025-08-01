using UnityEngine;

public class BossSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AgroStateB AgroState;
    private float fadeDuration = 6f;
    private float fadeTimer = 0f;
    public bool flagForSound = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        enabled = false; // ��������� ������ � ������
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        float volume = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
        audioSource.volume = volume;

        if (fadeTimer >= fadeDuration)
        {
            audioSource.Stop();
            enabled = false; // ��������� ������ ����� ����������
        }
    }

    // ����� ��� ������� ���������
    public void StartFadeOut()
    {
        if (flagForSound == false) return;
        fadeTimer = 0f;
        enabled = true; // �������� Update
        flagForSound = false;
    }
    public void AgroStateActive()
    {
        Debug.Log("���");
        if (flagForSound == false)
        {
            Debug.Log("���");
            audioSource.Play();
            audioSource.volume = 1;
            flagForSound = true;
        }
    }
}
