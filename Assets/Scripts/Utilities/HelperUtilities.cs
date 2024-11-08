using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{
    public static Camera mainCamera;

    /// <summary>
    /// Get the mouse world position
    /// </summary>
    /// <param name=""></param>
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        
        Vector3 mouseScreenPosition = Input.mousePosition;
        
        //Clamp mouse position to screen size
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        worldPosition.z = 0f;

        return worldPosition;
    }

    /// <summary>
    /// Get the angle in degrees from a direction vector
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x); // remember your trigonometry :)
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    /// <summary>
    /// Get the direction vector from an angle in degrees.<br/>
    /// Helper method that gives the ability to calculate the firing direction vector based on the aiming angle.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }

    /// <summary>
    /// Get AimDirection enum value from the passed in angleDegrees
    /// </summary>
    /// <param name="angleDegrees"></param>
    /// <returns></returns>
    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDirection;

        // Set player diection
        // Up Right
        if (angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDirection = AimDirection.UpRight;
        }
        // Up 
        else if (angleDegrees > 67f && angleDegrees <= 112f)
        {
            aimDirection = AimDirection.Up;
        }
        // Up Left
        else if(angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDirection = AimDirection.UpLeft;
        }
        // Left
        else if((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180f && angleDegrees <= -135f))
        {
            aimDirection = AimDirection.Left;
        }
        // Down
        else if(angleDegrees > -135f && angleDegrees <= -45f)
        {
            aimDirection = AimDirection.Down;
        }
        // Right
        else if((angleDegrees > -45 && angleDegrees <= 0f) || (angleDegrees > 0f && angleDegrees < 22f))
        {
            aimDirection = AimDirection.Right;
        }
        else
        {
            aimDirection = AimDirection.Right;
        }
        return aimDirection;
    }

    /// <summary>
    /// Convert the linear volume scale to decibels. Pass in a linear value and it returns a logarithmic value.
    /// </summary>
    /// <param name="linear"></param>
    /// <returns></returns>
    public static float LinearToDecibles(int linear)
    {
        float linearScaleRange = 20f;

        // formula to convert from the linear scale to the logarithmic decibel scale
        return Mathf.Log10((float)linear / linearScaleRange) * 20f; // the 20 multipler is an adjustment factor when converting a linear value to a log value that relates to amplitude 
    }

    /// <summary>
    /// Empty string debug check
    /// </summary>
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log($"{fieldName} is empty and must contain a value in object {thisObject.name.ToString()}");
            return true;
        }
        return false;
    }

    /// <summary>
    /// Null value debug check
    /// </summary>
    /// <param name="thisObject"></param>
    /// <param name="fieldName"></param>
    /// <param name="objectToCheck"></param>
    /// <returns></returns>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log($"{fieldName} is null and must contain a value in object {thisObject.name.ToString()}");
            return true;
        }
        return false;
    }

    /// <summary>
    /// list empty or contains null value check - returns true if there is an error
    /// </summary>
    
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log($"{fieldName} is null in object {thisObject.name.ToString()}");
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                Debug.Log($"{fieldName} has null values in object {thisObject.name.ToString()}");
                error = true;
            }
            else
            {
                count++;
            }
        }
        if (count == 0)
        {
            Debug.Log($"{fieldName} has no values in object {thisObject.name.ToString()}");
            error = true;
        }
        return error;
    }

    /// <summary>
    /// Positive value debug check - if zero is allowed set isZeroAllowed to true. Returns true if there is an error.
    /// </summary>
    /// <param name="thisObject"></param>
    /// <param name="fieldName"></param>
    /// <param name="valueToCheck"></param>
    /// <param name="isZeroAllowed"></param>
    /// <returns></returns>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;
        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log($"{fieldName} must contain a positive value or zero in object {thisObject.name.ToString()}");
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log($"{fieldName} must contain a positive value in object {thisObject.name.ToString()}");
                error = true;
            }
        }
        return error;
    }

    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f);

        // Loop through room spawn positions
        foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            // Convert the spawn grid positions to world positons
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            // Check spawnPositionWorld against the nearest spawn position we're maintaining
            // if the world spawn position is nearer to the player then we'll set it as the nearest spawn position
            if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                nearestSpawnPosition = spawnPositionWorld;
            }
        }
        return nearestSpawnPosition;
    }

    /// <summary>
    /// Positive value debug check - if zero is allowed set isZeroAllowed to true. Returns true if there is an error;
    /// </summary>
    /// <param name="thisObject"></param>
    /// <param name="fieldName"></param>
    /// <param name="valueToCheck"></param>
    /// <param name="isZeroAllowed"></param>
    /// <returns></returns>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log($"{fieldName} must contain a positive value or zero in object {thisObject.name.ToString()}");
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log($"{fieldName} must contain a positive value in object {thisObject.name.ToString()}");
                error = true;
            }
        }
        return error;
    }

    /// <summary>
    /// Positive range debug check - set isZeroAllowed to true if the min and max range values can both be zero. Returns true if there is an error.
    /// </summary>
    /// <param name="thisObject"></param>
    /// <param name="fieldNameMinimum"></param>
    /// <param name="valueToCheckMinimum"></param>
    /// <param name="fieldNameMaximum"></param>
    /// <param name="valueToCheckMaximum"></param>
    /// <param name="isZeroAllowed"></param>
    /// <returns></returns>
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, float valueToCheckMinimum, string fieldNameMaximum, float valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log($"{fieldNameMinimum} must be less than or equal to {fieldNameMaximum} in object {thisObject.name.ToString()}");
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;
        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;
    }

}
