using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class KarakterController : MonoBehaviour
{
    public static KarakterController instance;


    private Rigidbody rb;
    private CharacterController cc;
    public Animator anim;
    private Camera playerCamera;


    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private AudioSource audioSource;

    public float moveSoundVelocity = 1f;

    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private float gravityValue = 9.81f;

    public Transform holdPoint;

    [Header("Karakter Özellikleri")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float shiftSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] public bool canMove;


    [Header("Footstep Parameters")]
    [SerializeField] private bool useFootsteps = true;
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float crouchStepMultipler = 1.5f;
    [SerializeField] private float sprintStepMultipler = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    private float footstepTimer = 0;
    private float GetCurrent0ffset => crouching ? baseStepSpeed * crouchStepMultipler : IsSprinting ? baseStepSpeed * sprintStepMultipler : baseStepSpeed;

    public float jumpHeight = 3f;

    public Vector3 movement;
    float x = 0;
    float z = 0;
    public bool isGrounded;

    public bool esc;
    public bool crouching;
    public bool walking;
    public bool IsSprinting;

    float currentSpeed;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
        playerCamera = Camera.main;
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        Move();
        if (useFootsteps)
            Handle_Footsteps();
    }



    private void Move()
    {


        groundedPlayer = cc.isGrounded;


        
        if (canMove)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        if (x != 0 || z != 0)
            walking = true;
        else
            walking = false;

        anim.SetFloat("hor", x);
        anim.SetFloat("ver", z);
        anim.SetBool("crouch", crouching);
        anim.SetBool("walking", walking);



        if (groundedPlayer)
        {
            movement = new Vector3(x, 0, z);
            movement = transform.TransformDirection(movement);
            movement *= currentSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                movement.y = jumpHeight;
            }
        }
        else
        {
            movement.x = x * currentSpeed;
            movement.z = z * currentSpeed;

            movement = transform.TransformDirection(movement);
        }
        if (!crouching && Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = shiftSpeed;
            IsSprinting = true;
        }

        if (!crouching && Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = moveSpeed;
            IsSprinting = false;
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouching = true;
            currentSpeed = crouchSpeed;
            transform.DOKill();
            anim.transform.DOKill();
            transform.DOScaleY(1f, 1f);
            anim.transform.DOScale(1.5f, 1f);
            //FPSCAMController.instance.transform.DOLocalMove(new Vector3(-0.154f, 0.63f, 1.015f), 1f);
            anim.transform.DOLocalMoveZ(-0.7f, 1f);
            anim.transform.DOLocalRotate(new Vector3(-15, 0, 0), 1f);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouching = false;
            currentSpeed = moveSpeed;
            transform.DOKill();
            anim.transform.DOKill();
            transform.DOScaleY(1.4f, 1f);
            anim.transform.DOScale(1.1f, 1f);
            //FPSCAMController.instance.transform.DOLocalMove(new Vector3(0.003f, 0.868f, 0.03f), 1f);
            anim.transform.DOLocalMoveZ(0, 1f);
            anim.transform.DOLocalRotate(Vector3.zero, 1f);

        }

        // Apply gravity
        movement.y -= gravityValue * 4 * Time.deltaTime;
        cc.Move(movement * Time.deltaTime);

        /*if (isGrounded && movement.sqrMagnitude > moveSoundVelocity && !audioSource.isPlaying)
        {
            float pitch = UnityEngine.Random.Range(0.8f, 1.1f);
            //PV.RPC("PlayFootStep", RpcTarget.All, pitch);
        }*/

        if (transform.position.y <= -20f)
            transform.position = GameManager.instance.ItemSpawnPoint.position;
    }

    private void Handle_Footsteps()
    {
        if (!cc.isGrounded) return;
        if (x == 0 && z == 0) return;
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0)
        {
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[UnityEngine.Random.Range(0, woodClips.Length)]);
                        break;
                    case "Footsteps/METAL":
                        footstepAudioSource.PlayOneShot(metalClips[UnityEngine.Random.Range(0, metalClips.Length)]);
                        break;
                    case "Footsteps/GRASS":
                        footstepAudioSource.PlayOneShot(grassClips[UnityEngine.Random.Range(0, grassClips.Length)]);
                        break;
                    default:
                        footstepAudioSource.PlayOneShot(grassClips[UnityEngine.Random.Range(0, grassClips.Length)]);
                        break;
                }
            }
            
            footstepTimer = GetCurrent0ffset;
        }
    }

    public void ResetAxis()
    {
        x = 0;
        z = 0;
    }
}
