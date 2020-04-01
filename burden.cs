using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class burden : MonoBehaviour
{
    public GameObject player;
    public Text looseText;
    // Start is called before the first frame update
    void Start()
    {
        looseText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("EndGame"))
        {
            looseText.text = "YOU LET SMALLER BALL HIT THE WATER.";
            player.SetActive(false);
            this.gameObject.SetActive(false);
        }
        if (other.tag.Equals("Pickup"))
        {
            PlayerMovement.increasePoints();
            Destroy(other.gameObject);
        }
    }
}
