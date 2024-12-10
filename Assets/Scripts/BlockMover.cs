using UnityEngine;


public class BlockMover : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3f; // Speed of horizontal movement
    [SerializeField] float boundary = 3f; // The maximum distance the block can move from the center



    void Update()
    {
        bool firstSpawnPoint = GameManager.instance.useFirstSpawnPoint;
       
   
        transform.Translate((!firstSpawnPoint ? Vector3.back : Vector3.right) * moveSpeed * Time.deltaTime);;

  
        if (transform.position.x > boundary || transform.position.x < -boundary)
        {
            //game over
            Debug.Log("Game ");
            //Time.timeScale = 0f;
        }
    }
}

