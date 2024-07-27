using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    public float minXRotation = -90f;
    public float maxXRotation = 90f;

    // Geri tepme parametreleri
    public float recoilX = 2f;
    public float recoilY = 2f;
    public float recoilZ = 0.35f;
    public float snappiness = 6f;
    public float returnSpeed = 2f;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

        // Geri tepme kontrolü
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        // Kamera rotasyonunu güncelle
        transform.localRotation = Quaternion.Euler(xRotation + currentRotation.x, currentRotation.y, currentRotation.z);
        playerBody.Rotate(Vector3.up * mouseX);

        // Ateþ etme kontrolü (örnek olarak)
        if (Input.GetButtonDown("Fire1"))
        {
            AddRecoil();
        }
    }

    public void AddRecoil()
    {
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}