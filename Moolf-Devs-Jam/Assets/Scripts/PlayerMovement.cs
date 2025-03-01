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
        // Zeminde olup olmadığımızı kontrol et
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Yatay ve dikey hareket girişlerini al
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Hareket vektörünü hesapla
        Vector3 move = transform.right * x + transform.forward * z;
        bool isMoving = move.magnitude > walkingSoundThreshold;

        // Karakteri hareket ettir
        controller.Move(move * speed * Time.deltaTime);

        // Zıplama kontrolü
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Zıplama gerçekleşti!"); // Zıplama gerçekleştiğinde konsola log yaz
        }

        // Yerçekimini uygula
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Yürüyüş sesini oynat veya durdur
        if (isMoving && isGrounded)
        {
            if (!isWalking)
            {
                // Yürüyüş sesi oynatılacak
                walkingSound.Play();
                isWalking = true;
            }
            lastMoveTime = Time.time;
        }
        else
        {
            if (isWalking)
            {
                // Yürüyüş sesi durdurulacak
                walkingSound.Stop();
                isWalking = false;
            }
        }
    }
}