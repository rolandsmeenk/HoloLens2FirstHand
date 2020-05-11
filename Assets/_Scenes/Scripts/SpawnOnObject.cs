using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class SpawnOnObject : MonoBehaviour
{
    public GameObject PrefabToSpawn;

    public void Spawn(MixedRealityPointerEventData eventData)
    {
        if (PrefabToSpawn != null)
        {
            var result = eventData.Pointer.Result;
            Instantiate(PrefabToSpawn, result.Details.Point, Quaternion.identity);
        }
    }
}

