using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns a new prefab once this transform is empty
/// </summary>
public class AutoSpawnInTransform : MonoBehaviour
{
    public GameObject PrefabToSpawn;
    public float RespawnDelay = 0.5f;
    public float RespawnSpeed = 0.5f;
    public Vector3 SpawnOffset;
    private bool RespawnInProgress = false;

    private void OnEnable()
    {
        RespawnInProgress = false;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (transform.childCount == 0 && !RespawnInProgress)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        RespawnInProgress = true;
        yield return new WaitForSeconds(RespawnDelay);

        if (transform.childCount == 0)
        {
            Instantiate(PrefabToSpawn, transform);
        }

        // Move from offset to zero
        // Scale up from zero to one
        float spawnProgress = 0;
        while (spawnProgress < 1)
        {
            spawnProgress += RespawnSpeed * Time.deltaTime;
            transform.localPosition = Mathf.Max(0, (1 - spawnProgress)) * SpawnOffset;
            transform.localScale = Mathf.Min(1, spawnProgress) * Vector3.one;
            yield return null;
        }
        RespawnInProgress = false;
    }
}
