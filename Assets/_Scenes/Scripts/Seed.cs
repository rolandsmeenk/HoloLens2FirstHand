using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Transform SeedTransform;
    public AudioSource SeedAudio;
    public float SeedSpeed = 1f;
    public float GrowSpeed = 1f;
    public Renderer GrowthRenderer;
    public LayerMask LayerMask;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        GrowthRenderer.enabled = false;
    }

    public void Activate()
    {
        _rigidBody = gameObject.AddComponent<Rigidbody>();

        _rigidBody.mass = 0.01f;
        _rigidBody.drag = 5;
    }

    // Detached the object from it's current parent (eg. hand menu)
    public void SwitchToWorld()
    {
        transform.parent = null;
    }

    // Detect collision with spatialmesh
    private void OnCollisionEnter(Collision collision)
    {
        if (_rigidBody != null  && ((1 << collision.gameObject.layer) & LayerMask) != 0)
        {
            // Disable rigid body and collider
            _rigidBody.isKinematic = true;
            this.GetComponent<Collider>().enabled = false;

            this.transform.rotation = Quaternion.identity;

            StartCoroutine(Appear());
        }
    }

    private IEnumerator Appear()
    {
        SeedAudio.Play();

        // Move down seed
        float seedProgress = 0;
        while (seedProgress < 1)
        {
            seedProgress += SeedSpeed * Time.deltaTime;
            SeedTransform.localPosition = new Vector3(0, -seedProgress, 0);
            yield return null;
        }
        SeedTransform.gameObject.SetActive(false);

        GrowthRenderer.material.SetFloat("_Offset", 0);
        GrowthRenderer.enabled = true;

        // Let shader do growth animation
        float growthProgress = 0;
        while (growthProgress < 1)
        {
            growthProgress += GrowSpeed * Time.deltaTime;
            GrowthRenderer.material.SetFloat("_Offset", growthProgress);
            yield return null;
        }
    }
}
