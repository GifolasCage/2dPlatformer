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
    private float wallJumpTimeTimer;
    private bool dashPressed;
    private bool isDashing;
    private Vector2 dashDir;
    private bool isFacingRight;
    private bool isWallSliding;
    public int dashCount;
    private int extraJumpCount;
    private float lastVelocity;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxFallSpeed = 30f;
    [SerializeField] private float maxHorizontalSpeed = 30f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deAcceleration = 10f;
    [SerializeField] private float wallSlide = 1f;
    [SerializeField] private float velPower = 10f;
    [SerializeField] private float gravityScaleGrounded = 1f;
    [SerializeField] private float gravityScaleFalling = 12f;
    [Header("Jump Settings")]
    [SerializeField] private float jumpStrength = 30f;
    [SerializeField] private float extraJumpStrength = 30f;
    [SerializeField] private float wallJumpStrengthY = 30f;
    [SerializeField] private float wallJumpStrengthX = 30f;
    [SerializeField] private int maxExtraJumpCount = 1;

    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float wallJumpTime = 0.1f;
    [SerializeField] private float apexModifier = 30f;
    [Header("Dash Settings")]
    [SerializeField] private float dashPower = 500f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] public int maxDashCount = 1;
    [Header("Initilization")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private ParticleSystem dashParticles;
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private Transform snowParticlesTransform;
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
        MoveSnow();

        if(move.x < 0 && !isFacingRight && !isWallSliding){
            FlipThePlayer();
        }
        if(move.x > 0 && isFacingRight && !isWallSliding){
            FlipThePlayer();
        }

        //Check if the player is grounded
        if(isGrounded() && !isWallSliding){
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

        //Dash
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

        //Check if the player is wallsliding
        if(isWallSliding){
            wallJumpTimeTimer = wallJumpTime;
        }
        else{
            wallJumpTimeTimer -= Time.deltaTime;
        }
    }

    private void MoveSnow()
    {
        snowParticlesTransform.position = new Vector3(transform.position.x, transform.position.y + 20f, 0f);
    }

    private void FixedUpdate() {
        float targetSpeed = move.x * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deAcceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
        //Handle horizontal movement
        if(!isDashing){
            rb.AddForce(movement*Vector2.right);
        }

        //Clamp the fall & horizontal speed
        if(rb.velocity.y < -maxFallSpeed && !isDashing){
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        }
        if(rb.velocity.x > maxHorizontalSpeed){
            rb.velocity = new Vector2(maxHorizontalSpeed, rb.velocity.y);
        }
        if(rb.velocity.x < -maxHorizontalSpeed){
            rb.velocity = new Vector2(-maxHorizontalSpeed, rb.velocity.y);
        }

        //Check if the player is on wall
        if(isOnWall()){
            rb.velocity = new Vector2(rb.velocity.x, -wallSlide);
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

        //Handle extra jumps
        if(jumpPressed && !isGrounded() && !isWallSliding){
            if(extraJumpCount>0){
                extraJumpCount -= 1;
                ExtraJump();
            }
        }
        //Handle jumpbutton released
        if(jumpReleased){
            rb.gravityScale = gravityScaleFalling;
            coyoteTimeTimer = 0f;
        }

        //Handle walljump
        if(jumpPressed && wallJumpTimeTimer>0){
            WallJump();
        }
    }

    //Gather input
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

        RaycastHit2D hit = Physics2D.Raycast(position, direction,rayDistance,groundLayer | wallLayer);
        if(hit.collider!= null){
            extraJumpCount = maxExtraJumpCount;
            return true;
        }
        return false;
    }

    //Check if the player is against a wall and pushing towards it
    bool isOnWall(){
        Vector2 position = transform.position;
        Vector2 direction = Vector2.right;
        float rayDistance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(position, -direction,rayDistance,wallLayer);
        if(hit.collider!= null && move.x < -0.5 && isFacingRight && rb.velocity.y < apexModifier){
            isWallSliding  = true;
            return true;
        }
        RaycastHit2D hit2 = Physics2D.Raycast(position, direction,rayDistance,wallLayer);
        if(hit2.collider!= null && move.x > 0.5 && !isFacingRight && rb.velocity.y < apexModifier){
            isWallSliding  = true;
            return true;
        }
        isWallSliding = false;
        return false;
    }

    void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
        dustParticles.Play();
    }

    void ExtraJump(){
        rb.velocity = new Vector2(rb.velocity.x, extraJumpStrength);
        dustParticles.Play();
    }

    void WallJump(){
        dustParticles.Play();
        if(isWallSliding){
            rb.velocity = new Vector2(-transform.localScale.x * wallJumpStrengthX, wallJumpStrengthY);
        }
        else{
            rb.velocity = new Vector2(rb.velocity.x, wallJumpStrengthY);
            extraJumpCount = maxExtraJumpCount;
        }
    }

    void FlipThePlayer(){
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        if(isGrounded()){
            dustParticles.Play();
        }

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
