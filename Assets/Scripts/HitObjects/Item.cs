using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(zOrder))]


public class Item : MonoBehaviour, IHittableGameObjectByPlayer
{
    protected    string       m_itemName;

    [SerializeField]
    private    bool         m_expiresImmediately;

    [SerializeField]
    private    int          m_expiresWithTime;

    [SerializeField]
    private    AudioSource  m_sfx;

    protected CharacterBeatController m_player;

    private   float         m_counterTime = 0;
    private   bool          m_enterActionDone = false;

    public         void HitByPlayer   (float damage, CharacterBeatController player)
    {
        m_player = player;

        m_sfx = GetComponent<AudioSource>();

        if (m_sfx != null)
        {
            m_sfx.Play();
        }

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        m_counterTime = 0;
        
        ExecuteAction ();

        m_enterActionDone = true;
    }

    public virtual void ExecuteAction ()
    {
        if (m_expiresImmediately)
        {
            ExitAction ();
        }
    }

    public virtual void ExitAction    ()
    {
        m_enterActionDone = false;
    }

    private        void Update        ()
    {
        if (m_enterActionDone && !m_expiresImmediately && m_expiresWithTime > 0)
        {
            m_counterTime += Time.deltaTime;

            if (m_counterTime < m_expiresWithTime)
            {
                ExecuteAction();
            }
            else
            {
                ExitAction();
            }
        }
    }
}
