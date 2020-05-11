using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class SpawnOnSpatialMesh : MonoBehaviour, IMixedRealityPointerHandler
{
    public Transform ParentTransform;
    public GameObject PrefabToSpawn;
    public LayerMask LayerMask;
    public float AngleDegreesThreshold = 15f;
    public Vector3 Offset;

    private void OnEnable()
    {
        MixedRealityToolkit.Instance.GetService<IMixedRealityInputSystem>().RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    private void OnDisable()
    {
        if (MixedRealityToolkit.Instance && MixedRealityToolkit.Instance.HasActiveProfile)
        {
            MixedRealityToolkit.Instance.GetService<IMixedRealityInputSystem>()?.UnregisterHandler<IMixedRealityPointerHandler>(this);
        }
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (PrefabToSpawn != null)
        {
            var result = eventData.Pointer.Result;

            // Filter based on angle with up
            if (Vector3.Angle(result.Details.Normal, Vector3.up) < AngleDegreesThreshold)
            {
                // Filter based on layermask
                if (((1 << result.Details.Object.layer) & LayerMask) != 0)
                {
                    Instantiate(PrefabToSpawn, result.Details.Point + Offset, OrientToCamera(result.Details.Point), ParentTransform);

                    eventData.Use();
                }
            }
        }
    }

    private Quaternion OrientToCamera(Vector3 position)
    {
        Vector3 directionToTarget = Camera.main.transform.position - position;

        directionToTarget.y = 0.0f;

        if (directionToTarget.sqrMagnitude < 0.001f)
        {
            return Quaternion.identity;
        }

        return Quaternion.LookRotation(-directionToTarget);
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }
}
