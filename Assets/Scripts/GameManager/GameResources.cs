using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    /// <summary>
    /// This is a neat method of centralizing resources needed to share to make them easily accessible.
    /// Any resources I want to be accessible by multiple components we can add them to the GameResouces script.
    /// Once they're added to the script they appear in the editor and will be accessible to any other components or scripts that need to access them.
    /// Access them through the GameResources.Instance property, because it's static you won't have to specify anything else.
    /// </summary>
 
    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                // anything placed within a folder called "Resources" in Assets can be accessed by Resources.Load
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    #region Header DUNGEGON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region ToolTip
    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
    #endregion

    public RoomNodeTypeListSO roomNodeTypeList;

    #region Header PLAYER
    [Space(10)]
    [Header("PLAYER")]
    #endregion

    #region ToolTip
    [Tooltip("The current player scriptable object - this is used to reference the current player between scenes")]
    #endregion
    public CurrentPlayerSO currentPlayer;

    #region Header SOUNDS
    [Space(10)]
    [Header("SOUNDS")]
    #endregion Header
    #region Tooltip
    [Tooltip("Populate with the sounds master mixer group")]
    #endregion
    public AudioMixerGroup soundsMasterMixerGroup;

    #region Tooltip
    [Tooltip("Door open close sound effect")]
    #endregion
    public SoundEffectSO doorOpenCloseSoundEffect;


    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    #region Tooltip
    [Tooltip("Dimmed Material")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("Sprite-Lit-Default Material")]
    public Material litMaterial;
    #endregion

    #region Tooltip
    [Tooltip("Populate with the Variable Lit Shader")]
    public Shader variableLitShader;
    #endregion

    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with ammo icon prefab")]
    #endregion
    public GameObject ammoIconPrefab;

#if UNITY_EDITOR
    // validate the scriptable object details entered
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(roomNodeTypeList), roomNodeTypeList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(currentPlayer), currentPlayer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundsMasterMixerGroup), soundsMasterMixerGroup);
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorOpenCloseSoundEffect), doorOpenCloseSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(litMaterial), litMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(dimmedMaterial), dimmedMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(variableLitShader), variableLitShader);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoIconPrefab), ammoIconPrefab);
    }
#endif
}
