using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Player")]
    private CharacterController characterController;
    public Vector2 input_Movement;
    public Vector2 input_View;
    private DefaultInput defaultInput;

    private Vector3 cameraRotation;
    private Vector3 playerRotation;
    


    [Header("Game Object References")]
    public Transform cameraObject;
    public Transform feetTransform;
    public LayerMask playerMask;
    public LayerMask groundMask;



    public PlayerSettings playerSettings;


    [Header("View Clamping")]
    public float viewLimitYmin;
    public float viewLimitYmax;


    [Header("Player Status")]
    public PlayerStance playerStance;
    public CharacterStance playerStandStance;
    public CharacterStance playerCrouchStance;
    public CharacterStance playerProneStance;
    public float playerStanceSmoothing;
    private float stanceCheckErrorMargin = 0.05f;


    [Header("Camera")]
    private float cameraHeight;
    private float cameraHeightVelocity;


    [Header("Gravity")]
    public float gravityForce;
    public float gravityMin;
    public float playerGravity;


    [Header("Jump")]
    public float newJump;
    public Vector3 jumpForce;
    private Vector3 jumpForceVelocity;
    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;
    public bool isSprinting;
    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;


    [Header("Weapon")]
    public WeaponController currentWeapon;
    public float gunAnimationSpeed;
    public bool isGrounded;
    public bool isFalling;


    [Header("Aiming")]
    public bool isAimingIn;
    public bool isFiring;

    [Header("Audio")]
    public AudioSource walkingSound;
    public AudioSource runningSound;

    [Header("Damage")]
    public GameObject hitScreen;
    public float maxhealth;
    public float health;
    public Transform startPoint;
    public HealthBar healthBar;

    private void Awake() {

        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        defaultInput = new DefaultInput();
        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => input_View = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => Jump();
        defaultInput.Character.Crouch.performed += e => Crouch();
        defaultInput.Character.Prone.performed += e => Prone();
        defaultInput.Character.Sprint.performed += e => TogggleSprint();
        defaultInput.Character.SprintRelease.performed += e => StopSprint();
        defaultInput.Weapon.Fire2Pressed.performed += e => AimingInPressed();
        defaultInput.Weapon.Fire2Released.performed += e => AimingInReleased();
        defaultInput.Weapon.Fire1Pressed.performed += e => FireInPressed();
        defaultInput.Weapon.Fire1Released.performed += e => FireInReleased();
        defaultInput.Weapon.Reload.performed += e => currentWeapon.ReloadNest();

        defaultInput.Enable();

        cameraRotation = cameraObject.localRotation.eulerAngles;
        playerRotation = cameraObject.localRotation.eulerAngles;

        cameraHeight = cameraObject.localPosition.y;

        if(currentWeapon){
            currentWeapon.Initialise(this);
        }
    }

    private void Start() {
        Time.timeScale = 1f;
        health = maxhealth;
        healthBar.ChangeHealthBar(maxhealth,health);
    }

    private void Update() {

        SetIsGrounded();
        SetIsFalling();
        CalculateStance();
        CalculateAimingIn();
        CalculateView();
        ResetHurtScreen();

        
    }

    void FixedUpdate()
    {
        CalculateMovement();
        CalculateJump();
  
        
    }

    private void AimingInPressed(){

        isAimingIn = true;

    }

    private void AimingInReleased(){

        isAimingIn = false;

    }

    private void FireInPressed(){

        isFiring = true;

    }

    private void FireInReleased(){

        isFiring = false;

    }

    private void CalculateAimingIn(){

        if(!currentWeapon){
            return;
        }

        currentWeapon.isAimingIn = isAimingIn;

    }

    private void CalculateView(){

        playerRotation.y += (isAimingIn ? playerSettings.ViewXSensitivity * playerSettings.AimingSensitivityEffector : playerSettings.ViewXSensitivity) * (playerSettings.ViewXInverted ? input_View.x : -input_View.x) * Time.deltaTime;
        cameraRotation.x += (isAimingIn ? playerSettings.ViewYSensitivity * playerSettings.AimingSensitivityEffector : playerSettings.ViewYSensitivity) * (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;
  

        cameraRotation.x = Mathf.Clamp(cameraRotation.x,viewLimitYmin,viewLimitYmax);
        
        cameraObject.localRotation = Quaternion.Euler(cameraRotation);
        transform.localRotation = Quaternion.Euler(playerRotation);

    }

    private void CalculateMovement(){

        if(input_Movement.y <= 0.2f){
            isSprinting = false;
        }

        if(gunAnimationSpeed > 0.1f){
            walkingSound.enabled = true;
        }else walkingSound.enabled = false;



        var verticalSpeed = playerSettings.forwardSpeed;
        var horizontalSpeed = playerSettings.strafeSpeed;

        if(isSprinting){
            verticalSpeed = playerSettings.runningForwardSpeed;
            horizontalSpeed = playerSettings.runningStrafeSpeed;
            runningSound.enabled = true;
            walkingSound.enabled = false;
        }else runningSound.enabled = false;

        if(!isGrounded){
            playerSettings.SpeedEffector = playerSettings.fallingSpeedEffector;
        } else if(playerStance == PlayerStance.Crouch){
            playerSettings.SpeedEffector = playerSettings.CrouchSpeedEffector;
        }else if(playerStance == PlayerStance.Prone){
            playerSettings.SpeedEffector = playerSettings.ProneSpeedEffector;
        }else if(isAimingIn){
            playerSettings.SpeedEffector = playerSettings.aimingSpeedEffector;
        }else{
            playerSettings.SpeedEffector = 1;
        }

        gunAnimationSpeed = characterController.velocity.magnitude / (playerSettings.forwardSpeed * playerSettings.SpeedEffector);

        if(gunAnimationSpeed > 1){
            gunAnimationSpeed = 1;
        }

        verticalSpeed *= playerSettings.SpeedEffector;
        horizontalSpeed *= playerSettings.SpeedEffector;

        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed,new Vector3(horizontalSpeed* input_Movement.x * Time.deltaTime,0,verticalSpeed* input_Movement.y * Time.deltaTime), ref newMovementSpeedVelocity, isGrounded ? playerSettings.MovementSmoothing : playerSettings.fallingSmoothing);
        Vector3 movementSpeed =  transform.TransformDirection(newMovementSpeed);

        if(playerGravity > gravityMin){
            playerGravity -= gravityForce * Time.deltaTime;
        }

        if(playerGravity < -0.1f && isGrounded){
            playerGravity = -0.1f;
        }

        movementSpeed.y += playerGravity;
        movementSpeed += jumpForce * Time.deltaTime;
        characterController.Move(movementSpeed);
    }

    private void CalculateJump(){
        jumpForce = Vector3.SmoothDamp(jumpForce, Vector3.zero, ref jumpForceVelocity, playerSettings.JumpingFallOff);
    }

    private void Jump(){
        if(!isGrounded){
            return;
        }

        if(playerStance == PlayerStance.Crouch || playerStance == PlayerStance.Prone){
            if(StanceCheck(playerStandStance.StanceCollider.height)){
                return;
            }
            playerStance = PlayerStance.Stand;
            return;
        }
        jumpForce = Vector3.up * (playerSettings.JumpingHeight);
        playerGravity = 0;
        // currentWeapon.TriggerJump();
    }

    private void CalculateStance(){

        var currentStance = playerStandStance;

        if(playerStance == PlayerStance.Crouch){

            currentStance = playerCrouchStance;

        }else if(playerStance == PlayerStance.Prone){

            currentStance = playerProneStance;

        }
        cameraHeight = Mathf.SmoothDamp(cameraObject.localPosition.y,currentStance.cameraHeight,ref cameraHeightVelocity,playerStanceSmoothing);
        cameraObject.localPosition = new Vector3(cameraObject.localPosition.x,cameraHeight,cameraObject.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.StanceCollider.height, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.StanceCollider.center, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    private void Crouch(){
       if(playerStance == PlayerStance.Crouch){

            if(StanceCheck(playerStandStance.StanceCollider.height)){
                return;
            }

            playerStance = PlayerStance.Stand;
            return;
       }

        if(StanceCheck(playerCrouchStance.StanceCollider.height)){
            return;
        }
        playerStance = PlayerStance.Crouch;
    }

    private void Prone(){
        if(playerStance == PlayerStance.Prone){

            if(StanceCheck(playerStandStance.StanceCollider.height)){
                return;
            }
            
            playerStance = PlayerStance.Stand;
            return;
        }
        playerStance = PlayerStance.Prone;

    }

     private bool StanceCheck(float stanceCheckHeight){
        var start  = new Vector3(feetTransform.position.x,feetTransform.position.y + characterController.radius + stanceCheckErrorMargin,feetTransform.position.z);
        var end = new Vector3(feetTransform.position.x,feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + stanceCheckHeight ,feetTransform.position.z);

        return Physics.CheckCapsule(start, end, characterController.radius,playerMask);


     }

     private void TogggleSprint(){
        
        if(input_Movement.y <= 0.2f){
            isSprinting = false;
            return;
        }
        isSprinting = !isSprinting;
     }

    private void StopSprint(){
        if(playerSettings.sprintingHold){
            isSprinting = false;
        }
     }

    private void OnDrawGizmos() {
       Gizmos.DrawWireSphere(feetTransform.position, playerSettings.isGroundedRadius); 
    }

    private void SetIsGrounded(){

        isGrounded = Physics.CheckSphere(feetTransform.position, playerSettings.isGroundedRadius, groundMask);

    }

    private void SetIsFalling(){

        isFalling = (!isGrounded && characterController.velocity.magnitude >= playerSettings.isFallingSpeed);

        
    }

    public void GotHurt(float damage){

        var color = hitScreen.GetComponent<Image>().color;
        color.a = 0.8f;
        health -= damage;
        healthBar.ChangeHealthBar(maxhealth,health);
        hitScreen.GetComponent<Image>().color = color;
  
    }

    private void ResetHurtScreen(){
        if(hitScreen == null){
            return;
        }

        if(hitScreen.GetComponent<Image>().color.a > 0){
            var color = hitScreen.GetComponent<Image>().color;
            color.a -= 0.01f;
            hitScreen.GetComponent<Image>().color = color;
        }
    }
}
