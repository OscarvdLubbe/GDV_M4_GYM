using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private InputActionAsset input;
    [SerializeField] private string actionMapName = "Player1";
    private Animator animator;
    private InputActionMap map;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    [SerializeField] private float sprintMultiplier = 2f;
    //private Rigidbody rb;
    private CharacterController cc;
    private float movespeed = 400f;
    [SerializeField] private float jumpHeight = 2f;
    private float jumpForce = 200f;
    private bool Grounded;
    private float verticalVelocity;
    [SerializeField] private float gravity = -20f;


    void Awake()
    {
        map = input.FindActionMap(actionMapName);
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");

        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        map.Enable();
    }
    void OnDisable()
    {
        map.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        cc= GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.isGrounded)
        {
            verticalVelocity = -1f; // kleine downward force om grounded te blijven

            if (jumpAction.WasPressedThisFrame())
            {
                // Sprong-formule: v = sqrt(2 * |g| * h)
                verticalVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * jumpHeight);
                animator.SetTrigger("JumpTrigger");
            }
        }
        else
        {
            // Niet op de grond: zwaartekracht toepassen
            verticalVelocity += gravity * Time.deltaTime;
        }
        if (sprintAction.WasPerformedThisFrame())
        {
            movespeed *= sprintMultiplier;
        }
        animator.SetFloat("Speed", movespeed);

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        float currentMoveSpeed = moveInput.y * movespeed * Time.deltaTime;
        cc.Move(transform.forward * currentMoveSpeed * Time.deltaTime);
        
        //transform.Translate(transform.forward * currentMoveSpeed, Space.World);
        transform.Rotate(Vector3.up , moveInput.x * Time.deltaTime * 100f, Space.World);

        animator.SetFloat("Speed", currentMoveSpeed);
        Debug.Log(Grounded);

        
    }
    
    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.collider.CompareTag("Ground"))
    //     {
    //         Grounded = true;   
    //     }
    // }
    // void OnCollisionExit(Collision collision)
    // {
    //     if (collision.collider.CompareTag("Ground"))
    //     {
    //         Grounded = false;   
    //     }
    // }
}


