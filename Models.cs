using System;
using System.Collections.Generic;
using UnityEngine;

public static class Models 
{

    public enum PlayerStance{
        Stand,
        Crouch,
        Prone
    }

    [Serializable]
    public class PlayerSettings{
        [Header("View")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;
        public float AimingSensitivityEffector;

        public bool ViewXInverted;
        public bool ViewYInverted;


        [Header("Movement")]
        public bool sprintingHold;
        public float MovementSmoothing;
        public float runningForwardSpeed;
        public float runningStrafeSpeed;
        public float forwardSpeed;
        public float strafeSpeed;
        public float backwardSpeed;


        [Header("Jump")]
        public float JumpingHeight;
        public float JumpingFallOff;
        public float  fallingSmoothing;


        [Header("Speed Effectors")]
        public float SpeedEffector = 1;
        public float CrouchSpeedEffector;
        public float ProneSpeedEffector;
        public float fallingSpeedEffector;
        public float aimingSpeedEffector;
        
 
        [Header("Ground check")]
        public float isGroundedRadius;
        public float isFallingSpeed;
    }

    [Serializable]
    public class CharacterStance{
        public float cameraHeight;
        public CapsuleCollider StanceCollider;
    }

    [Serializable]
    public class WeaponSettingsModel{
       
        [Header("Weapon Sway")]
        public float swayAmount;
        public float swaySmoothing;
        public bool SwayYInverted;
        public bool SwayXInverted;
        public float SwayResetSmoothing;
        public float SwayClampX;
        public float SwayClampY;

       
        [Header("Weapon Sway while moving")]
        public float MovementSwayX;
        public float MovementSwayY;
        public bool MovementSwayYInverted;
        public bool MovementSwayXInverted;
        public float MovementSwaySmoothing;


    }
}
