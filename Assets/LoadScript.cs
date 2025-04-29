using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private float _minLoadTime = 2f; // Минимальное время показа картинки (сек)

    private void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        // Ждём минимальное время (чтобы картинка не мелькала)
        yield return new WaitForSeconds(_minLoadTime);

        // Начинаем загрузку основной сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleScene");
        asyncLoad.allowSceneActivation = false;

        // Ждём завершения загрузки
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Добавляем небольшую задержку для плавности
                yield return new WaitForSeconds(0.5f);
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}