using System.Collections;
using UnityEngine;

public class OverDriveDriftController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.85f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;

    [Header("Sprites")]
    public SpriteRenderer carSpriteRenderer;
    public SpriteRenderer carShadowRenderer;

    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    Rigidbody2D carRigidbody2D;
    Collider2D carCollider;
    CarSfxHandler carSfxHandler;
    CarSurfaceHandler carSurfaceHandler;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponentInChildren<Collider2D>();
        carSfxHandler = GetComponent<CarSfxHandler>();
        carSurfaceHandler = GetComponent<CarSurfaceHandler>();
    }

    void Start()
    {
        rotationAngle = transform.rotation.eulerAngles.z;
    }

    void FixedUpdate()
    {
        if (GameManager.instance == null || GameManager.instance.GetGameState() != GameStates.running)
        {
            FreezeCarPhysics();
            return;
        }

        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    void FreezeCarPhysics()
    {
        carRigidbody2D.linearVelocity = Vector2.zero;
        carRigidbody2D.angularVelocity = 0;
        carRigidbody2D.Sleep();
    }

    void ApplyEngineForce()
    {
        carRigidbody2D.linearDamping = accelerationInput == 0 ? Mathf.Lerp(carRigidbody2D.linearDamping, 3.0f, Time.fixedDeltaTime * 3)
                                                              : Mathf.Lerp(carRigidbody2D.linearDamping, 0, Time.fixedDeltaTime * 10);

        switch (GetSurface())
        {
            case Surface.SurfaceTypes.Sand:
                carRigidbody2D.linearDamping = Mathf.Lerp(carRigidbody2D.linearDamping, 9.0f, Time.fixedDeltaTime * 3);
                break;
            case Surface.SurfaceTypes.Grass:
                carRigidbody2D.linearDamping = Mathf.Lerp(carRigidbody2D.linearDamping, 10.0f, Time.fixedDeltaTime * 3);
                break;
            case Surface.SurfaceTypes.Oil:
                carRigidbody2D.linearDamping = 0;
                accelerationInput = Mathf.Clamp(accelerationInput, 0, 1.0f);
                break;
        }

        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.linearVelocity);

        if ((velocityVsUp > maxSpeed && accelerationInput > 0) ||
            (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) ||
            (carRigidbody2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0))
            return;

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        float turnSpeedFactor = Mathf.Clamp01(carRigidbody2D.linearVelocity.magnitude / 2);
        rotationAngle -= steeringInput * turnFactor * turnSpeedFactor;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.linearVelocity, transform.right);

        float currentDriftFactor = driftFactor;
        switch (GetSurface())
        {
            case Surface.SurfaceTypes.Sand:
                currentDriftFactor *= 1.05f;
                break;
            case Surface.SurfaceTypes.Oil:
                currentDriftFactor = 1.00f;
                break;
        }

        carRigidbody2D.linearVelocity = forwardVelocity + rightVelocity * currentDriftFactor;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody2D.linearVelocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        return Mathf.Abs(lateralVelocity) > 4.0f;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public float GetVelocityMagnitude() => carRigidbody2D.linearVelocity.magnitude;

    public Surface.SurfaceTypes GetSurface() => carSurfaceHandler.GetCurrentSurface();
}
