using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] XRInputValueReader<float> m_GripInput;
    [SerializeField] XRInputValueReader<float> m_TriggerInput;
    [SerializeField] Animator animator;

    private void Update()
    {
        animator.SetFloat("Grip", m_GripInput.ReadValue());
        animator.SetFloat("Trigger", m_TriggerInput.ReadValue());
    }
}
