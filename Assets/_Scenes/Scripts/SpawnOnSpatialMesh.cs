using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Linq;
using UnityEngine;

public class SpawnOnSpatialMesh : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
{
    public GameObject PrefabToSpawn;
    public LayerMask LayerMask;
    public float AngleDegreesThreshold = 15f;

    private void Awake()
    {
        MixedRealityToolkit.Instance.GetService<IMixedRealityInputSystem>().RegisterHandler<IMixedRealityPointerHandler>(this);
        MixedRealityToolkit.Instance.GetService<IMixedRealityInputSystem>().RegisterHandler<IMixedRealityTouchHandler>(this); 
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
                    Instantiate(PrefabToSpawn, result.Details.Point, Quaternion.LookRotation(result.Details.Normal));

                    eventData.Use();
                }
            }
        }
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

    private void Update()
    {

        var hand = HandJointUtils.FindHand(Microsoft.MixedReality.Toolkit.Utilities.Handedness.Any);
        //var nearPointers = PointerUtils.GetPointers<IMixedRealityNearPointer>().ToList();

        //foreach (var pointer in nearPointers)
        //{
        //    if (pointer.IsNearObject)
        //    {
        //        float distance; 
        //        if (pointer.TryGetDistanceToNearestSurface(out distance))
        //        {
        //            Debug.Log(pointer.PointerName + "  " + distance);
        //        }
        //    }
        //}
    }

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        
    }

    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        
    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {
        
    }
}
