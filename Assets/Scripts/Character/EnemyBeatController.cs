using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CharacterBeatView))]

public class EnemyBeatController : CharacterBeatController, IHittableGameObjectByPlayer
{
    private enum Character_State { CHASE, ATTACK, HURT, WAIT_TO_ATTACK, DIE}

    [SerializeField]
    private float                  m_minTimeBeforeAttack;

    [SerializeField]
    private float                  m_maxTimeBeforeAttack;

    [SerializeField]
    private float                  m_minDistanceToAttack;

    private Rigidbody2D            m_rigidBody;
    private CharacterBeatView      m_mainCharacterAnimation;
    private bool                   m_isDead      = false;
    private bool                   m_doingAttack = false;
    private Vector2                m_movementVector;
    private Vector3                m_velocity;
    private Character_State        m_playerState;
    private GameObject             m_targetPosition;
    private float                  m_counterWaitingAttack;
    private float                  m_timeBeforeAttack;

    private void Start        ()
    {
        m_rigidBody              = GetComponent<Rigidbody2D> ();
		m_mainCharacterAnimation = GetComponent<CharacterBeatView>();
        m_playerState            = Character_State.CHASE;
        m_mainCharacterAnimation.ChangeAnimatorState ("moving", 1);
        GameManager.Instance.AddEnemy(this);
        m_targetPosition = GameManager.Instance.m_player;
    }

    public void HitByPlayer   (float damage, CharacterBeatController player)
    {
        m_currentLife -= (int)damage;
        float normalizedLife = m_currentLife*1f / m_maxLife*1f;
        GameManager.Instance.m_ui.SetEnableEnemyElements(true);
        GameManager.Instance.EnemyHitted(m_mainCharacterAnimation.m_face, m_mainCharacterAnimation.name, normalizedLife);
        
        if (m_currentLife <= 0)
        {
            m_mainCharacterAnimation.ChangeAnimatorState ("die", 1);
            StopAllCoroutines();
            m_playerState = Character_State.DIE;
            m_rigidBody.velocity = Vector2.zero;
            m_velocity = Vector3.zero;
            GameManager.Instance.RemoveEnemy(this);
            GameManager.Instance.m_ui.SetEnableEnemyElements(false);
            Destroy(this.gameObject,2);
        }
        else
        {
            m_mainCharacterAnimation.ChangeAnimatorState ("hurt", 1);
            m_playerState = Character_State.HURT;
            m_rigidBody.velocity = Vector2.zero;
            StartCoroutine("FinishHurtAnimationState");
        }
    }

    public void Stop          ()
    {
        this.enabled = false;
    }

    private void MoveAction   (Vector2 movementVector) 
	{
        // Movement
        m_movementVector = Vector2.ClampMagnitude(movementVector, 1f);
        m_movementVector.x = m_movementVector.x * m_maxSpeedX;
        m_movementVector.y = m_movementVector.y * m_maxSpeedY;
        

        // Flip Sprite
        LookToPlayer ();
    }

    private void LookToPlayer ()
    {
        if (m_targetPosition.transform.position.x > transform.position.x)
        { 
			transform.rotation = Quaternion.Euler (transform.rotation.x, 0, transform.rotation.z);
		}
        else
        { 
			transform.rotation = Quaternion.Euler (transform.rotation.x, -180, transform.rotation.z);
		}
    }

    private void Update       ()
    {
        LookToPlayer ();

        if (m_playerState == Character_State.CHASE)
        {
            if (Vector3.Distance (m_targetPosition.transform.position, transform.position) > m_minDistanceToAttack)
            {
                Vector2 direction = m_targetPosition.transform.position - transform.position; 
                MoveAction (new Vector2(direction.normalized.x, direction.normalized.y)); // move to enemy
		    }
            else
            {
                MoveAction (new Vector2(0, 0));
                m_playerState  = Character_State.ATTACK;
                StartCoroutine ("Attack");
            }
        }
    }

    private void FixedUpdate  ()
    {
        if (m_playerState == Character_State.CHASE)
        {
            if ((m_bottomAnchor.position.x < m_leftLimit.position.x && m_movementVector.x < 0)|| (m_bottomAnchor.position.x > m_rightLimit.position.x &&  m_movementVector.x > 0))
            {
                m_movementVector.x = 0;
            }

            if ((m_bottomAnchor.position.y > m_topLimit.position.y && m_movementVector.y > 0)|| (m_bottomAnchor.position.y < m_bottomLimit.position.y &&  m_movementVector.y < 0))
            {
                m_movementVector.y = 0;
            }

            m_velocity = m_rigidBody.velocity;

            float maxSpeedChange = m_maxAcceleration * Time.deltaTime;

		    m_velocity.x = Mathf.MoveTowards(m_velocity.x, m_movementVector.x, maxSpeedChange);
		    m_velocity.y = Mathf.MoveTowards(m_velocity.y, m_movementVector.y, maxSpeedChange);

            m_rigidBody.velocity = m_velocity;
		}
        else if (m_playerState == Character_State.WAIT_TO_ATTACK)
        {
            m_velocity = m_rigidBody.velocity;
            float maxSpeedChange = m_maxAcceleration * Time.deltaTime;
            m_counterWaitingAttack += Time.deltaTime;
            if (m_counterWaitingAttack < m_timeBeforeAttack * 0.5f)
            {
                m_movementVector.x = -1;
            }
            else if (m_counterWaitingAttack < m_timeBeforeAttack)
            {
                
                m_movementVector.x = 1;
            }
            else
            {
                m_counterWaitingAttack = 0;
                MoveAction (new Vector2(0, 0));
                m_playerState  = Character_State.CHASE;
                m_velocity = Vector3.zero;
            }
            
            m_velocity.x = Mathf.MoveTowards(m_velocity.x, m_movementVector.x, maxSpeedChange);
		    m_velocity.y = Mathf.MoveTowards(m_velocity.y, m_movementVector.y, maxSpeedChange);
            m_rigidBody.velocity = m_velocity;
        }
    }

    private IEnumerator FinishHurtAnimationState() 
	{
        yield return new WaitForSeconds (0.3f); 
        m_mainCharacterAnimation.ChangeAnimatorState ("hurt", 0);
        m_playerState = Character_State.CHASE;
	}

    private IEnumerator Attack () 
	{
        Debug.Log("Start Attack");
        m_mainCharacterAnimation.ChangeAnimatorState ("moving", 0);
        if (!m_isDead)
        { 
			m_playerState = Character_State.ATTACK;
            m_rigidBody.velocity = Vector2.zero; 

            // Calcular si los enemigos reciben daño. OverlapSphere
            Collider2D[] objects = Physics2D.OverlapBoxAll (m_hitAnchor.position, m_hitSize, 0);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<IHittableGameObjectByEnemy>() != null)
                {
                    objects[i].GetComponent<IHittableGameObjectByEnemy>().HitByEnemy(m_damagePerHit, this);
                }
            }
		}

        yield return new WaitForSeconds (1f); 
        //m_mainCharacterAnimation.ChangeAnimatorState ("movingTransition", 0);
        m_playerState      = Character_State.WAIT_TO_ATTACK;
        m_timeBeforeAttack = Random.Range (m_minTimeBeforeAttack, m_maxTimeBeforeAttack);
        m_mainCharacterAnimation.ChangeAnimatorState ("moving", 1);
        Debug.Log("Finish Attack");
	}
}
