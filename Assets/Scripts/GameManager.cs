using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] Camera mainCamera;

    public bool useFirstSpawnPoint = true;
    public float speedIncreaseRate = 0.1f;
    public float cameraMoveUpAmount = 1f; public float cameraMoveSpeed = 2f;

    float stackHeight;
    GameObject currentBlock;
    GameObject lastStackedBlock; Vector3 targetCameraPosition;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        targetCameraPosition = mainCamera.transform.position;
        SpawnBlock();
    }


    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceBlock();
            Debug.Log("Place");
        }

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, cameraMoveSpeed * Time.deltaTime);
    }


    void SpawnBlock()
    {
        // Determine which spawn point to use
        Transform selectedSpawnPoint = useFirstSpawnPoint ? spawnPoint1 : spawnPoint2;

        // Spawn a new block at the selected spawn point
        currentBlock = Instantiate(blockPrefab, new Vector3(selectedSpawnPoint.position.x, selectedSpawnPoint.position.y + stackHeight, selectedSpawnPoint.position.z), Quaternion.identity);

        // Add the BlockMover script and adjust its speed
        BlockMover mover = currentBlock.AddComponent<BlockMover>();
        mover.moveSpeed += stackHeight * speedIncreaseRate;

        // Toggle the spawn point for the next block
        useFirstSpawnPoint = !useFirstSpawnPoint;


    }


    void PlaceBlock()
    {
        Destroy(currentBlock.GetComponent<BlockMover>());

        if (lastStackedBlock != null && !IsContactingLastBlock())
        {
            Debug.Log("Game Over");

            return;
            //Add game over screen
            //sfx
        }
        Vector3 currentBlockPosition = currentBlock.transform.position;
        currentBlock.transform.position = new Vector3(currentBlockPosition.x, currentBlockPosition.y, currentBlockPosition.z);
        //sfx

        stackHeight += currentBlock.transform.localScale.y;
        lastStackedBlock = currentBlock;

        MoveCameraUp();

        SpawnBlock();
    }
    bool IsContactingLastBlock()
    {
        // Use the colliders to detect overlap
        Collider currentCollider = currentBlock.GetComponent<Collider>();
        Collider lastCollider = lastStackedBlock.GetComponent<Collider>();

        return currentCollider.bounds.Intersects(lastCollider.bounds);
    }

    void MoveCameraUp()
    {
        if (mainCamera != null)
        {
            // Update the target camera position
            targetCameraPosition.y += cameraMoveUpAmount;
        }
        else
        {
            Debug.LogWarning("Main Camera not assigned in GameManager!");
        }
    }
}
