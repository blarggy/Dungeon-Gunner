using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// A static class to handle game-wide events
// static class don't have to be instantiated so can be called directly using the class name
public static class StaticEventHandler
{
    // Room changed event
    public static event Action<RoomChangedEventArgs> OnRoomChanged;
    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs() { room = room });
    }
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room; // what room has the player entered
}