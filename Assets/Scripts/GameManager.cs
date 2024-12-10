using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;

    public bool useFirstSpawnPoint = true;
    public float placementTolerance = 0.5f;
    public float speedIncreaseRate = 0.1f;

    float stackHeight;
    GameObject currentBlock;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        SpawnBlock();
    }


    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceBlock();
            Debug.Log("Place");
        }


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

        print(Mathf.Abs(currentBlock.transform.position.x));
        if (Mathf.Abs(currentBlock.transform.position.x) > 0.5f)
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


        SpawnBlock();
    }
}
