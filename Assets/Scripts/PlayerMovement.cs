using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpForce = 1.5f;

    [Header("Ground")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("References")]
    public MouseLook mouseLook;
    public GameObject playerCamera, weapon, playerBody;
    public Transform cameraPivot;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    PlayerHealth health;
    public bool isDead = false;


    NetworkVariable<float> netYRot = new NetworkVariable<float>();
    NetworkVariable<float> netXRot = new NetworkVariable<float>();

    public float bodyRotationOffset = 90f;

    Animator anim;


    float inputX;
    float inputY;
    bool inputJump;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        health = GetComponent<PlayerHealth>();
        anim = GetComponentInChildren<Animator>(); // 🔥 IMPORTANTE
    }

    public override void OnNetworkSpawn()
    {
        controller.enabled = IsOwner;

        if (!IsOwner)
        {
   
            playerBody.SetActive(true);
            playerCamera.SetActive(false);
            weapon.SetActive(true);

            AudioListener al = GetComponentInChildren<AudioListener>();
            if (al != null) al.enabled = false;

            mouseLook.isOwner = false;
            return;
        }


        playerBody.SetActive(false);
        mouseLook.isOwner = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        bool isLocal = IsOwner && IsClient;

        if (playerCamera.activeSelf != isLocal)
            playerCamera.SetActive(isLocal);

        if (weapon.activeSelf != isLocal)
            weapon.SetActive(isLocal);

        mouseLook.isOwner = isLocal;
    }

    void Update()
    {
        controller.enabled = true;

      
        if (!IsOwner)
        {
            transform.rotation = Quaternion.Euler(0f, netYRot.Value, 0f);

            cameraPivot.localRotation = Quaternion.Euler(netXRot.Value, 0f, 0f);
            weapon.transform.localRotation = Quaternion.Euler(netXRot.Value, 0f, 0f);

            playerBody.transform.rotation =
                transform.rotation * Quaternion.Euler(0f, bodyRotationOffset, 0f);

            return;
        }


        if (isDead || health.isRespawning) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        bool jump = Input.GetButtonDown("Jump");

        float yRot = playerCamera.transform.eulerAngles.y;

        cameraPivot.localRotation = Quaternion.Euler(mouseLook.xRotation, 0f, 0f);

     
        SendInputServerRpc(x, y, jump, yRot, mouseLook.xRotation);

        playerBody.transform.rotation =
            transform.rotation * Quaternion.Euler(0f, bodyRotationOffset, 0f);

     
    }
    void FixedUpdate()
{
    if (!IsServer) return;  

    MoveServer(inputX, inputY, inputJump);
}
   
    [ServerRpc]
    void SendInputServerRpc(float x, float y, bool jump, float rotY, float rotX)
    {
        inputX = x;
        inputY = y;
        inputJump = jump;

        transform.rotation = Quaternion.Euler(0f, rotY, 0f);
        netYRot.Value = rotY;
        netXRot.Value = rotX;
    }

   
    void MoveServer(float x, float y, bool jump)
    {
        float dt = Time.deltaTime;

     
        bool groundCheckSphere = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = controller.isGrounded || groundCheckSphere;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = right * x + forward * y;
        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * dt);

        if (jump && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            anim.SetTrigger("Jump");
        }

        velocity.y += gravity * dt;

        controller.Move(new Vector3(0, velocity.y, 0) * dt);


        anim.SetBool("IsGrounded", isGrounded);

        float speedValue = new Vector2(x, y).magnitude;
        anim.SetFloat("Speed", speedValue, 0.1f, Time.deltaTime);
    }
}