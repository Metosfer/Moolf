using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;   // Oyuncunun transformu
    public Vector3 offset;    // Kamera ile oyuncu arasýndaki mesafe

    private void Update()
    {
        if (player != null)
        {
            // Kamerayý oyuncunun konumuna göre konumlandýr
            transform.position = player.position + offset;
        }
    }
}
