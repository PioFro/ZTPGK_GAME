using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("Terrain"))
        {
            transform.RotateAround(player.transform.position, 0.1f);
            Debug.Log("terrain hit with camera");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Terrain"))
        {
            transform.RotateAround(player.transform.position, 0.1f);
            Debug.Log("terrain hit with camera");
        }
    }


}
