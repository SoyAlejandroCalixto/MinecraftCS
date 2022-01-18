using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class steve : MonoBehaviour
{
    #region Vars
    Animator runAnimation;
    public Animator HandAnimation;
    public BoxCollider SteveCollider;
    public BoxCollider DownCollider;
    public LayerMask PlayerLayer;
    public ParticleSystem BreakingParticle;
    public GameObject PauseCanvas;
    public GameObject hudCanvas;
    Transform mainCam;
    Vector3 Movement;
    bool isOnTheGround;
    float fallingForceCooldown;
    float jumpCooldown;
    float Creative_BreakingBlocksCooldown;
    public float Velocity;
    public float Sensibility;
    public int FOV;
    public int FPSLimit;
    float punchAnimCooldown;
    float rotY;
    public KeyCode sprintingKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode shiftKey = KeyCode.LeftShift;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        runAnimation = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = transform.GetChild(6);
        Application.targetFrameRate = FPSLimit;
    }

    private void Awake()
    {
        Camera.main.fieldOfView = FOV;
    }

    // Update is called once per frame
    private void Update()
    {
        #region Animations
        runAnimation.SetBool("isRunning", Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        HandAnimation.SetBool("isRunning", Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        HandAnimation.SetBool("isOnTheGround", isOnTheGround);
        runAnimation.SetBool("isShifting", Input.GetKey(shiftKey));
        #endregion

        #region WASD and camera movement
        Movement = new Vector3(Input.GetAxisRaw("Vertical"), 0, -Input.GetAxisRaw("Horizontal"));
        Movement = Movement.normalized;

        if (Input.GetAxisRaw("Horizontal") != 0 && !PauseCanvas.activeSelf || Input.GetAxisRaw("Vertical") != 0 && !PauseCanvas.activeSelf) //Movement
        {
            if(Input.GetKey(sprintingKey)) //Movement when sprint
            {
                GetComponent<Rigidbody>().AddRelativeForce(-Movement * Velocity*1.232f);
                if(!Input.GetKey(shiftKey)) HandAnimation.SetFloat("BobbingSpeedMultiplier", 1.35f);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOV+10, 0.35f);
            }
            else //Movement when not sprint
            {
                GetComponent<Rigidbody>().AddRelativeForce(-Movement * Velocity);
                if(!Input.GetKey(shiftKey)) HandAnimation.SetFloat("BobbingSpeedMultiplier", 1f);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOV, 0.35f);
            }
        } else Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOV, 0.35f); //FOV restauration

        if(Input.GetAxisRaw("Mouse X") != 0 && !PauseCanvas.activeSelf || Input.GetAxisRaw("Mouse Y") != 0 && !PauseCanvas.activeSelf) //Camera movement
        {
            rotY -= Sensibility * Input.GetAxisRaw("Mouse Y");
            rotY = Mathf.Clamp(rotY, -90, 90);
            Camera.main.transform.localEulerAngles = new Vector3(rotY, -90, 0);
            transform.Rotate(0, Sensibility * Input.GetAxisRaw("Mouse X"), 0);
        }
        
        #endregion

        #region Punch
        if(Input.GetMouseButtonDown(0) && Time.time > punchAnimCooldown && !PauseCanvas.activeSelf)
        {
            HandAnimation.Play("steve_PunchingHand");
            punchAnimCooldown = Time.time + 0.20f;
        }
        #endregion

        #region Jump
        if (Input.GetKey("space") && isOnTheGround && Time.time > jumpCooldown && !PauseCanvas.activeSelf)
        {
            GetComponent<Rigidbody>().AddForce(0,20,0, ForceMode.Impulse);
            isOnTheGround = false;
            jumpCooldown = Time.time + 0.1f;
            fallingForceCooldown = Time.time + 0.33f;
        }

        if (!isOnTheGround && Time.time > fallingForceCooldown)
        {
            GetComponent<Rigidbody>().AddForce(0,-75,0);
        }
        #endregion
    
        #region Shifting
        if(Input.GetKeyDown(shiftKey) && !PauseCanvas.activeSelf)
        {
            transform.GetChild(6).position = new Vector3(mainCam.position.x, mainCam.position.y-0.4f, mainCam.position.z);
            Velocity = Velocity * 0.25f;
            HandAnimation.SetFloat("BobbingSpeedMultiplier", 0.25f);
            SteveCollider.size = new Vector3(2.1f, 6f, 2.1f);
            SteveCollider.center = new Vector3(0, 1.051354f, 0);
        }
        if(Input.GetKeyUp(shiftKey))
        {
            transform.GetChild(6).position = new Vector3(mainCam.position.x, mainCam.position.y+0.4f, mainCam.position.z);
            Velocity = Velocity * 4f;
            HandAnimation.SetFloat("BobbingSpeedMultiplier", 1f);
            SteveCollider.size = new Vector3(2.1f, 8f, 2.1f);
            SteveCollider.center = new Vector3(0, 2.05f, 0);
        }
        #endregion

        #region breaking blocks
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5, ~PlayerLayer))
        {
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Block") && Input.GetMouseButton(0) && Time.time > Creative_BreakingBlocksCooldown)
            {
                var bp = Instantiate(BreakingParticle, hit.transform.position, BreakingParticle.transform.rotation);
                bp.GetComponent<ParticleSystemRenderer>().material = hit.transform.GetChild(0).GetComponent<MeshRenderer>().material;
                bp.Play();
                Destroy(hit.transform.gameObject);
                Creative_BreakingBlocksCooldown = Time.time + 0.20f;

                HandAnimation.Play("steve_PunchingHand");
                punchAnimCooldown = Time.time + 0.20f;
            }
        }

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 5, Color.red);
        #endregion

        #region Others keybinds
        if(Input.GetKeyDown(KeyCode.F11))
        {
            if(Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
            if(Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(!PauseCanvas.activeSelf)
            {
                PauseCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                hudCanvas.SetActive(false);
                Time.timeScale = 0;
            }
            else
            {
                PauseCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                hudCanvas.SetActive(true);
                Time.timeScale = 1;
            }
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        isOnTheGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isOnTheGround = false;
    }
    private void OnTriggerStay(Collider other) {
        
    }
}