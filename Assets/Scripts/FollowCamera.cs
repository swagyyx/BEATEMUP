using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float speed = 15f;
	public float minDistance;
	public GameObject target;
	public Vector3 offset;

    public Transform m_leftUpLimit;
    public Transform m_rightBottomLimit;
    
    

	private Vector3 targetPos;

	// Use this for initialization
	void Start () {
		targetPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (target)
		{
			Vector3 posNoZ = transform.position + offset;
			Vector3 targetDirection = (target.transform.position - posNoZ);
			float interpVelocity = targetDirection.magnitude * speed;
			targetPos = (transform.position) + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            Vector3 m_cameraPosition = Vector3.Lerp( transform.position, targetPos, 0.25f);
            m_cameraPosition.x = Mathf.Clamp(m_cameraPosition.x, m_leftUpLimit.position.x, m_rightBottomLimit.position.x);
            m_cameraPosition.y = Mathf.Clamp(m_cameraPosition.y, m_rightBottomLimit.position.y, m_leftUpLimit.position.y);
            //m_cameraPosition.z = 1;
            transform.position = m_cameraPosition;
		}
	}
}
