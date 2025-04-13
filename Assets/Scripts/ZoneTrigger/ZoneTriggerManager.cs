using Unity.VisualScripting;
using UnityEngine;

public class ZoneTriggerManager : MonoBehaviour
{
    public int top = 0;
    public int middle = 0;
    public int down = 0;
    public int left = 0;
    public int right = 0;
    public int summ = 0;

    public void SetActiveZone(string zone)
    {
        switch (zone.ToLower())
        {
            case "top": top++; break;
            case "middle": middle++; break;
            case "down": down++; break;
            case "left": left++; break;
            case "right": right++; break;
            default: Debug.LogWarning($"Unknown zone: {zone}"); break;
        }
        summ++;
    }
    public void ResetZone(string zone)
    {
        switch (zone.ToLower())
        {
            case "top": top = 0; break;
            case "middle": middle = 0; break;
            case "down": down = 0; break;
            case "left": left = 0; break;
            case "right": right = 0; break;
            default: Debug.LogWarning($"Unknown zone: {zone}"); break;
        }
        summ--;
    }

    private void Update()
    {
        Debug.Log($"top: {top} down: {down} middle: {middle} left: {left} right: {right}");
    }
}
