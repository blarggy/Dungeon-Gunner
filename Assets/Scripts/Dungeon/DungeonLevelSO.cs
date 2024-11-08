using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header BASIC LEVEL DETAILS
    [Space(10)]
    [Header("BASIC LEVEL DETAILS")]
    #endregion BASIC LEVEL DETAILS

    #region Tooltip
    [Tooltip("The name for the level")]
    #endregion Tooltip

    public string levelName;

    #region Header ROOM TEMPLATES FOR LEVEL
    [Space(10)]
    [Header("ROOM TEMPLATES FOR LEVEL")]
    #endregion Header ROOM TEMPLATES FOR LEVEL

    #region Tooltip
    [Tooltip("Popualte the list with the room templates that you want to be part of the level. You need to ensure that room templates are included for all room node types that are specified in the Room Node Graphs for the level.")]
    #endregion Tooltip

    public List<RoomTemplateSO> roomTemplateList;

    #region Header ROOM NODE GRAPHS FOR LEVEL
    [Space(10)]
    [Header("ROOM NODE GRAPHS FOR LEVEL")]
    #endregion

    #region Tooltip
    [Tooltip("Populate this list with the room node graphs which should be randomly selected from for the level")]
    #endregion Tooltip

    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR

    // Validate scriptable objects details entered
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(levelName), levelName);
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return;

        // Check to make sure that room templates are specified for all the node types in the specified node graphs

        // First check that north/south corridor, east/west corridor and entrance types have been specified

        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;

        // Loop through all room templates to check that this node type has been specified
        foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;

            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isEWCorridor = true;

            if (roomTemplateSO.roomNodeType.isCorridorNS)
                isNSCorridor = true;

            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;
        }

        if (isEWCorridor == false)
        {
            Debug.Log($"In {this.name.ToString()} : No E/W corridor room type specified");
        }

        if (isNSCorridor == false)
        {
            Debug.Log($"In {this.name.ToString()} : No N/S corridor room type specified");
        }

        if (isEntrance == false)
        {
            Debug.Log($"In {this.name.ToString()} : No Entrance room type specified");
        }

        // Loop thru node graphs
        foreach (RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;

            // Loop through all nodes in node graph
            foreach (RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if (roomNodeSO == null)
                    continue;

                // Check that a room template has been specified for each roomNode type
                // Corridors and entrance already checked
                if (roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS ||
                roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isNone)
                    continue;

                bool isRoomNodeTypeFound = false;

                // Loop through all room templates to check that this node type has been specified
                foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if (roomTemplateSO == null)
                        continue;

                    if (roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                        
                }
                if (!isRoomNodeTypeFound)
                    Debug.Log($"In {this.name.ToString()} : No room template {roomNodeSO.roomNodeType.name.ToString()} found for node graph {roomNodeGraph.name.ToString()}");
            }
        }


    }
#endif
    #endregion Validation

}
