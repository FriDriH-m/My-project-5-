using UnityEngine;

public class HitZone : MonoBehaviour
{
    public enum ZoneType { Limbs, Torso, Head, Sword, Shield };
    public ZoneType zone;
    public bool haveArmor = false;
}
