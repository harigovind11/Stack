using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [SerializeField] Camera mainCamera;

    public float cameraMoveUpAmount = 1f;
    public float cameraMoveSpeed = 2f;
    Vector3 targetCameraPosition;

    public GameObject lastStackedBlock;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        targetCameraPosition = mainCamera.transform.position;
        //SpawnBlock();
    }


    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceBlock();
            //Stop();
        }

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, cameraMoveSpeed * Time.deltaTime);
    }



    void PlaceBlock()
    {
        Destroy(CubeSpawn.instance.currentBlock.GetComponent<BlockMover>());

        if (lastStackedBlock != null && !IsContactingLastBlock())
        {
            Debug.Log("Game Over");

            return;
            //Add game over screen
            //sfx
        }
        Vector3 currentBlockPosition = CubeSpawn.instance.currentBlock.transform.position;
        Vector3 currentBlockLocalScale = CubeSpawn.instance.currentBlock.transform.localScale;

        //x axis cutting
        float hangoverX = transform.position.x - currentBlockPosition.x;
        float newXSize = currentBlockLocalScale.x - Mathf.Abs(hangoverX);
        float newXPosition = currentBlockPosition.x + (hangoverX / 2);

        float fallingBlockSizeX = currentBlockLocalScale.x - newXSize;
        float cubeEdgeX = currentBlockPosition.x + (newXSize / 2f );
        float fallingBlockPositionX = cubeEdgeX * fallingBlockSizeX / 2f ;

        //z axis cutting
        float hangoverZ = transform.position.z - currentBlockPosition.z;
        float newZSize = currentBlockLocalScale.z - Mathf.Abs(hangoverZ);
        float newZPosition = currentBlockPosition.z + (hangoverZ / 2);

        float fallingBlockSizeZ = currentBlockLocalScale.z - newZSize;
        float cubeEdgeZ = currentBlockPosition.z + (newZSize / 2f );
        float fallingBlockPositionZ = cubeEdgeZ * fallingBlockSizeZ / 2f ;


        //assign new scale and position
        CubeSpawn.instance.currentBlock.transform.position = new Vector3(newXPosition, currentBlockPosition.y, newZPosition);
        CubeSpawn.instance.currentBlock.transform.localScale = new Vector3(newXSize, currentBlockLocalScale.y, newZSize);

        bool firstSpawnPoint = CubeSpawn.instance.useFirstSpawnPoint;

        if (firstSpawnPoint)
        {
            var cubeX = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeX.transform.localScale = new Vector3(fallingBlockSizeX, currentBlockLocalScale.y, currentBlockLocalScale.z);
            cubeX.transform.localPosition = new Vector3(fallingBlockPositionX, currentBlockPosition.y, currentBlockPosition.z);
        }
        else
        {
            var cubeZ = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubeZ.transform.localScale = new Vector3(currentBlockLocalScale.x, currentBlockLocalScale.y, fallingBlockSizeZ);
            cubeZ.transform.localPosition = new Vector3(currentBlockPosition.x, currentBlockPosition.y, fallingBlockPositionZ);

        }





        //sfx

        CubeSpawn.instance.stackHeight += currentBlockLocalScale.y;
        lastStackedBlock = CubeSpawn.instance.currentBlock;

        MoveCameraUp();

        CubeSpawn.instance.SpawnBlock();
    }
    bool IsContactingLastBlock()
    {
        // Use the colliders to detect overlap
        Collider currentCollider = CubeSpawn.instance.currentBlock.GetComponent<Collider>();
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
