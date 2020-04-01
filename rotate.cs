using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private GameObject gameObject;
    private Transform myPosition;
    private Vector3 rotateVector;
    // Start is called before the first frame update
    void Start()
    {
        rotateVector = new Vector3(UnityEngine.Random.Range(0, 0.5f), UnityEngine.Random.Range(0, 0.5f), UnityEngine.Random.Range(0, 0.5f));
        myPosition = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        myPosition.Rotate(rotateVector);
    }
}
