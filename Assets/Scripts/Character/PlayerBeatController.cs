using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CharacterBeatView))]
[RequireComponent(typeof(PlayerBeatInput))]

public class PlayerBeatController : CharacterBeatController, IHittableGameObjectByEnemy
{
    private enum Character_State { IDLE, WALK, JUMP, ATTACK, ATTACK2, FALL, HURT, DIE}

    private Rigidbody2D            m_rigidBody;
    private CharacterBeatView      m_mainCharacterAnimation;
    private float                  m_floorLevel;
    private Vector2                m_movementVector;
    private Vector3                m_velocity;
    private Vector3                m_jumpVelocity;
    private bool                   m_desiredJump;
    private Character_State        m_playerState;

    [SerializeField, Range(0f, 3f)]
    protected float m_timeToFinishAttackAnimation;

    [SerializeField, Range(0f, 3f)]
    protected float m_timeToFinishHurtAnimation;

    private void Awake                () 
	{
        m_rigidBody              = GetComponent<Rigidbody2D> ();
		m_mainCharacterAnimation = GetComponent<CharacterBeatView>();

        m_currentLife            = m_maxLife;
		m_rigidBody.gravityScale = 0;
		m_floorLevel             = float.MinValue;
        m_playerState            = Character_State.IDLE;
	}

    public void  MoveAction           (Vector2 movementVector) 
	{
        // Movement
        m_movementVector = Vector2.ClampMagnitude(movementVector, 1f);
        m_movementVector.x = m_movementVector.x * m_maxSpeedX;
        m_movementVector.y = m_movementVector.y * m_maxSpeedY;

        if (m_playerState == Character_State.IDLE || m_playerState == Character_State.WALK)
        {
            // Flip Sprite
            if (m_movementVector.x < 0 && transform.rotation.y != -180)
            { 
			    transform.rotation = Quaternion.Euler (transform.rotation.x, -180, transform.rotation.z); 
		    }
            else if (m_movementVector.x > 0 && transform.rotation.y != 0)
            { 
			    transform.rotation = Quaternion.Euler (transform.rotation.x, 0, transform.rotation.z); 
		    }

            // Set Animation is started or stopped movement
            if (m_movementVector == Vector2.zero && m_playerState != Character_State.IDLE)
            {
                m_playerState = Character_State.IDLE;
                m_mainCharacterAnimation.ChangeAnimatorState ("moving", 0); 
		    }
            else if (m_movementVector != Vector2.zero && m_playerState != Character_State.WALK)
            { 
                m_playerState = Character_State.WALK;
			    m_mainCharacterAnimation.ChangeAnimatorState ("moving", 1);
		    }
        }
	}
    
    public void  JumpAction           ()
    {
        if (m_playerState == Character_State.IDLE || m_playerState == Character_State.WALK)
        {
            m_desiredJump = true;
            m_playerState = Character_State.JUMP;
		}
    }

    public void  AttackAction         () 
	{
        if (m_playerState == Character_State.IDLE || m_playerState == Character_State.WALK)
        {
            m_playerState        = Character_State.ATTACK;
            m_rigidBody.velocity = Vector2.zero; 
			m_mainCharacterAnimation.ChangeAnimatorState ("attack", 1);

            Collider2D[] objects = Physics2D.OverlapBoxAll (m_hitAnchor.position, m_hitSize, 0);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<IHittableGameObjectByPlayer>() != null)
                {
                    objects[i].GetComponent<IHittableGameObjectByPlayer>().HitByPlayer(m_damagePerHit, this);
                }
            }

            StartCoroutine ("FinishAttackAnimationState");
        }
	}

    // SEGUNDO ATAQUE AÑADIDO
    public void AttackAction2()
    {
        if (m_playerState == Character_State.IDLE || m_playerState == Character_State.WALK)
        {
            m_playerState = Character_State.ATTACK2;
            m_rigidBody.velocity = Vector2.zero;
            m_mainCharacterAnimation.ChangeAnimatorState("attack2", 2);

            Collider2D[] objects = Physics2D.OverlapBoxAll(m_hitAnchor.position, m_hitSize, 0);

            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<IHittableGameObjectByPlayer>() != null)
                {
                    objects[i].GetComponent<IHittableGameObjectByPlayer>().HitByPlayer(m_damagePerHit, this);
                }
            }

            StartCoroutine("FinishAttack2AnimationState");
        }
    }

    public void  HitByEnemy           (float damage, CharacterBeatController player)
    {
        if (m_playerState == Character_State.ATTACK)
        {
            return;
        }
        else if (m_playerState == Character_State.FALL || m_playerState == Character_State.JUMP)
        {
            SetInGround ();
        }

        m_currentLife -= (int)damage;
        float normalizedLife = m_currentLife*1f / m_maxLife*1f;
        GameManager.Instance.PlayerHitted(normalizedLife);

        if (m_currentLife < 0) // Die
        {
            m_mainCharacterAnimation.ChangeAnimatorState ("hurt", 2);
            m_playerState = Character_State.DIE;
            m_rigidBody.velocity = Vector2.zero;
            GameManager.Instance.GameOver();
            StopAllCoroutines();
        }
        else // Take hit
        {
            if (m_playerState == Character_State.FALL)
            {
                SetInGround ();
            }

            m_mainCharacterAnimation.ChangeAnimatorState ("hurt", 1);
            m_playerState = Character_State.HURT;
            m_rigidBody.velocity = Vector2.zero;
            StartCoroutine("FinishHurtAnimationState");
        }
    }

    private IEnumerator FinishAttackAnimationState() 
	{
        yield return new WaitForSeconds (m_timeToFinishAttackAnimation); 
        m_mainCharacterAnimation.ChangeAnimatorState ("attack", 0);
        m_playerState = Character_State.IDLE;
	}

    private IEnumerator FinishAttack2AnimationState()
    {
        yield return new WaitForSeconds(m_timeToFinishAttackAnimation);
        m_mainCharacterAnimation.ChangeAnimatorState("attack2", 0);
        m_playerState = Character_State.IDLE;
    }

    private IEnumerator FinishHurtAnimationState  () 
	{
        yield return new WaitForSeconds (m_timeToFinishHurtAnimation); 
        m_mainCharacterAnimation.ChangeAnimatorState ("hurt", 0);
        m_playerState = Character_State.IDLE;
	}

    private void SetInGround          () 
	{
        m_playerState            = Character_State.IDLE;
        m_mainCharacterAnimation.ChangeAnimatorState ("moving", 0); 
        m_rigidBody.gravityScale = 0; 
		m_rigidBody.velocity     = new Vector2 (m_rigidBody.velocity.x, 0);
        transform.position       = new Vector2 (transform.position.x, m_floorLevel);
		m_floorLevel             = float.MinValue;
        m_mainCharacterAnimation.ChangeAnimatorState ("fall", 0);
	}

    private void Jump                 ()
    {
		m_rigidBody.gravityScale = 1; 
        m_velocity.y += Mathf.Sqrt(-2f * Physics.gravity.y * m_jumpHeight);
		m_floorLevel             = transform.position.y - 0.00001f;
        m_mainCharacterAnimation.ChangeAnimatorState ("moving", 0); 
		m_mainCharacterAnimation.ChangeAnimatorState ("jump", 1);
    }

    private void Update               ()
    {
        if (m_playerState == Character_State.FALL)
        {
            if (transform.position.y <= m_floorLevel) 
		    {
			    SetInGround ();
		    }
        }
        else if (m_playerState == Character_State.JUMP)
        {
            if (m_rigidBody.velocity.y < 0)
            {
                m_playerState = Character_State.FALL;
                m_mainCharacterAnimation.ChangeAnimatorState("jump", 0);
                m_mainCharacterAnimation.ChangeAnimatorState("fall", 1);
            }
        }
    }

    private void FixedUpdate          ()
    {
        if (m_playerState == Character_State.IDLE || m_playerState == Character_State.WALK || m_playerState == Character_State.JUMP)
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

            float acceleration = (m_playerState != Character_State.JUMP) ? m_maxAcceleration : m_maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;

		    m_velocity.x = Mathf.MoveTowards(m_velocity.x, m_movementVector.x, maxSpeedChange);
		    m_velocity.y = Mathf.MoveTowards(m_velocity.y, m_movementVector.y, maxSpeedChange);

            if (m_desiredJump)
            {
			    m_desiredJump  = false;
			    Jump();
		    }

            m_rigidBody.velocity = m_velocity;
		}
    }

    private void OnTriggerEnter2D     (Collider2D other)
    {
        if (other.GetComponent<ITriggerObject>() != null)
        {
            other.GetComponent<ITriggerObject>().TriggerByPlayer(this);
        }
    }
}
