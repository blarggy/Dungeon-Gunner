using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;

    #region Tooltip
    [Tooltip("Populate with the CursorTarget gameobject")]
    #endregion
    [SerializeField] private Transform cursorTarget;

    private void Awake()
    {
        // Load components
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        SetCinemachineTargetGroup();
    }

    /// <summary>
    /// Set Cinemachine camera target group
    /// </summary>
    private void SetCinemachineTargetGroup()
    {
        // Create target group for cinemachine for the cinemachine camera to follow the player
        // when cinemachine creates target group it forms a bounding box, the radius indicates the size of the bounding box
        CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target { weight = 1f, radius = 2.5f, target = GameManager.Instance.GetPlayer().transform };

        // Create target group for cinemachine for the cinemachine camera to follow the cursor
        CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target { weight = 1f, radius = 1f, target = cursorTarget };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachineGroupTarget_player, cinemachineGroupTarget_cursor };
        cinemachineTargetGroup.m_Targets = cinemachineTargetArray; // cinemachineTargetGroup.m_Targets is the same value as the target array seen in the Inspector
    }

    private void Update()
    {
        // as the mouse moves on the screen the position of cursorTarget gameobject changes
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition();  
    }
}
