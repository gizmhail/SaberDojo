using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledCube : MonoBehaviour {
    public GameObject part1;
    public GameObject part2;
    public float impactPower;

    // Use this for initialization
    void Start () {
        // Set a minimal impact power
        impactPower = Mathf.Max(impactPower, 1.0f);
        part1.GetComponent<Rigidbody>().velocity = - impactPower * transform.right;
        part2.GetComponent<Rigidbody>().velocity = impactPower * transform.right;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
