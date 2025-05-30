using UnityEngine;
using System.Collections.Generic;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> UnspawnObjescts = new List<GameObject>();
    private void Awake()
    {
        foreach (GameObject obj in spawnObjects)
        {
            obj.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject obj in spawnObjects)
            {
                obj.SetActive(true);
            }
            foreach (GameObject obj in UnspawnObjescts)
            {
                obj.SetActive(false);
            }
        }
    }
}
