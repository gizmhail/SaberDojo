using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCubeController : MonoBehaviour {
    public KilledCube killedCubePrefab;
    public List<CubeHitPoint> hitPoints;
    public Material debugHitMaterial;
    public float speed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += speed * Time.deltaTime*transform.forward;
	}

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 averageHitPosition = Vector3.zero;
        if (collision.contacts.Length > 0)
        {
            foreach (var contact in collision.contacts) {
                averageHitPosition += contact.point;
            }
            averageHitPosition = averageHitPosition / collision.contacts.Length;
        }
        float minDistance = float.PositiveInfinity;
        CubeHitPoint hitPoint = null;
        foreach (var candidateHitPoint in hitPoints) {
            float distance = Vector3.Distance(candidateHitPoint.transform.position, averageHitPosition);
            Debug.Log(" ? " + candidateHitPoint.name + "/" + distance);
            if (distance < minDistance) {
                hitPoint = candidateHitPoint;
                minDistance = distance;
            }

        }
        hitPoint.gameObject.GetComponent<Renderer>().material = debugHitMaterial;
        float impactPower = collision.rigidbody.velocity.magnitude;
        Debug.Log("HIT " + hitPoint.name + "/ Distance" + minDistance + " / Impact " + impactPower);
        Destroy(this.gameObject);
        KilledCube killedCube = Instantiate(killedCubePrefab, transform.position, Quaternion.identity);
        killedCube.transform.Rotate(0, 0, hitPoint.killedCubeRotation);
        killedCube.impactPower = impactPower;
        Destroy(killedCube.gameObject, 2.0f);
    }
}
