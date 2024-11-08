using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
//Publisher class
public class PlayerControl : MonoBehaviour
{
    #region Tooltip
    [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed")]
    #endregion Tooltip

    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private bool isPlayerRolling = false;
    private float playerRollCooldownTimer = 0f;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        // Create waitforfixed update for use in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        // Set player animationspeed
        SetPlayerAnimationSpeed();

        // Set starting weapon
        SetStartingWeapon();
    }

    /// <summary>
    /// Set player starting weapon <br/>
    /// If weapon details match starting weapon listed in player details, set as current weapon
    /// </summary>
    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }

    /// <summary>
    /// Set player animator speed to match movement speed
    /// </summary>
    private void SetPlayerAnimationSpeed()
    {
        // set animator speed to match movement speed
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimations;
    }

    private void Update()
    {
        // if player is rolling then return, don't want to process other movement
        if (isPlayerRolling) return;

        // Player roll cooldown timer
        PlayerRollCooldownTimer();

        // Process the player movement input
        MovementInput();

        // Process the player weapon input
        WeaponInput();

    }

    /// <summary>
    /// Weapon input
    /// </summary>
    private void WeaponInput()
    {
        Vector3 weaponDirection; // direction vector between cursor and weapon shoot position in player prefab
        float weaponAngleDegrees; // shoot angle between cursor and weapon shoot position
        float playerAngleDegrees; // angle between cursor and pivot point on player transform
        AimDirection playerAimDirection;

        // Aim weapon input
        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection); // 'out' keyword: passed in values are calculated based on the the AimWeaponInput method.
                                                                                                                     // Allows the values to be used again. The method will return the values
                                                                                                                     // it doesn't simply update the values locally within the scope, the values are returned back into the method
                                                                                                                     // these values will be processed by additional methods where these values are needed so the 'out' keyword is required

        // Fire weapon input
        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);

        // Reload weapon input
        ReloadWeaponInput();

        // Switch Weapon Input
        SwitchWeaponInput();
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        // Get mouse world position
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        // calculate direction vector of mouse cursor from weapon shoot position
        weaponDirection = (mouseWorldPosition - player.activeWeapon.GetShootPosition());

        // Calculate direction vector of mouse cursor from player transform position
        // because this player control component is attached to the player prefab, the transform position will be the player position
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        // Get weapon to cursor angle
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        // Get player to cursor angle
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        // Set player aim direction
        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        // Trigger weapon aim event
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="weaponDirection"></param>
    /// <param name="weaponAngleDegrees"></param>
    /// <param name="playerAngleDegrees"></param>
    /// <param name="playerAimDirection"></param>
    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {
        // Fire when left mouse button is clicked
        if (Input.GetMouseButton(0))
        {
            // Trigger fire weapon event
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
            leftMouseDownPreviousFrame = true;
        }
        else
        {
            leftMouseDownPreviousFrame = false;
        }
    }

    /// <summary>
    /// Handle weapon
    /// </summary>
    private void SwitchWeaponInput()
    {
        // Only allow the player to switch weapons if the current active weapon is not being reloaded
        if(player.activeWeapon.GetCurrentWeapon().isWeaponReloading == false)
        { 
            // Switch weapon if mouse scroll wheel selected
            if (Input.mouseScrollDelta.y < 0f) // if scroll wheel is moved DOWN
            {
                PreviousWeapon();
            }

            if (Input.mouseScrollDelta.y > 0f) // if scroll wheel is moved UP
            {
                NextWeapon();
            }

            // Switch weapon based on key press
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetWeaponByIndex(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetWeaponByIndex(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetWeaponByIndex(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetWeaponByIndex(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetWeaponByIndex(5);
            }

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetWeaponByIndex(6);
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetWeaponByIndex(7);
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetWeaponByIndex(8);
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SetWeaponByIndex(9);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SetWeaponByIndex(10);
            }
        }

        // Set favorite weapon
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            SetCurrentWeaponToFirstInTheList();
        }

    }

    /// <summary>
    /// Set the current weapon to be first in the player weapon list
    /// </summary>
    private void SetCurrentWeaponToFirstInTheList()
    {
        // Create new temporary list of weapons
        List<Weapon> tempWeaponList = new List<Weapon>();
        // Add the current weapon to first in the temp list
        Weapon currentWeapon = player.weaponList[currentWeaponIndex - 1];
        currentWeapon.weaponListPosition = 1;
        tempWeaponList.Add(currentWeapon);

        // Loop thru existing weapon list and add weapons to temp weapon list - skipping current weapon
        // build up a new temp weapon list where the weapon the player wants to 'favorite' is added to the first of the list, all weapons follow after it
        int index = 2;

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon == currentWeapon) continue;

            tempWeaponList.Add(weapon);
            weapon.weaponListPosition = index;
            index++; 
        }

        // Assign new list
        player.weaponList = tempWeaponList;

        currentWeaponIndex = 1;

        // Set current weapon
        SetWeaponByIndex(currentWeaponIndex);

    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;
        // if player scrolls to past beginning of list set to end of list
        if (currentWeaponIndex < 1)
        {
            currentWeaponIndex = player.weaponList.Count;
        }
        SetWeaponByIndex(currentWeaponIndex);
    }

    private void NextWeapon()
    {
        currentWeaponIndex++;
        // if player scrolls to past end of list set to beginning of list
        if (currentWeaponIndex > player.weaponList.Count)
        {
            currentWeaponIndex = 1;
        }
        SetWeaponByIndex(currentWeaponIndex);
    }

    /// <summary>
    /// Pass in a weapon index, calls the SetActiveWeaponEvent based on weapon index that was passed in
    /// </summary>
    /// <param name="weaponIndex"></param>
    private void SetWeaponByIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = weaponIndex;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[weaponIndex - 1]);
        }
    }

    /// <summary>
    /// Tests for weapon reload input
    /// </summary>
    private void ReloadWeaponInput()
    {
        Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

        // if current weapon is reloading return
        if (currentWeapon.isWeaponReloading) 
            return;

        // remaining ammo is less than magazine capacity then return and not infinite ammo then return
        if (currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponMagazineAmmoCapacity && !currentWeapon.weaponDetails.hasInfiniteAmmo)
            return;

        // if ammo in the magazine equals magazine capacity (is full) return
        if (currentWeapon.weaponMagazineRemainingAmmo == currentWeapon.weaponDetails.weaponMagazineAmmoCapacity) 
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            // call the reload weapon event
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), 0);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if Collided with someting stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if in collision with something stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void StopPlayerRollRoutine()
    {
        if (playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);
            isPlayerRolling = false;
        }
    }

    /// <summary>
    /// Player movement input
    /// </summary>
    private void MovementInput()
    {
        // Get movement input
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);

        // Create a direction vector based on the input
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        // Adjust for 2 simultaenous keys being pressed (pythagoras approximation)
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        // If there is movement either move or roll
        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                // trigger movement event
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            // else roll if not cooling down (RMB was clicked)
            else if (playerRollCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }
        }
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    /// <summary>
    /// Player roll
    /// </summary>
    /// <param name="direction"></param>
    private void PlayerRoll(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    /// <summary>
    /// Player roll coroutine
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        // minDistance used to decide when to exit coroutine loop
        float minDistance = 0.2f;

        isPlayerRolling = true;

        // Calculate target position to roll to
        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;

        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed, direction, isPlayerRolling);
            // yield and wait for fixed update

            yield return waitForFixedUpdate;
        }
        isPlayerRolling = false;
        playerRollCooldownTimer = movementDetails.rollCooldownTime;

        player.transform.position = targetPosition;
    }

    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            // Reduce the cooldown timer until it's 0
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion Validation
}
