using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Vector2 move;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private bool jumpPressed;
    private bool jumpReleased;
    private float jumpBufferTime;
    private float coyoteTimeTimer;
    private int extraJumpsRemaining;
    private bool dashPressed;
    private bool isDashing;
    private Vector2 dashDir;
    private bool isFacingRight;
    public int dashCount;
    private float lastVelocity;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpStrength = 30f;
    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float gravityScaleGrounded = 1f;
    [SerializeField] private float gravityScaleFalling = 12f;
    [SerializeField] private float apexModifier = 30f;
    [SerializeField] private float maxFallSpeed = 30f;
    [SerializeField] private float dashPower = 500f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private int maxDashCount = 1;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deAcceleration = 10f;
    [SerializeField] private float velPower = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem dashParticles;
    // Start is called before the first frame update
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        InputHandler();
        JumpHandler();
        ChangeColor();

        if(move.x < 0 && !isFacingRight){
            FlipThePlayer();
        }
        if(move.x > 0 && isFacingRight){
            FlipThePlayer();
        }

        //Check if the player is grounded
        if(isGrounded()){
            rb.gravityScale = gravityScaleGrounded;
            coyoteTimeTimer = coyoteTime;
            dashCount = maxDashCount;
        }
        else{
            coyoteTimeTimer -= Time.deltaTime;
        }

        //Change gravity if the y velocity is lower than the set apex.
        if(rb.velocity.y < apexModifier && !isGrounded() && !isDashing){
            rb.gravityScale = gravityScaleFalling;
        }

        if(dashPressed && dashCount > 0){
            dashDir = move.normalized;
            dashCount -= 1;
            if(dashDir.x == 0f && dashDir.y==0f){
                if(isFacingRight){
                    StartCoroutine(Dash(-transform.right));
                }
                else{
                    StartCoroutine(Dash(transform.right));
                }
            }
            else{
                StartCoroutine(Dash(dashDir));
            }
        }
    }


    private void FixedUpdate() {
        float targetSpeed = move.x * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deAcceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
        //Handle horizontal movement
        if(!isDashing){
            //rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
            rb.AddForce(movement*Vector2.right);
        }

        //Clamp the fall speed
        if(rb.velocity.y < -maxFallSpeed){
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }

    }

    void JumpHandler (){
        //Handle jumpbutton pressed
        if(jumpPressed ){
            jumpBufferTime = jumpBuffer;
        }
        else{
            jumpBufferTime -= Time.deltaTime;
        }

        if(jumpBufferTime > 0 && coyoteTimeTimer > 0){
            Jump();
            jumpBufferTime = 0f;
        }

        //Handle jumpbutton released
        if(jumpReleased){
            rb.gravityScale = gravityScaleFalling;
            coyoteTimeTimer = 0f;
        }
    }

    void InputHandler(){
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");
        jumpReleased = Input.GetButtonUp("Jump");
        dashPressed = Input.GetButtonDown("Fire1");
    }

    //Check if the player is grounded.
    bool isGrounded(){
        Vector2 position =  transform.position;
        Vector2 direction = Vector2.down;
        float rayDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction,rayDistance,groundLayer);
        if(hit.collider!= null){
            return true;
        }
        return false;
    }

    void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
    }

    void FlipThePlayer(){
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        isFacingRight = !isFacingRight;

    }

    IEnumerator Dash(Vector2 dir){
        isDashing = true;
        dashParticles.Play();
        rb.velocity = new Vector2(rb.velocity.x,0f);
        rb.AddForce(dashPower * dir, ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.gravityScale = gravity;
    }
    private void ChangeColor()
    {
        if(dashCount>0){
            spriteRenderer.color = Color.blue;
        }
        else{
            spriteRenderer.color = Color.gray;
        }
    }
}
