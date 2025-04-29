using UnityEngine;
using UnityEngine.EventSystems;
public class AudioUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioClip _hoverSound;
    [SerializeField] AudioSource _audioSource;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!this.enabled) return;
        _audioSource.PlayOneShot(_hoverSound);
    }
}

