using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSfxHandler : MonoBehaviour
{
    [Header("Mixers")]
    public AudioMixer audioMixer;

    [Header("Audio sources")]
    public AudioSource tiresScreeachingAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource carHitAudioSource;
    public AudioSource carJumpAudioSource;
    public AudioSource carJumpLandingAudioSource;

    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    OverDriveDriftController overDriveDriftController;

    void Awake()
    {
        overDriveDriftController = GetComponentInParent<OverDriveDriftController>();
    }

    void Start()
    {
        audioMixer.SetFloat("SFXVolume", 0.5f);
    }

    void Update()
    {
        UpdateEngineSFX();
        UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        //Handle engine SFX
        float velocityMagnitude = overDriveDriftController.GetVelocityMagnitude();

        //Increase the engine volume as the car goes faster
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        //But keep a minimum level so it playes even if the car is idle
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);

        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        //To add more variation to the engine sound we also change the pitch
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 2f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTiresScreechingSFX()
    {
        //Handle tire screeching SFX
        if (overDriveDriftController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            //If the car is braking we want the tire screech to be louder and also change the pitch.
            if (isBraking)
            {
                tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                //If we are not braking we still want to play this screech sound if the player is drifting.
                tiresScreeachingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        //Fade out the tire screech SFX if we are not screeching. 
        else tiresScreeachingAudioSource.volume = Mathf.Lerp(tiresScreeachingAudioSource.volume, 0, Time.deltaTime * 10);
    }
    
    public void PlayJumpSfx()
    {
        carJumpAudioSource.Play();
    }

    public void PlayLandingSfx()
    {
        carJumpLandingAudioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Get the relative velocity of the collision
        float relativeVelocity = collision2D.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = volume;

        if (!carHitAudioSource.isPlaying)
            carHitAudioSource.Play();
    }



}
