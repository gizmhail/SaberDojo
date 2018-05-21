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
        float impactPower = collision.rigidbody.velocity.magnitude;

        CubeHitPoint hitPoint = null;
        if (collision.rigidbody.velocity.magnitude == 0)
        {
            // Should not occur. Switch to position based hit point selection
            float minDistance = float.PositiveInfinity;
            foreach (var candidateHitPoint in hitPoints)
            {
                float distance = Vector3.Distance(candidateHitPoint.transform.position, averageHitPosition);
                Debug.Log(" ? " + candidateHitPoint.name + "/" + distance);
                if (distance < minDistance)
                {
                    hitPoint = candidateHitPoint;
                    minDistance = distance;
                }

            }
            Debug.Log("HIT pos" + hitPoint.name + "/ Distance" + minDistance + " / Impact " + impactPower);
        }
        else {
            // Direction based
            foreach (var candidateHitPoint in hitPoints)
            {
                if (hitPoint == null)
                {
                    hitPoint = candidateHitPoint;
                }
                if (candidateHitPoint.expectedDirection.x != 0 && Mathf.Sign(candidateHitPoint.expectedDirection.x) == Mathf.Sign(collision.rigidbody.velocity.x) && Mathf.Abs(collision.rigidbody.velocity.x) > Mathf.Abs(collision.rigidbody.velocity.y))
                {
                    hitPoint = candidateHitPoint;
                    Debug.Log("HIT dirX " + hitPoint.name + "Other velo " + collision.rigidbody.velocity.normalized + " / Expected " + candidateHitPoint.expectedDirection + " / Impact " + impactPower);
                    break;
                }
                if (candidateHitPoint.expectedDirection.y != 0 && Mathf.Sign(candidateHitPoint.expectedDirection.y) == Mathf.Sign(collision.rigidbody.velocity.y) && Mathf.Abs(collision.rigidbody.velocity.y) > Mathf.Abs(collision.rigidbody.velocity.x))
                {
                    hitPoint = candidateHitPoint;
                    Debug.Log("HIT dirY " + hitPoint.name + "Other velo " + collision.rigidbody.velocity.normalized + " / Expected " + candidateHitPoint.expectedDirection + " / Impact " + impactPower);
                    break;
                }
            }
        }

        DebugText.SharedInstance.text = hitPoint.name;

        hitPoint.gameObject.GetComponent<Renderer>().material = debugHitMaterial;
        Destroy(this.gameObject);
        KilledCube killedCube = Instantiate(killedCubePrefab, transform.position, Quaternion.identity);
        killedCube.transform.Rotate(0, 0, hitPoint.killedCubeRotation);
        killedCube.impactPower = impactPower;
        Destroy(killedCube.gameObject, 2.0f);
    }
}
