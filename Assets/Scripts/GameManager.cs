using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject       m_player;
    public UIController     m_ui;
    public List<EnemyBeatController> m_enemies;
    public GameObject menu;

    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance      = this;
        m_enemies = new List<EnemyBeatController>();
    }

    // Start is called before the first frame update
    public void StartGame()
    {

        menu.SetActive(false);
        m_ui.SetPlayerFace(m_player.GetComponent<CharacterBeatView>().m_face);
        m_ui.SetPlayerName(m_player.GetComponent<CharacterBeatView>().m_name);
        m_ui.SetPlayerLife(1f);
        GameManager.Instance.m_ui.SetEnableEnemyElements(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyHitted (Sprite face, string name, float normalizedLife)
    {
        m_ui.SetEnemyFace(face);
        m_ui.SetEnemyName(name);
        m_ui.SetEnemyLife(normalizedLife);
    }

    public void PlayerHitted (float normalizedLife)
    {
        m_ui.SetPlayerLife(normalizedLife);
    }

    private void FinishStage ()
    {
        Debug.Log("FINISH!!");
    }

    public void GameOver    ()
    {
        for (int i = 0; i < m_enemies.Count; i++)
        {
            m_enemies[i].Stop();
        }
        m_ui.GameOver();
    }

    public void AddEnemy (EnemyBeatController enemy)
    {
        m_enemies.Add(enemy);
    }

    public void RemoveEnemy (EnemyBeatController enemy)
    {
        m_enemies.Remove(enemy);
    }
}
