using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    // Singleton Instance to ensure only one CubeSpawn exists
    public static CubeSpawn Instance { get; private set; }-

    [SerializeField] private GameObject blockPrefab; // Prefab used for spawning blocks
    [SerializeField] private Transform spawnPoint1;   // First spawn location
    [SerializeField] private Transform spawnPoint2;   // Second spawn location

    public float stackHeight; // Vertical offset for stacking new blocks
    public bool useFirstSpawnPoint = true; // Toggle to alternate between spawn points
    public float speedIncreaseRate = 0.1f; // Controls how much the block's speed increases with height

    public GameObject CurrentBlock { get; private set; } // Reference to the currently active block

    private void Awake()
    {
        // Set the singleton Instance, destroy this object if another exists
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Spawn the first block when the game starts
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        // Ensure all required references are assigned
        if (blockPrefab == null || spawnPoint1 == null || spawnPoint2 == null)
        {
            Debug.LogError("CubeSpawn: Missing required references.");
            return;
        }

        // Choose which spawn point to use based on the toggle
        Transform selectedSpawnPoint = useFirstSpawnPoint ? spawnPoint1 : spawnPoint2;

        // Start with the default block scale
        Vector3 newScale = blockPrefab.transform.localScale;

        Vector3 newPosition;

        // If there's a stacked block in the game manager, inherit its scale
        if (GameManager.Instance != null && GameManager.Instance.lastStackedBlock != null)
        {
            newScale = GameManager.Instance.lastStackedBlock.transform.localScale;

            newPosition = new Vector3(
              GameManager.Instance.lastStackedBlock.transform.localPosition.x,
                selectedSpawnPoint.position.y + stackHeight,
              GameManager.Instance.lastStackedBlock.transform.localPosition.z
            );
            // Determine the position to spawn the block based on stack height
            Vector3 spawnPosition = new Vector3(
               useFirstSpawnPoint ?  newPosition.x :selectedSpawnPoint.position.x ,
                selectedSpawnPoint.position.y + stackHeight,
                 useFirstSpawnPoint ? selectedSpawnPoint.position.z:newPosition.z 
            );

            // Instantiate the block at the calculated position
            CurrentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("No last stacked block found, spawning at default position.");
            Vector3 spawnPosition = new Vector3(
         selectedSpawnPoint.position.x,
         selectedSpawnPoint.position.y + stackHeight,
         selectedSpawnPoint.position.z
     );
            CurrentBlock = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
        }




        // Apply inherited or default scale to the new block
        CurrentBlock.transform.localScale = newScale;

        // Check if a BlockMover already exists; if not, add it
        BlockMover mover = CurrentBlock.GetComponent<BlockMover>();
        if (mover == null)
        {
            mover = CurrentBlock.AddComponent<BlockMover>();
        }

        // Increase the block's move speed based on how high it's stacked
        mover.moveSpeed += stackHeight * speedIncreaseRate;

        // Toggle the spawn point to alternate for the next block
        useFirstSpawnPoint = !useFirstSpawnPoint;
    }
}
