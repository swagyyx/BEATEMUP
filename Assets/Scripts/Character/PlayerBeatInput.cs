using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatInput : MonoBehaviour
{
    [SerializeField]
    private KeyCode m_jumpButton;

    [SerializeField]
    private KeyCode m_attackButton;

    [SerializeField]
    private KeyCode m_attackButton2;

    private PlayerBeatController m_mainCharacter;
    private Vector2              m_movementVector;

    void Start ()
    {
        m_mainCharacter  = GetComponent<PlayerBeatController>();
        m_movementVector = new Vector2();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<Animator>().SetTrigger("Patrol");
        }



        m_movementVector.x = Input.GetAxisRaw("Horizontal");
        m_movementVector.y = Input.GetAxisRaw("Vertical");
        m_mainCharacter.MoveAction(m_movementVector);

		if (Input.GetKeyDown (m_jumpButton))
		{
			m_mainCharacter.JumpAction ();
		}
		else if (Input.GetKeyDown (m_attackButton))
		{
			m_mainCharacter.AttackAction ();
		}
        else if (Input.GetKeyDown(m_attackButton2))
        {
            m_mainCharacter.AttackAction2();
        }
    }

    


}
