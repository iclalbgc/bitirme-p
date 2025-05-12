using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    OverDriveDriftController carController;

    void Awake()
    {
        carController = GetComponent<OverDriveDriftController>();
    }

    void Update()
    {
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        carController.SetInputVector(inputVector);

        // Önceden şuna benzer bir satır vardı ve hata veriyordu:
        // if (Input.GetKeyDown(KeyCode.Space)) carController.Jump(0.5f, 1.0f);
        // Artık Jump() fonksiyonu olmadığı için bu satır tamamen kaldırıldı.
    }
}
