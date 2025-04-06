using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Camera mainCamera; // Reference to the main camera

    public float cameraMoveUpAmount = 1f; // Distance to move the camera up after placing a block
    public float cameraMoveSpeed = 2f; // Speed of the camera's upward movement
    Vector3 targetCameraPosition; // Target position the camera should move towards

    public GameObject lastStackedBlock; // Reference to the last successfully placed block

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Initialize the singleton instance
        }
        targetCameraPosition = mainCamera.transform.position; // Set the initial camera position
    }

    void Update()
    {
        // Check for touch input to place a block
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceBlock();
        }

        // Smoothly move the camera toward the target position
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, cameraMoveSpeed * Time.deltaTime);
    }

    void PlaceBlock()
    {
        // Stop the block's movement
        Destroy(CubeSpawn.Instance.CurrentBlock.GetComponent<BlockMover>());

        // Check if the block is not placed on top of the last block
        if (lastStackedBlock != null && !IsContactingLastBlock())
        {
            Debug.Log("Game Over");
            return;
        }

        // Store current block's position and scale
        Vector3 CurrentBlockPosition = CubeSpawn.Instance.CurrentBlock.transform.position;
        Vector3 CurrentBlockLocalScale = CubeSpawn.Instance.CurrentBlock.transform.localScale;

        // Handle X-axis overlap and trimming
        float hangoverX = transform.position.x - CurrentBlockPosition.x;
        float newXSize = CurrentBlockLocalScale.x - Mathf.Abs(hangoverX);
        float newXPosition = CurrentBlockPosition.x + (hangoverX / 2);

        float directionX = hangoverX > 0 ? -1 : 1;
        float fallingBlockSizeX = CurrentBlockLocalScale.x - newXSize;
        float cubeEdgeX = CurrentBlockPosition.x + (newXSize / 2f * directionX);
        float fallingBlockPositionX = cubeEdgeX + (fallingBlockSizeX / 2f) * directionX;

        // Handle Z-axis overlap and trimming
        float hangoverZ = transform.position.z - CurrentBlockPosition.z;
        float newZSize = CurrentBlockLocalScale.z - Mathf.Abs(hangoverZ);
        float newZPosition = CurrentBlockPosition.z + (hangoverZ / 2);

        float directionZ = hangoverZ > 0 ? -1 : 1;
        float fallingBlockSizeZ = CurrentBlockLocalScale.z - newZSize;
        float cubeEdgeZ = CurrentBlockPosition.z + (newZSize / 2f * directionZ);
        float fallingBlockPositionZ = cubeEdgeZ + (fallingBlockSizeZ / 2f) * directionZ;

        // Assign the new size and position to the current block
        CubeSpawn.Instance.CurrentBlock.transform.position = new Vector3(newXPosition, CurrentBlockPosition.y, newZPosition);
        CubeSpawn.Instance.CurrentBlock.transform.localScale = new Vector3(newXSize, CurrentBlockLocalScale.y, newZSize);

        bool firstSpawnPoint = CubeSpawn.Instance.useFirstSpawnPoint;

        // Create a falling piece (leftover block) based on the direction of movement
        if (firstSpawnPoint)
        {
            var cubeX = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeX.transform.localScale = new Vector3(fallingBlockSizeX, CurrentBlockLocalScale.y, CurrentBlockLocalScale.z);
            cubeX.transform.localPosition = new Vector3(fallingBlockPositionX, CurrentBlockPosition.y, CurrentBlockPosition.z);
            cubeX.AddComponent<Rigidbody>();
        }
        else
        {
            var cubeZ = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeZ.transform.localScale = new Vector3(CurrentBlockLocalScale.x, CurrentBlockLocalScale.y, fallingBlockSizeZ);
            cubeZ.transform.localPosition = new Vector3(CurrentBlockPosition.x, CurrentBlockPosition.y, fallingBlockPositionZ);
            cubeZ.AddComponent<Rigidbody>();
        }

        // Update the stack height and reference to the last block
        CubeSpawn.Instance.stackHeight += CurrentBlockLocalScale.y;
        lastStackedBlock = CubeSpawn.Instance.CurrentBlock;

        MoveCameraUp(); // Raise the camera for the next block

        CubeSpawn.Instance.SpawnBlock(); // Spawn a new block
    }

    bool IsContactingLastBlock()
    {
        // Use colliders to detect whether the current block overlaps with the last one
        Collider currentCollider = CubeSpawn.Instance.CurrentBlock.GetComponent<Collider>();
        Collider lastCollider = lastStackedBlock.GetComponent<Collider>();

        return currentCollider.bounds.Intersects(lastCollider.bounds);
    }

    void MoveCameraUp()
    {
        if (mainCamera != null)
        {
            targetCameraPosition.y += cameraMoveUpAmount; // Adjust the target Y position of the camera
        }
        else
        {
            Debug.LogWarning("Main Camera not assigned in GameManager!");
        }
    }
}
