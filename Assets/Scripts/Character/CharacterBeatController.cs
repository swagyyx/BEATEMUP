using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBeatController : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    protected int   m_maxLife;

    [SerializeField, Range(0f, 100f)]
    protected int   m_damagePerHit;

    [SerializeField, Range(0f, 3f)]
    protected float m_maxSpeedX;

    [SerializeField, Range(0f, 3f)]
    protected float m_maxSpeedY;

    [SerializeField, Range(0f, 100f)]
	protected float m_maxAcceleration = 10f;

    [SerializeField, Range(0f, 100f)]
    protected float m_maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)]
	protected float m_jumpHeight = 2f;

    [SerializeField]
    protected Transform m_bottomAnchor;

    [SerializeField]
    protected Vector2   m_hitSize;

    [SerializeField]
    protected Transform m_hitAnchor;

    [SerializeField]
    protected Transform m_topLimit;

    [SerializeField]
    protected Transform m_bottomLimit;

    [SerializeField]
    protected Transform m_leftLimit;

    [SerializeField]
    protected Transform m_rightLimit;

    [SerializeField]
    protected int       m_currentLife;

    protected void Awake        () 
	{
        m_currentLife = m_maxLife;
	}

    public    void AddHealth    (int health)
    {
        m_currentLife += health;
    }

    protected void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(m_hitAnchor.position, m_hitSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(m_bottomAnchor.position, new Vector2(0.1f, 0.1f));
    }
}
