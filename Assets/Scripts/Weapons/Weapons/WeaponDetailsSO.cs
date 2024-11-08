using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetals_", menuName = "Scriptable Objects/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject
{
    #region Header WEAPON BASE DETAILS
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]
    #endregion WEAPON BASE DETAILS
    #region Tooltip
    [Tooltip("Weapon name")]
    #endregion
    public string weaponName;
    #region Tooltip
    [Tooltip("The sprite for the weapon - the sprite should have the 'generate physics shape' option selected")]
    #endregion
    public Sprite weaponSprite;

    #region Header WEAPON CONFIGURATION
    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    #endregion Header WEAPON CONFIGURATION
    #region Tooltip
    [Tooltip("Weapon shoot position - the offset position for the end of the weapon from the sprite pivot point")]
    #endregion
    public Vector3 weaponShootPosition;

    #region Tooltip
    [Tooltip("Weapon current ammo")]
    #endregion
    public AmmoDetailsSO weaponCurrentAmmo;

    #region Tooltip
    [Tooltip("Firing sound effect SO for the weapon")]
    #endregion
    public SoundEffectSO weaponFiringSoundEffect;

    #region Tooltip
    [Tooltip("Reload effect SO for the weapon")]
    #endregion
    public SoundEffectSO weaponReloadingSoundEffect;

    #region Header WEAPON OPERATING VALUES
    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]
    #endregion Header WEAPON OPERATING VALUES
    #region Tooltip
    [Tooltip("Select if the weapon has infinite ammo")]
    #endregion
    public bool hasInfiniteAmmo = false;
    #region Tooltip
    [Tooltip("Select if the weapon has infinite magazine capacity")]
    #endregion
    public bool hasInfiniteMagazineCapacity = false;
    #region Tooltip
    [Tooltip("The weapon capacity - shots before a reload")]
    #endregion
    public int weaponMagazineAmmoCapacity = 6;
    #region Tooltip
    [Tooltip("Weapon ammo capacity - the maximum number of rounds that can be held for this weapon")]
    #endregion
    public int weaponAmmoCapacity = 100;
    #region Tooltip
    [Tooltip("Weapon fire rate - 0.2 means 5 shots per second")]
    #endregion
    public float weaponFireRate = 0.2f;
    #region Tooltip
    [Tooltip("Weapon precharge time - time in seconds to hold fire button down before firing")]
    #endregion
    public float weaponPrechargeTime = 0f;
    #region Tooltip
    [Tooltip("This is the weapon reload time in seconds")]
    #endregion
    public float weaponReloadTime = 0f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }

        if (!hasInfiniteMagazineCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponMagazineAmmoCapacity), weaponMagazineAmmoCapacity, false);
        }
    }
#endif
    #endregion
}