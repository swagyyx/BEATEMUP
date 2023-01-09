using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zOrder : MonoBehaviour
{
    public Transform m_anchor;

    SpriteRenderer m_sprite;
    // Start is called before the first frame update
    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_anchor == null)
        { 
            m_sprite.sortingOrder = (int)(transform.position.y*-10);
        }
        else
        {
            m_sprite.sortingOrder = (int)(m_anchor.position.y*-10);
        }
    }
}
