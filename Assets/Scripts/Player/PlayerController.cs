using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Athena.Mario.Player
{

    public enum PlayerStates
    {
        MARIO_DEAD,
        MARIO_SMALL,
        MARIO_BIG,
        MARIO_FIRE
    }

    public enum PowerEffects
    {
        EFFECT_NONE,
        EFFECT_SINV,
        EFFECT_CINV
    }

    public class PlayerController : MonoBehaviour
    {


        [Header("Horizontal Movement")]
        [SerializeField]
        private float moveSpeed = 10f;
        [SerializeField] private Vector2 direction;
        [SerializeField] private bool enableMovement=true;
        private bool facingRight = true;
        private bool isSprinting = false;
        private bool changingDirection = false;

        [Header("Vertical Movement")]
        [SerializeField]
        private float jumpSpeed = 8f;
        [SerializeField] private float jumpDelay = 0.25f;
        private float jumpTimer = 0f;
        [SerializeField] private float bounceForce = 20f;

        [Header("Components")]
        [SerializeField]
        private Rigidbody2D rb= null;
        [SerializeField] private Animator animator=null;
        [SerializeField] private EdgeCollider2D playerCollider;
        

        [Header("Physics")]
        [SerializeField]
        private float maxSpeed = 7f;
        [SerializeField] private float maxSprintSpeed = 10f;
        [SerializeField] private float linearDrag = 4f;
        [SerializeField] private float gravity = 4f;
        [SerializeField] private float fallMultiplier = 4f;
        [SerializeField] private bool enableLinearDrag = true;


        [Header("Collision")]
        [SerializeField]
        private bool onGround=false;
        [SerializeField] private float groundLength = 0.6f;
        [SerializeField] private Vector3 colliderOffset;
        [SerializeField] private List<LayerMask> groundLayers;
        [SerializeField] private int invIgnoreLayer;
        private int groundMask;
        

        [Header("Player States")]
        [SerializeField] public PlayerStates CurrentPlayerState = PlayerStates.MARIO_SMALL;

        [SerializeField] private SpriteRenderer smallMarioRenderer;
        [SerializeField] private SpriteRenderer bigMarioRenderer;
        [SerializeField] private SpriteRenderer fireMarioRenderer;
        [SerializeField] private SpriteRenderer deadMarioRenderer;

        [SerializeField] private Vector2[] smallMarioColliderVals;
        [SerializeField] private Vector2[] bigMarioColliderVals;

        [Header("Player Effects")]
        private bool isInvincible=false;
        [SerializeField] private float invTime = 5f;
        private PowerEffects activeEffect = PowerEffects.EFFECT_NONE;

        public bool IsInvincible { get => isInvincible; private set => isInvincible = value; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            playerCollider = GetComponent<EdgeCollider2D>();
            
            if (groundLayers.Count>0)
            {
                groundMask = 0;
                foreach (var mask in groundLayers)
                {
                    groundMask |= mask.value;
                }
                
            }
        }

        private void Start()
        {
            EnablePlayer();
            
        }

        private void EnablePlayer()
        {
            enableMovement = true;
            playerCollider.enabled = true;
            rb.simulated = true;
        }

        #region Movement + Physics
        private void Update()
        {
            if (enableMovement)
            {
                onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundMask) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundMask);
                if (Input.GetButtonDown("Jump"))
                    jumpTimer = Time.time + jumpDelay;
                isSprinting = Input.GetKey(KeyCode.LeftShift);
                direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
        }
        private void FixedUpdate()
        {
            if (enableMovement)
            {
                MoveCharacter(direction.x);
                ModifyPhysics();
                if (jumpTimer > Time.time && onGround)
                    Jump();
            }
            
        }

        private void MoveCharacter(float horizontal)
        {
            changingDirection = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
            rb.AddForce(Vector2.right * horizontal * moveSpeed);
            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
                Flip();

            if(Mathf.Abs(rb.velocity.x)> (isSprinting ? maxSprintSpeed : maxSpeed))
            {
                rb.velocity = new Vector2( Mathf.Sign(rb.velocity.x)* (isSprinting ? maxSprintSpeed : maxSpeed), rb.velocity.y);
            }


            if (changingDirection)
            {
                animator.SetBool("skid", true);   
            }
            else
            {
                animator.SetBool("skid", false);
                animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
            }

            animator.SetFloat("vertical", rb.velocity.y);


        }

        private void Jump()
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpTimer = 0;
        }

        private void ModifyPhysics()
        {
            if (onGround)
            {
                if ((Mathf.Abs(direction.x) < 0.4f || changingDirection) && enableLinearDrag)
                {
                    rb.drag = linearDrag;
                }
                else
                {
                    rb.drag = 0;
                }
                rb.gravityScale = 0;
            }
            else
            {
                rb.gravityScale = gravity;
                rb.drag = enableLinearDrag ? linearDrag * 0.15f: 0;
                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = gravity * fallMultiplier;

                }else if(rb.velocity.y>0 && !Input.GetButton("Jump"))
                {
                    rb.gravityScale = gravity * fallMultiplier / 2;
                }
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
            Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
        }
        #endregion


        private SpriteRenderer GetCurrentActiveRenderer()
        {
            switch (CurrentPlayerState)
            {
                case PlayerStates.MARIO_SMALL:
                    return smallMarioRenderer;
                case PlayerStates.MARIO_BIG:
                    return bigMarioRenderer;
                case PlayerStates.MARIO_FIRE:
                    return fireMarioRenderer;
                case PlayerStates.MARIO_DEAD:
                    return deadMarioRenderer;
                default:
                    return smallMarioRenderer;
            }
        }

        private void ResetPlayerRenderers()
        {
            ResetRenderer(smallMarioRenderer);
            ResetRenderer(bigMarioRenderer);
            ResetRenderer(fireMarioRenderer);
            ResetRenderer(deadMarioRenderer);
        }

        private void ResetRenderer(SpriteRenderer renderer)
        {
            renderer.color = Color.white;
        }

        private void OnValidate()
        {
            if(CurrentPlayerState!=PlayerStates.MARIO_DEAD)
                SetPlayerState(CurrentPlayerState);
        }

        public void SetPlayerState(PlayerStates state)
        {
            CurrentPlayerState = state;
            switch (state)
            {
                case PlayerStates.MARIO_SMALL:
                    playerCollider.points = smallMarioColliderVals;
                    smallMarioRenderer.gameObject.SetActive(true);
                    bigMarioRenderer.gameObject.SetActive(false);
                    fireMarioRenderer.gameObject.SetActive(false);
                    deadMarioRenderer.gameObject.SetActive(false);
                    break;
                case PlayerStates.MARIO_BIG:
                    playerCollider.points = bigMarioColliderVals;
                    smallMarioRenderer.gameObject.SetActive(false);
                    bigMarioRenderer.gameObject.SetActive(true);
                    fireMarioRenderer.gameObject.SetActive(false);
                    deadMarioRenderer.gameObject.SetActive(false);
                    break;
                case PlayerStates.MARIO_FIRE:
                    playerCollider.points = bigMarioColliderVals;
                    smallMarioRenderer.gameObject.SetActive(false);
                    bigMarioRenderer.gameObject.SetActive(false);
                    fireMarioRenderer.gameObject.SetActive(true);
                    deadMarioRenderer.gameObject.SetActive(false);
                    break;
                case PlayerStates.MARIO_DEAD:
                    playerCollider.points = smallMarioColliderVals;
                    smallMarioRenderer.gameObject.SetActive(false);
                    bigMarioRenderer.gameObject.SetActive(false);
                    fireMarioRenderer.gameObject.SetActive(false);
                    deadMarioRenderer.gameObject.SetActive(true);
                    PlayerDie();
                    break;
                default:
                    break;
            }
        }

        private IEnumerator MarioDeathSequence()
        {
            enableMovement = false;
            playerCollider.enabled = false;
            //Move.constraints = RigidbodyConstraints2D.FreezeAll;
            rb.velocity = Vector2.zero;
            float popForce = 10f;
            rb.AddForce(Vector2.up * popForce, ForceMode2D.Impulse);
            rb.gravityScale = gravity*1f;
            
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }

        public void GetHit()
        {
            if (!IsInvincible && CurrentPlayerState != PlayerStates.MARIO_DEAD) 
                PowerDown();
        }
        public void PowerUp()
        {

            if (CurrentPlayerState == PlayerStates.MARIO_SMALL)
            {
                SetPlayerState(PlayerStates.MARIO_BIG);
            }
            else
            {
                SetPlayerState(PlayerStates.MARIO_FIRE);
            }

        }
        private void PowerDown()
        {
            if (CurrentPlayerState == PlayerStates.MARIO_SMALL)
            {
                SetPlayerState(PlayerStates.MARIO_DEAD);
            }
            else
            {
                SetPlayerState(PlayerStates.MARIO_SMALL);
                SetEffect(PowerEffects.EFFECT_CINV, invTime);
            }
        }

        private void PlayerDie()
        {
            StartCoroutine(MarioDeathSequence());
        }

        public void SetEffect(PowerEffects effect,float time=0f)
        {
            activeEffect = effect;
            switch (effect)
            {
                case PowerEffects.EFFECT_SINV:
                    StartCoroutine(SInvEffect(time));
                    break;
                case PowerEffects.EFFECT_CINV:
                    StartCoroutine(CInvEffect(time));
                    break;
                default:
                    break;
            }
        }

        private IEnumerator SInvEffect(float time)
        {
            IsInvincible = true;
            
            var currTime = 0f;
            var currSwitchTime = 0f;
            var switchTime = 0.1f;
            var switchMode = false;
            while (currTime < time)
            {
                var ren = GetCurrentActiveRenderer();

                if (currSwitchTime >= switchTime)
                {
                    switchMode = !switchMode;
                    currSwitchTime = 0f;
                    ren.color = switchMode? Color.red : Color.white;
                }
                currSwitchTime += Time.deltaTime;
                currTime += Time.deltaTime;
                yield return null;
            }
            ResetPlayerRenderers();
            IsInvincible = false;
            SetEffect(PowerEffects.EFFECT_NONE);
        }

        private IEnumerator CInvEffect(float time)
        {
            IsInvincible = true;
            Physics2D.IgnoreLayerCollision(10,11,true);
            var currTime = 0f;
            var currSwitchTime = 0f;
            var switchTime = 0.1f;
            var switchMode = false;
            while (currTime < time)
            {
                var renderer = GetCurrentActiveRenderer();
                if (currSwitchTime >= switchTime)
                {
                    switchMode = !switchMode;
                    var col = renderer.color;
                    col.a = switchMode? 0f:0.7f;
                    renderer.color = col;
                    
                }
                currSwitchTime += Time.deltaTime;
                currTime += Time.deltaTime;
                yield return null;
            }
            ResetPlayerRenderers();
            Physics2D.IgnoreLayerCollision(10,11,false);
            IsInvincible = false;
            SetEffect(PowerEffects.EFFECT_NONE);
        }



        public void BounceOff()
        {
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
        public void BounceOff(float force)
        {
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        public bool IsEffectActive(PowerEffects effect)
        {
            return activeEffect == effect;
        }
    }
}