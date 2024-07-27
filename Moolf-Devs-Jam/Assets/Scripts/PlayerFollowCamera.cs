using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float bumpIntensity = 0.1f;
    public float bumpSpeed = 2f;

    void LateUpdate()
    {
        transform.position = player.position + offset;

        // Bumping effect
        if (player.GetComponent<CharacterController>().velocity.magnitude > 0)
        {
            float bumpOffset = Mathf.Sin(Time.time * bumpSpeed) * bumpIntensity;
            transform.localPosition = new Vector3(transform.localPosition.x, offset.y + bumpOffset, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, offset.y, transform.localPosition.z);
        }
    }
}
