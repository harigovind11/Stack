using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    public static CubeSpawn instance;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;

    public float stackHeight;
    public bool useFirstSpawnPoint = true;
    public float speedIncreaseRate = 0.1f;

    public GameObject currentBlock;
    void Awake()

    {
        if (instance == null)
        {
            instance = this;
        }
        SpawnBlock();
    }

    public void SpawnBlock()
    {
        // Determine which spawn point to use
        Transform selectedSpawnPoint = useFirstSpawnPoint ? spawnPoint1 : spawnPoint2;

        // Get the scale from the last stacked block (if it exists)
        Vector3 newScale = blockPrefab.transform.localScale;
        if (GameManager.instance.lastStackedBlock != null)
        {
            newScale = GameManager.instance.lastStackedBlock.transform.localScale;
        }

        // Spawn the new block with the inherited scale
        currentBlock = Instantiate(
            blockPrefab,
            new Vector3(
                selectedSpawnPoint.position.x,
                selectedSpawnPoint.position.y + stackHeight,
                selectedSpawnPoint.position.z
            ),
            Quaternion.identity
        );
        currentBlock.transform.localScale = newScale; // Apply the inherited scale

        // Add BlockMover script and adjust speed
        BlockMover mover = currentBlock.AddComponent<BlockMover>();
        mover.moveSpeed += stackHeight * speedIncreaseRate;

        // Toggle spawn point for the next block
        useFirstSpawnPoint = !useFirstSpawnPoint;
    }

}
