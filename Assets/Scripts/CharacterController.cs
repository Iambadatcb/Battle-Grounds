
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6;
    public float jumpPower = 6f;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance =0.4f;
    public float dashForce = 0.5f;
    public float dashCooldown = 1;

    private float dashTimer;
    private  Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private bool isGrounded;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        moveInput = (transform.right*horizontal + transform.forward*vertical).normalized;
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)&& dashTimer<=0){
            Dash();
        }
        dashTimer-=Time.deltaTime;
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position+moveInput*moveSpeed*Time.fixedDeltaTime);
    }
    void Jump(){
        rb.velocity = new Vector3(moveVelocity.x, 0, moveVelocity.z);
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    void Dash()
    {
        // var direction = moveInput !=Vector3
    }
}
