using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private Vector3 velocity;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public AudioSource walkingSound;
    public float walkingSoundThreshold = 0.1f;
    private float lastMoveTime = 0f;
    private bool isWalking = false;

    void Update()
    {
        // Zeminde olup olmad���m�z� kontrol et
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Yatay ve dikey hareket giri�lerini al
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Hareket vekt�r�n� hesapla
        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > walkingSoundThreshold;

        // Karakteri hareket ettir
        controller.Move(move * speed * Time.deltaTime);

        // Z�plama kontrol�
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Z�plama ger�ekle�ti!"); // Z�plama ger�ekle�ti�inde konsola log yaz
        }

        // Yer�ekimini uygula
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Y�r�y�� sesini oynat veya durdur
        if (isMoving && isGrounded)
        {
            if (!isWalking)
            {
                // Y�r�y�� sesi oynat�lacak
                walkingSound.Play();
                isWalking = true;
            }
            lastMoveTime = Time.time;
        }
        else
        {
            if (isWalking)
            {
                // Y�r�y�� sesi durdurulacak
                walkingSound.Stop();
                isWalking = false;
            }
        }
    }
}