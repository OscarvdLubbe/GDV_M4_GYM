using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCharacterController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputAsset;
    [SerializeField] private string mapName = "Player";
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;


    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private CharacterController characterController;
    private Animator animator;
    private InputActionMap map;
    private float verticalVelocity;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        InputActionMap map  = inputAsset.FindActionMap(mapName);
        moveAction          = map.FindAction("Move");
        sprintAction        = map.FindAction("Sprint");
        jumpAction          = map.FindAction("Jump");
    }

    void OnEnable()
    {
        map.Enable(); 
    }
    void OnDisable()
    {
        map.Disable();
    }

    void Update()
    {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        float speed = movementInput.y * moveSpeed;
        // Sprint
        if (sprintAction.IsPressed())
            speed *= sprintMultiplier;

        // Voorwaartse beweging wordt later verwerkt in de Move methode
        Vector3 move = transform.forward * speed * Time.deltaTime;

        // Rotatie (links/rechts draaien)
        transform.Rotate(Vector3.up * movementInput.x * rotationSpeed * Time.deltaTime);

        // Zwaartekracht en springen
        if (characterController.isGrounded)
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


        //verticale snelheid meegeven via de move vector
        move.y = verticalVelocity * Time.deltaTime;

        characterController.Move(move);

        // Animator aansturen voor rennen en landen
        animator.SetFloat("Speed", sprintMultiplier);
        animator.SetBool("Grounded", characterController.isGrounded);
    }


}
