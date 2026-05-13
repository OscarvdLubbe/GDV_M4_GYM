using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class InputPlayer : MonoBehaviour
{
    [SerializeField] private InputActionAsset input;
    [SerializeField] private string actionMapName = "Player1";
    private InputActionMap map;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private Rigidbody rb;
    void Awake()
    {
        map = input.FindActionMap(actionMapName);
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
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
        if (jumpAction.WasPressedThisFrame())
        {
            Debug.Log("Jump has been pressed");
        }
        else if (jumpAction.IsPressed())
        {
            Debug.Log("Jump held");
        }
        else if (jumpAction.WasReleasedThisFrame())
        {
            Debug.Log("no more jump");
        }
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        transform.Translate(moveInput.y * transform.forward * Time.deltaTime * 100f, Space.World);
        transform.Rotate(Vector3.up , moveInput.x * Time.deltaTime * 100f, Space.World);
    }
}
