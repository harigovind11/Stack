using UnityEngine;

public class BlockMover : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3f; // Speed of movement
    [SerializeField] float boundary =7f; // The maximum distance from the center where the block reverses direction

    private float direction = 1f; // Current movement direction (1 for forward, -1 for backward)

    void Update()
    {
        // Determine movement direction based on spawn point
        bool firstSpawnPoint = GameManager.instance.useFirstSpawnPoint;

        // Movement vector based on the spawn point
        Vector3 movement = (firstSpawnPoint ? Vector3.right : Vector3.back) * moveSpeed * direction * Time.deltaTime;

        // Move the block
        transform.Translate(movement);

        // Reverse direction when hitting boundaries
        if ((firstSpawnPoint && Mathf.Abs(transform.position.x) > boundary) ||
            (!firstSpawnPoint && Mathf.Abs(transform.position.z) > boundary))
        {
            direction *= -1f; // Reverse direction
        }
    }
}
