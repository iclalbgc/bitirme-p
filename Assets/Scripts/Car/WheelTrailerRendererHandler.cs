using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    public bool isOverpassEmitter = false;


    //Components
    OverDriveDriftController overDriveDriftController;
    TrailRenderer trailRenderer;
    CarLayerHandler carLayerHandler;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        //Get the top down car controller
        overDriveDriftController = GetComponentInParent<OverDriveDriftController>();

        carLayerHandler = GetComponentInParent<CarLayerHandler>();

        //Get the trail renderer component.
        trailRenderer = GetComponent<TrailRenderer>();

        //Set the trail renderer to not emit in the start. 
        trailRenderer.emitting = false;
    }


    // Update is called once per frame
    void Update()
    {

        trailRenderer.emitting = false;

        if(overDriveDriftController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            if (carLayerHandler.IsDrivingOnOverpass() && isOverpassEmitter)
                trailRenderer.emitting = true;

            if (!carLayerHandler.IsDrivingOnOverpass() && !isOverpassEmitter)
                trailRenderer.emitting = true;
        }

    }
}