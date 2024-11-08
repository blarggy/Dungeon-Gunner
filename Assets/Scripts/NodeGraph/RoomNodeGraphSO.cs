using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }

    /// <summary>
    /// Load the room node dictionary from the room node list.
    /// </summary>
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();

        // Populate dictionary
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node; // node.id is the key, node is the value in the dictionary
        }
    }

    /// <summary>
    /// Retreive room node type 
    /// </summary>
    /// <param name="roomNodeType"></param>
    /// <returns></returns>
    public RoomNodeSO GetRoomNode(RoomNodeTypeSO roomNodeType)
    {
        foreach (RoomNodeSO node in roomNodeList)
        {
            if (node.roomNodeType == roomNodeType) // is current room node type the passed in room node type? pass it if so
            {
                return node;
            }
        }
        return null;
    }

    /// <summary>
    /// Retreive room node by room nodeID
    /// </summary>
    /// <param name="roomNodeID"></param>
    /// <returns></returns>
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        if (roomNodeDictionary.TryGetValue(roomNodeID, out RoomNodeSO roomNode)) // if function finds a value relating to the key it will store it in the 'roomNode' value
        {
            return roomNode;
        }
        return null;
    }

    /// <summary>
    /// Get child room nodes for supplied parent room node
    /// </summary>
    /// <param name="parentRoomNode"></param>
    /// <returns></returns>
    public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO parentRoomNode)
    {
        foreach (string childNodeID in parentRoomNode.childRoomNodeIDList)
        // loop thru each child room nodeIDs passed in by parentRoomNode
        // rather than get all child room IDs and returning it as a list
        // iterate thru the list and return each one of those nodes for the ID using yield return statement
        // has return type IEnumerable<RoomNodeSO>, method iterates thru Enumerable list and returns each item in list individually
        // the method we setup that calls this iterates thru a call to this method to get the childNodeID rather than iterating thru a list  :) 
        {
            yield return GetRoomNode(childNodeID);
        }
    }

    #region Editor Code
    // The following code should only run in the Unity Editor
#if UNITY_EDITOR
    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePosition;

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }

    /// <summary>
    /// Repopulate node dictionary every time a change is made in the editor.
    /// </summary>
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }
#endif
    #endregion Editor Code
}
