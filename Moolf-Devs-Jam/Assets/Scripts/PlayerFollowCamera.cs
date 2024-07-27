using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player;   // Oyuncunun transformu
    public Vector3 offset;    // Kamera ile oyuncu aras�ndaki mesafe

    private void Update()
    {
        if (player != null)
        {
            // Kameray� oyuncunun konumuna g�re konumland�r
            transform.position = player.position + offset;
        }
    }
}
