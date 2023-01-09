using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IHittableGameObjectByPlayer
{
    public GameObject m_item;
    public Transform  m_pivot;

    public void HitByPlayer(float damage, CharacterBeatController player)
    {
        Instantiate(m_item, m_pivot.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
