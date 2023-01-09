using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(zOrder))]

public class CharacterBeatView : MonoBehaviour
{
    public Sprite      m_face;
    public string      m_name;

    private Animator   m_animator;

    private void Awake()
    {
        m_animator       = GetComponent<Animator>();
    }

    public void ChangeAnimatorState  (string variable, int i) 
	{
		m_animator.SetInteger (variable, i);
	}

    public void ChangeAnimatorState  (string variable, bool i) 
	{
		m_animator.SetBool (variable, i);
	}
}
