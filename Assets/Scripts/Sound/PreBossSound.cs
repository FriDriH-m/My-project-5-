using UnityEngine;

public class PreBossSound : MonoBehaviour
{
    private AudioSource audioSource;
    private float fadeDuration = 6f;
    private float fadeTimer = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        enabled = false; // Выключаем скрипт в начале
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        float volume = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
        audioSource.volume = volume;

        if (fadeTimer >= fadeDuration)
        {
            audioSource.Stop();
            enabled = false; // Выключаем скрипт после завершения
        }
    }

    // Метод для запуска затухания
    public void StartFadeOut()
    {
        fadeTimer = 0f;
        enabled = true; // Включаем Update
    }

}
