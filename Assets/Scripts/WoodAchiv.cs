using UnityEngine;
using System.Collections;
public class WoodAchiv : MonoBehaviour
{
    public DataAchievement Icon11;
    public DataAchievement Icon14;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("R_Hand") || other.CompareTag("L_Hand"))
        {
            Icon11._unlocked = true;
        }
        if (other.CompareTag("Weapon") || other.CompareTag("Secret_Weapon"))
        {
            Icon14._unlocked = true;
        }
    }
}
