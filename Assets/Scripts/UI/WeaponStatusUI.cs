using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class WeaponStatusUI : MonoBehaviour
{
    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    #endregion header OBJECT REFERENCES
    #region Tooltip
    [Tooltip("Populate with image component on the child WeaponImage gameobject")]
    #endregion Tooltip
    [SerializeField] private Image weaponImage;
    #region Tooltip
    [Tooltip("Populate with the Transform from the child AmmoHolder gameobject")]
    #endregion Tooltip
    [SerializeField] private Transform ammoHolderTransform;
    #region Tooltip
    [Tooltip("Populate with the TextMeshPro-Text component on the child ReloadText gameobject")]
    #endregion Tooltip
    [SerializeField] private TextMeshProUGUI reloadText;
    #region Tooltip
    [Tooltip("Populate with the TextMeshPro-Text component on the child AmmoRemainingText gameobject")]
    #endregion Tooltip
    [SerializeField] private TextMeshProUGUI ammoRemainingText;
    #region Tooltip
    [Tooltip("Populate with the TextMeshPro-Text component on the child WeaponNameText gameobject")]
    #endregion Tooltip
    [SerializeField] private TextMeshProUGUI weaponNameText;
    #region Tooltip
    [Tooltip("Populate with the RectTransform of the child gameobject ReloadBar")]
    #endregion Tooltip
    [SerializeField] private Transform reloadBar;
    #region Tooltip
    [Tooltip("Populate with the Image component of the child gameobject BarImage")]
    #endregion Tooltip
    [SerializeField] private Image barImage;

    private Player player;
    private List<GameObject> ammoIconList = new List<GameObject>();
    private Coroutine reloadWeaponCoroutine;
    private Coroutine blinkingReloadTextCoroutine;

    private void Awake()
    {
        // Get player
        player = GameManager.Instance.GetPlayer();
    }

    private void OnEnable()
    {
        // Subscribe to the set active weapon event
        player.setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;

        // Subscribe to the weapon fired event
        player.weaponFiredEvent.OnWeaponFired += WeaponFiredEvent_OnWeaponFired;

        // Subscribe to the reload weapon event
        player.reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnWeaponReload;

        // Subscribe to weapon reloaded event
        player.weaponReloadedEvent.OnWeaponReloaded += WeaponReloadedEvent_OnWeaponReloaded;
    }

    private void OnDisable()
    {
        // UnSubscribe to the set active weapon event
        player.setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;

        // UnSubscribe to the weapon fired event
        player.weaponFiredEvent.OnWeaponFired -= WeaponFiredEvent_OnWeaponFired;

        // UnSubscribe to the reload weapon event
        player.reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnWeaponReload;

        // UnSubscribe to weapon reloaded event
        player.weaponReloadedEvent.OnWeaponReloaded -= WeaponReloadedEvent_OnWeaponReloaded;
    }

    private void Start()
    {
        // Update active weapon status in the UI
        SetActiveWeapon(player.activeWeapon.GetCurrentWeapon());
    }

    /// <summary>
    /// Handle set active weapon event in the UI
    /// </summary>
    /// <param name="setActiveWeaponEvent"></param>
    /// <param name="setActiveWeaponEventArgs"></param>
    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {
        // Event handlers often simply call another method that actually handle the event. They receive the event and call another method generally.
        SetActiveWeapon(setActiveWeaponEventArgs.weapon);
    }

    /// <summary>
    /// Handle weapon fired event on the UI
    /// </summary>
    /// <param name="weaponFiredEvent"></param>
    /// <param name="weaponFiredEventArgs"></param>
    private void WeaponFiredEvent_OnWeaponFired(WeaponFiredEvent weaponFiredEvent, WeaponFiredEventArgs weaponFiredEventArgs)
    {
        WeaponFired(weaponFiredEventArgs.weapon);
    }

    /// <summary>
    /// Weapon fired update UI
    /// </summary>
    /// <param name="weapon"></param>
    private void WeaponFired(Weapon weapon)
    {
        UpdateAmmoText(weapon);
        UpdateAmmoLoadedIcons(weapon);
        UpdateReloadText(weapon);
    }

    /// <summary>
    /// Handle weapon reload event on the UI
    /// </summary>
    /// <param name="reloadWeaponEvent"></param>
    /// <param name="reloadWeaponEventArgs"></param>
    private void ReloadWeaponEvent_OnWeaponReload(ReloadWeaponEvent reloadWeaponEvent, ReloadWeaponEventArgs reloadWeaponEventArgs)
    {
        UpdateWeaponReloadBar(reloadWeaponEventArgs.weapon);
    }

    /// <summary>
    /// Handle weapon reloaded event on the UI
    /// </summary>
    /// <param name="weaponReloadedEvent"></param>
    /// <param name="weaponReloadedEventArgs"></param>
    private void WeaponReloadedEvent_OnWeaponReloaded(WeaponReloadedEvent weaponReloadedEvent, WeaponReloadedEventArgs weaponReloadedEventArgs)
    {
        WeaponReloaded(weaponReloadedEventArgs.weapon);
    }

    /// <summary>
    ///  Weapon has been reloaded - update UI if current weapon
    /// </summary>
    /// <param name="weapon"></param>
    private void WeaponReloaded(Weapon weapon)
    {
        // if weapon reloaded is the current weapon
        if (player.activeWeapon.GetCurrentWeapon() == weapon)
        {
            UpdateReloadText(weapon);
            UpdateAmmoText(weapon);
            UpdateAmmoLoadedIcons(weapon);
            ResetWeaponReloadBar();
        }
    }

    /// <summary>
    /// Set the active weapon in the UI
    /// </summary>
    /// <param name="weapon"></param>
    private void SetActiveWeapon(Weapon weapon)
    {
        UpdateActiveWeaponImage(weapon.weaponDetails);
        UpdateActiveWeaponName(weapon);
        UpdateAmmoText(weapon);
        UpdateAmmoLoadedIcons(weapon);

        // If set weapon is still reloading then update reload bar
        if (weapon.isWeaponReloading)
        {
            UpdateWeaponReloadBar(weapon);
        }
        else
        {
            ResetWeaponReloadBar();
        }

        UpdateReloadText(weapon);
    }

    /// <summary>
    /// Populate active weapon image
    /// </summary>
    /// <param name="weaponDetails"></param>
    private void UpdateActiveWeaponImage(WeaponDetailsSO weaponDetails)
    {
        weaponImage.sprite = weaponDetails.weaponSprite;
    }

    /// <summary>
    /// Populate active weapon name
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateActiveWeaponName(Weapon weapon)
    {
        weaponNameText.text = ($"({weapon.weaponListPosition}) {weapon.weaponDetails.weaponName.ToUpper()}");
    }

    /// <summary>
    /// Update the ammo remaining text in the UI
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateAmmoText(Weapon weapon)
    {
        if (weapon.weaponDetails.hasInfiniteAmmo)
        {
            ammoRemainingText.text = "INFINITE AMMO";
        }
        else
        {
            ammoRemainingText.text = ($"{weapon.weaponRemainingAmmo.ToString()} / {weapon.weaponDetails.weaponAmmoCapacity.ToString()}");
        }
    }

    /// <summary>
    /// Update ammo magazine icons in the UI
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateAmmoLoadedIcons(Weapon weapon)
    {
        ClearAmmoLoadedIcons();

        for (int i = 0; i < weapon.weaponMagazineRemainingAmmo; i++)
        {
            // Instantiate ammo icon prefab
            GameObject ammoIcon = Instantiate(GameResources.Instance.ammoIconPrefab, ammoHolderTransform);
            ammoIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Settings.uiAmmoIconSpacing * i); // this setting aligns the ammo icons in a column 
            ammoIconList.Add(ammoIcon);
        }
    }

    /// <summary>
    /// Clear ammo icons
    /// </summary>
    private void ClearAmmoLoadedIcons()
    {
        // Loop through icon gameobjects and destroy them
        foreach (GameObject ammoIcon in ammoIconList)
        {
            Destroy(ammoIcon);
        }
        ammoIconList.Clear();
    }

    /// <summary>
    /// Reload weapon - update the reload bar on the UI
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateWeaponReloadBar(Weapon weapon)
    {
        if (weapon.weaponDetails.hasInfiniteMagazineCapacity)
            return;
        
        // for weapons that don't have infinite magazine capacity
        StopReloadWeaponCoroutine();
        UpdateReloadText(weapon);

        reloadWeaponCoroutine = StartCoroutine(UpdateWeaponReloadBarRoutine(weapon));
    }

    /// <summary>
    /// Animate reload weapon bar coroutine
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    private IEnumerator UpdateWeaponReloadBarRoutine(Weapon currentWeapon)
    {
        // set the reload bar to red
        barImage.color = Color.red;

        // Animate the weapon reload bar
        while (currentWeapon.isWeaponReloading)
        {
            // update reloadbar
            float barFill = currentWeapon.weaponReloadTimer / currentWeapon.weaponDetails.weaponReloadTime;

            // update bar fill
            reloadBar.transform.localScale = new Vector3(barFill, 1f, 1f);

            yield return null;
        }
    }

    /// <summary>
    /// Initialise the weapon reload bar on the UI
    /// </summary>
    private void ResetWeaponReloadBar()
    {
        StopReloadWeaponCoroutine();

        // set bar color to green
        barImage.color = Color.green;

        // set bar scale to 1
        reloadBar.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    /// <summary>
    /// Stop coroutine updating weapon reload progress bar
    /// </summary>
    private void StopReloadWeaponCoroutine()
    {
        // Stop any active weapon reload bar on the UI
        if (reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }
    }

    /// <summary>
    /// Update the blinking weapon reload text
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateReloadText(Weapon weapon)
    {
        if ((!weapon.weaponDetails.hasInfiniteMagazineCapacity) && (weapon.weaponMagazineRemainingAmmo <= 0 || weapon.isWeaponReloading))
        {
            // set the reload bar to red
            barImage.color = Color.red;
            StopBlinkingReloadTextCoroutine();
            blinkingReloadTextCoroutine = StartCoroutine(StartBlinkingReloadTextCoroutine());
        }
        else
        {
            StopBlinkingReloadText();
        }
    }

    /// <summary>
    /// Start the coroutine to blink the reload weapon text
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartBlinkingReloadTextCoroutine()
    {
        while (true)
        {
            reloadText.text = "RELOAD";
            yield return new WaitForSeconds(0.3f);
            reloadText.text = "";
            yield return new WaitForSeconds(0.3f);
        }
    }

    /// <summary>
    /// Stop the blinking reload text
    /// </summary>
    private void StopBlinkingReloadText()
    {
        StopBlinkingReloadTextCoroutine();
        reloadText.text = "";
    }

    /// <summary>
    /// Stop the blinking reload text coroutine
    /// </summary>
    private void StopBlinkingReloadTextCoroutine()
    {
        if (blinkingReloadTextCoroutine != null)
        {
            StopCoroutine(blinkingReloadTextCoroutine);
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponImage), weaponImage);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoHolderTransform), ammoHolderTransform);
        HelperUtilities.ValidateCheckNullValue(this, nameof(reloadText), reloadText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoRemainingText), ammoRemainingText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponNameText), weaponNameText);
        HelperUtilities.ValidateCheckNullValue(this, nameof(reloadBar), reloadBar);
        HelperUtilities.ValidateCheckNullValue(this, nameof(barImage), barImage);
    }
#endif
    #endregion Validation
}
