
using System.Collections;
using System.Security;
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
    [Header("Animations")]
    public Animator animator;
    public AttackInfo[] attacks;

    [Header("Attacks")]
    public float attackCooldown = 2.5f;


    private float attackTimer;
    private float dashTimer;
    private bool isGrounded;
    private Rigidbody rb;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private int attackIndex;
    
    
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
        
        if(Input.GetMouseButtonDown(0) && attackTimer<=0){
            StartCoroutine(Attack());
        }

        if(moveInput != Vector3.zero){
            var targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*10f);
        }
        dashTimer-=Time.deltaTime;
        attackTimer -= Time.deltaTime;

        animator.SetBool("IsMoving", moveInput != Vector3.zero);
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
        var direction = moveInput != Vector3.zero ? moveInput : Vector3.forward;

        //condition ? if true : if false 
        //moveInput != Vector3.zero ? moveInput : Vector3.forward;
        //if(moveInput != Vector3.zero){
        //    direction = moveInput;
        //}
        //else{
        //    direction = Vector3.forward;
        //}
        
        rb.AddForce(direction * dashForce, ForceMode.Impulse);
    }

    IEnumerator Attack()
    {
        attackTimer = attackCooldown;
        animator.Play(attacks[attackIndex].name);

        yield return new WaitForSeconds(attacks[attackIndex].delay);

        if(attacks[attackIndex].vfx!= null)
        {

            if(attacks[attackIndex].target!=null)
            {
                Instantiate(attacks[attackIndex].vfx, attacks[attackIndex].target.transform.position, attacks[attackIndex].target.transform.rotation);
            }
            else
            {
                Instantiate(attacks[attackIndex].vfx, transform.position, Quaternion.identity);
            }
        }

        attackIndex++;
        attackIndex %= attacks.Length;
    }
}
