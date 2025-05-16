using UnityEngine;

[CreateAssetMenu(fileName = "DataAchivement", menuName = "Data/Achievement")]
public class DataAchivement : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private bool _unlocked;
    public string Name => _name;
    public string Description => _description;
    public bool Unlocked => _unlocked;
}
