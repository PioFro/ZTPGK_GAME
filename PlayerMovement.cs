using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class PlayerMovement : MonoBehaviour
{
    public float sensitivity = 10;
    public Text score;
    public Text looseText;
    Vector3 accumulatedForce;
    Rigidbody rb;
    public GameObject pickupPrefab;
    public int numberOfPickups = 1;
    public float maxH = 1F;
    public float maxV = 1F;
    public float minV = 1f;
    public float minH = 1f;
    private static int points = 0;
    // Start is called before the first frame update
    void Start()
    {
        score.text = "SCORE: 0/"+numberOfPickups;
        looseText.text = "";
        for (int i = 0; i < numberOfPickups; i++)
        {

            Instantiate(pickupPrefab, new Vector3(UnityEngine.Random.Range(minH, maxH), transform.position.y-20, UnityEngine.Random.Range(minV, maxV)), Quaternion.identity);
        }
        rb = GetComponent<Rigidbody>();
        accumulatedForce = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float moveH = Input.GetAxis("Horizontal"), moveV = Input.GetAxis("Vertical");
        if (Input.GetKeyDown("space"))
        {
            accumulatedForce = accumulatedForce + new Vector3(0f, sensitivity, 0f);
        }
        if(Input.GetKey("space"))
        {
            accumulatedForce = accumulatedForce + new Vector3(0f, sensitivity, 0f);
        }
        if(Input.GetKeyUp("space"))
        {
            //rb.AddTorque(accumulatedForce);
            rb.AddForce(accumulatedForce);
            Debug.Log(accumulatedForce);
            accumulatedForce = new Vector3(0f, 0f, 0f);
        }
        rb.AddForce(new Vector3(moveH, 0f, moveV)*sensitivity);
        //end
        if(points==numberOfPickups)
        {
            looseText.text = "YOU WON";
            this.gameObject.SetActive(false);
        }
        score.text = "SCORE: " + points.ToString() + "/" + numberOfPickups.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("EndGame"))
        {
            this.looseText.text = "YOU LOOSE";
            this.gameObject.SetActive(false);   
        }

        if(other.tag.Equals("Pickup"))
        {
            points++;
            Destroy(other.gameObject);
        }
        Debug.Log("COLLISION WITH: " +other.tag);
    }
 
    public static void increasePoints()
    {
        points++;
    }
}
