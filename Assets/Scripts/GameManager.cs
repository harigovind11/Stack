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
            //BlockMover.Instance.Stop();
            Stop();
        }

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, cameraMoveSpeed * Time.deltaTime);
    }



    void PlaceBlock()
    {
        Destroy(CubeSpawn.instance.currentBlock.GetComponent<BlockMover>());

        if (CubeSpawn.instance.lastStackedBlock != null && !IsContactingLastBlock())
        {
            Debug.Log("Game Over");


            return;
            //Add game over screen
            //sfx
        }
        Vector3 currentBlockPosition = CubeSpawn.instance.currentBlock.transform.position;
        CubeSpawn.instance.currentBlock.transform.position = new Vector3(currentBlockPosition.x, currentBlockPosition.y, currentBlockPosition.z);
        //sfx

        CubeSpawn.instance.stackHeight += CubeSpawn.instance.currentBlock.transform.localScale.y;
        CubeSpawn.instance.lastStackedBlock = CubeSpawn.instance.currentBlock;

        MoveCameraUp();

        CubeSpawn.instance.SpawnBlock();
    }
    bool IsContactingLastBlock()
    {
        // Use the colliders to detect overlap
        Collider currentCollider = CubeSpawn.instance.currentBlock.GetComponent<Collider>();
        Collider lastCollider = CubeSpawn.instance.lastStackedBlock.GetComponent<Collider>();

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

    ////
    ///

    public void Stop()
    {
        //moveSpeed = 0f;
        float hangover = transform.position.z - CubeSpawn.instance.lastStackedBlock.transform.position.z;
        Debug.Log(hangover);
        SplitBlockOnZ(hangover);
    }
    void SplitBlockOnZ(float hangover)
    {
        float newZSize = CubeSpawn.instance.lastStackedBlock.transform.localScale.z - Mathf.Abs(hangover);
        float fallingBlockSize = transform.localScale.z - newZSize;

        float newZPosition = CubeSpawn.instance.lastStackedBlock.transform.position.z + (hangover / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);


    }


}
