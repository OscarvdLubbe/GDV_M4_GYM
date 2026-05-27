using System.Runtime.CompilerServices;
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
    private Rigidbody rb;
    private float movespeed = 5f;
    private float jumpForce = 200f;
    private bool Grounded;


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
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpAction.WasPressedThisFrame() && Grounded == true)
        {
            Debug.Log("Jump has been pressed");
            rb.AddForce(Vector3.up * jumpForce , ForceMode.Force);
            animator.SetTrigger("JumpTrigger");
        }
        else if (jumpAction.IsPressed())
        {
            Debug.Log("Jump held");
        }
        else if (jumpAction.WasReleasedThisFrame())
        {
            Debug.Log("no more jump");
        }
        animator.SetFloat("Speed", movespeed);

        
        

        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        float currentMoveSpeed = moveInput.y * movespeed * Time.deltaTime;

        transform.Translate(transform.forward * currentMoveSpeed, Space.World);
        transform.Rotate(Vector3.up , moveInput.x * Time.deltaTime * 100f, Space.World);

        animator.SetFloat("Speed", currentMoveSpeed);
        Debug.Log(Grounded);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Grounded = true;   
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Grounded = false;   
        }
    }
}
