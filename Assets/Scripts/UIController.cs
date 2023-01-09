using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image m_playerFace;
    public Image m_playerLife;
    public Text  m_playerName;

    public Image m_enemyFace;
    public Image m_enemyLife;
    public Text  m_enemyName;

    public GameObject m_gameOver;
    
    public void SetPlayerFace(Sprite playerFace)
    {
        m_playerFace.sprite = playerFace;
    }

    public void SetPlayerLife(float lifeNormalized)
    {
        m_playerLife.fillAmount = lifeNormalized;
    }

    public void SetPlayerName(string name)
    {
        m_playerName.text = name;
    }

    public void SetEnemyFace (Sprite enemyFace)
    {
        m_enemyFace.sprite = enemyFace;
    }

    public void SetEnemyLife (float lifeNormalized)
    {
         m_enemyLife.fillAmount = lifeNormalized;
    }

    public void SetEnemyName (string name)
    {
        m_enemyName.text = name;
    }

    public void SetEnableEnemyElements(bool enabled)
    {
        m_enemyFace.gameObject.SetActive(enabled);
        m_enemyLife.gameObject.SetActive(enabled);
        m_enemyName.gameObject.SetActive(enabled);
    }

    public bool IsEnableEnemyElements()
    {
        return m_enemyFace.gameObject.activeSelf;
    }

    public void GameOver()
    {
        m_gameOver.SetActive(true);
    }
}
