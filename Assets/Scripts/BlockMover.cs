using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockMover : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3f; // Speed of horizontal movement
    [SerializeField] float direction = -1f; // Movement direction (1 for right, -1 for left)
    [SerializeField] float boundary = 3f; // The maximum distance the block can move from the center

    void Update()
    {
        // Move the block horizontally
        transform.Translate(Vector3.forward * moveSpeed * direction * Time.deltaTime);

        // Reverse direction if the block reaches the boundary
        if (transform.position.x > boundary || transform.position.x < -boundary)
        {
            direction *= -1f; // Flip the direction
        }
    }
}

