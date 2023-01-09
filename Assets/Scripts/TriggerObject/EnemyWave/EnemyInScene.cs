﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyInScene 
{
    public  GameObject m_enemyObject;
    public  float      m_timeToInitBehaviour = 0;
    public  bool       m_isHiddenAtStar      = false;

    private float       m_currentTime = 0;
    private EnemyGroup  m_myGroup;
    private bool        m_isStarted = false;

    public void      SetMyGroup (EnemyGroup group)
    {
        m_myGroup = group;

        if (m_isHiddenAtStar)
        {
            m_enemyObject.SetActive(false);
        }
        else
        {
            m_enemyObject.GetComponent<EnemyBeatController>().enabled = false;
        }
    }

    public void      AddTime    (float timeDelta)
    {
        m_currentTime += timeDelta;

        if (m_currentTime >= m_timeToInitBehaviour && !m_isStarted)
        {
            m_enemyObject.GetComponent<EnemyBeatController>().enabled = true;
            m_myGroup.InitEnemy (m_enemyObject.GetComponent<EnemyBeatController>());
            m_isStarted = true;
        }
    }
}
