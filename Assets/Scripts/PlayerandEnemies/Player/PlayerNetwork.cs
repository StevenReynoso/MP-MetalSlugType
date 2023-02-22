using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerNetwork : NetworkBehaviour
{
    public float speed;
    public int jumpForce;

    public Rigidbody2D rb;
    public Animator anim;
    public LayerMask groundLayer;
    public Vector3 boxSize;

    private enum State {Idle, LeftIdle, RightIdle, RunningLeft, RunningRight, BackIdle };
    private State state;

    public float maxDistance;
    public bool isGrounded;
    private bool facingRight;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (!IsOwner) return;


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical);

        if (direction.magnitude > 0)
        {
            if (horizontal < 0)
            {
                state = State.RunningLeft;
                facingRight = false;
            }
            else if (horizontal > 0)
            {
                state = State.RunningRight;
                facingRight = true;
            }
        }
        else if (state == 0 && facingRight == false)
        {
            state = State.LeftIdle;
            
        }
        else if (state == 0 && facingRight)
        {
            state = State.RightIdle;
            
        }
        else
        {
            state = State.Idle;
            horizontal = 0;

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            state = State.BackIdle;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        
        anim.SetInteger("State", (int)state);




        GroundCheck();




        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            CmdJump();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

    private bool GroundCheck()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, maxDistance, groundLayer))
        {
            return isGrounded = true;
        }
        else
        {
            return isGrounded = false;
        }
    }


    private void CmdJump()
    {
        JumpClientRpc();
    }

    [ClientRpc]
    private void JumpClientRpc()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
