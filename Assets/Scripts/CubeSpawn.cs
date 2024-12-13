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
    public GameObject lastStackedBlock;
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

        // Spawn a new block at the selected spawn point
        currentBlock = Instantiate(blockPrefab, new Vector3(selectedSpawnPoint.position.x, selectedSpawnPoint.position.y + stackHeight, selectedSpawnPoint.position.z), Quaternion.identity);

        // Add the BlockMover script and adjust its speed
        BlockMover mover = currentBlock.AddComponent<BlockMover>();
        mover.moveSpeed += stackHeight * speedIncreaseRate;

        // Toggle the spawn point for the next block
        useFirstSpawnPoint = !useFirstSpawnPoint;


    }

}
