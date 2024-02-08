using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{

    public WeaponSettingsModel settings;

    private PlayerController playerController;

    public Animator gunAnimator;

    bool isInitialised;
    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;
    
    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    
    Vector3 newWeaponMovementRotation;
    Vector3 newWeaponMovementRotationVelocity;
    
    Vector3 targetWeaponMovementRotation;
    Vector3 targetWeaponMovementRotationVelocity;
    public Transform weaponSwayObject;

    [Header("Firing")]
    private int maxClip = Gun.magSize;
    private float damage = Gun.damage;
    private float fireRate = Gun.fireRate;
    private int maxAmmo = 100;
    public int currentAmmo;

    public int currentClip;
    public float reloadTime;
    public bool isReloading = false;
    public float range;
    public Camera cam;
    public List<GameObject> impact;
    GameObject impactObject;

    private float nextTimeToFire = 0f;
    private ParticleSystem muzzleFlash;
    public LoadGun loadGun;
    public AudioSource shootSound;
    public AudioSource noAmmoSound;
    public AudioSource reloadSound;
    public Image dot;
    public Text ammoCount;
    public Text clipCount;

    [Header("Recoil")]
    private Recoil recoil_Script;

    #region Variables for weapon breathing
    // [Header("Weapon Breathing")]

    // public float swayAmountA = 1;
    // public float swayAmountB = 2;
    // public float swayScale = 600;
    // public float swayLerpSpeed = 14;
    // float swayTime;
    // Vector3 swayPosition;
    // private bool isGroundedTrigger;
    // private float fallingDelay;

    #endregion


    [Header("Sights")]
    public bool isAimingIn;
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;

    private void Start() {
        newWeaponRotation = transform.localRotation.eulerAngles;
        recoil_Script = GameObject.Find("CameraRecoil").GetComponent<Recoil>();
        currentAmmo = maxAmmo;
        currentClip = maxClip;
        ammoCount.text = currentAmmo.ToString();
        clipCount.text = currentClip.ToString();
        loadGun.GetGun();
        muzzleFlash = loadGun.muzzleFlash;
    }
    public void Initialise(PlayerController playerController){
        this.playerController = playerController;
        isInitialised = true;
    }

    private void Update() {
        if(!isInitialised){
            return;
        }
        dot.enabled = true;


        if(playerController.isAimingIn){
            playerController.isSprinting = false;
            dot.enabled = false;
        }

        if(isReloading){
            playerController.isSprinting = false;
        }

        if(playerController.isSprinting){
            dot.enabled =false;

        }

        CalculateGunRotation();
        SetGunAnimations();
        // CalculateGunSway();

        CalculateAimingIn();

        
        if(currentClip > 0 && isReloading == false){
            gunFire();
        }

    }

    public void ReloadNest(){
        StartCoroutine(Reload());
    }

    public IEnumerator Reload(){
        if(currentClip != maxClip && currentAmmo > 0){
            isReloading = true;
            reloadSound.Play();
            yield return new WaitForSeconds(reloadTime - .25f);
            isReloading = false;
            yield return new WaitForSeconds(.25f);
            int reloadAmount = maxClip - currentClip;
            reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
            currentClip += reloadAmount;
            currentAmmo -= reloadAmount;
            ammoCount.text = currentAmmo.ToString();
            clipCount.text = currentClip.ToString();
        }
    }

    private void gunFire(){
        GameObject impactObject;
        if(playerController.isFiring && Time.time >= nextTimeToFire && !playerController.isSprinting){
            currentClip--;
            clipCount.text = currentClip.ToString();
            nextTimeToFire = Time.time + 1f/fireRate;
            muzzleFlash.Play();
            shootSound.Play();
            recoil_Script.RecoilFire();
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();


                if(enemy != null){
                    enemy.isAlert = true;
                    enemy.TakeDamage(damage);
                    enemy.StopChasing();

                }

                foreach (GameObject item in impact){
                    if(enemy){
                        impactObject = Instantiate(impact[0], hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impactObject, 2f);
                    } else{
                        impactObject = Instantiate(impact[1], hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impactObject, 2f);
                    }
                }

                

            }
            if(maxAmmo <= 0){
                noAmmoSound.Play();
            }
        }

    }

    private void CalculateAimingIn(){
        var targetPosition = transform.position;

        if(isAimingIn && !isReloading){
            targetPosition = playerController.cameraObject.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (playerController.cameraObject.transform.forward * sightOffset);
        }



        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);

        weaponSwayObject.transform.position = weaponSwayPosition;
    }

    #region Old code for jump animation trigger
    // public void TriggerJump(){

    //     isGroundedTrigger = false;
    //     Debug.Log("Jump");
    //     gunAnimator.SetTrigger("Jump");
        
    // }
    #endregion

    private void CalculateGunRotation(){

        targetWeaponRotation.x += (isAimingIn ? settings.swayAmount / 3 : settings.swayAmount) * (settings.SwayYInverted ? playerController.input_View.y : -playerController.input_View.y) * Time.deltaTime;
        targetWeaponRotation.y += (isAimingIn ? settings.swayAmount / 3 : settings.swayAmount) * (settings.SwayXInverted ? -playerController.input_View.x : playerController.input_View.x) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x,-settings.SwayClampX,settings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.x,-settings.SwayClampY,settings.SwayClampY);

        targetWeaponRotation.z = targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation,Vector3.zero, ref targetWeaponRotationVelocity, settings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, settings.swaySmoothing);

        targetWeaponMovementRotation.z = (isAimingIn ? settings.MovementSwayX / 2 : settings.MovementSwayX) * (settings.MovementSwayXInverted ? -playerController.input_Movement.x: playerController.input_Movement.x);
        targetWeaponMovementRotation.x = (isAimingIn ? settings.MovementSwayY  / 2 : settings.MovementSwayY) * (settings.MovementSwayYInverted ? -playerController.input_Movement.y: playerController.input_Movement.y);

        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation,Vector3.zero, ref targetWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, settings.MovementSwaySmoothing);


        
        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);

    }

    private void SetGunAnimations(){
        #region Old code for jump animations
            
        
        // if(isGroundedTrigger){
        //     fallingDelay = 0;
        // }
        // else{
        //     fallingDelay += Time.deltaTime;
        // }

        // if(playerController.isGrounded && !isGroundedTrigger && fallingDelay > 0.1f){
        //     Debug.Log("land");
        //     gunAnimator.SetTrigger("Land");
        //     isGroundedTrigger = true;
        // }
        // else if (!playerController.isGrounded && isGroundedTrigger){
        //     Debug.Log("Falling");
        //     gunAnimator.SetTrigger("Fall");
        //     isGroundedTrigger = false;
        // }
        #endregion


        gunAnimator.SetBool("isRunning", playerController.isSprinting);
        gunAnimator.SetBool("isAiming", playerController.isAimingIn);
        gunAnimator.SetFloat("gunAnimationSpeed",playerController.gunAnimationSpeed);
        gunAnimator.SetBool("isReloading",isReloading);
    }

    #region Old Code for weapon breathing
    // private void CalculateGunSway(){
    //     var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;

    //     swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
    //     swayTime += Time.deltaTime;

    //     if(swayTime > 6.3f){
    //         swayTime = 0;
    //     }
        
        //weaponSwayObject.localPosition = swayPosition;
        
    // }

    // private Vector3 LissajousCurve(float Time, float A, float B)
    // {
    // return new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    // }

    #endregion

}
