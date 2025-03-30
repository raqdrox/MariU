using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Athena.Mario.Player
{

    

    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;
        [SerializeField]PlayerAnimationController animController;
        [Header("Horizontal Movement")]
        [SerializeField]
        private float moveSpeed = 10f;
        [SerializeField] private Vector2 direction;
        [SerializeField] public bool enableMovement=true;
        private bool facingRight = true;
        private bool isSprinting = false;
        private bool changingDirection = false;

        [Header("Vertical Movement")]
        [SerializeField]
        private float jumpSpeed = 8f;
        [SerializeField] private float jumpDelay = 0.25f;
        private float jumpTimer = 0f;
        public float bounceForce = 20f;

        [Header("Components")]
        [SerializeField]
        public Rigidbody2D rb= null;
        [SerializeField] public Animator animator=null;
        [SerializeField] public EdgeCollider2D playerCollider;
        

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
        public float invTime = 5f;
        private PowerEffects activeEffect = PowerEffects.EFFECT_NONE;

        public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
        public PowerEffects ActiveEffect { get => activeEffect; set => activeEffect = value; }

     



        private void Awake()
        {
            instance = this;
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

        
        

        #region Movement + Physics
        private void Update()
        {
            if (enableMovement)
            {
                onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundMask) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundMask);
                if (Input.GetButtonDown("Jump"))
                    jumpTimer = Time.time + jumpDelay;
                isSprinting = Input.GetKey("x");
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
                animController.skid(true); 
            }
            else
            {
                animController.skid(false); 
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

         public void Bounce(float? force)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (force != null)
            rb.AddForce(Vector2.up * force.Value, ForceMode2D.Impulse);
        else
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
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
        public SpriteRenderer GetCurrentActiveRenderer()
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

        public void ResetPlayerRenderers()
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
                    break;
                default:
                    break;
            }
        }
    }

   
}