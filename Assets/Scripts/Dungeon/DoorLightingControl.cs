using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
// Fade in the doors as the player approaches them
public class DoorLightingControl : MonoBehaviour
{
    private bool isLit = false;
    private Door door;

    private void Awake()
    {
        // Get components
        door = GetComponentInParent<Door>(); // the actual door component is on the parent game object, the door lighting control component is on the child
    }

    /// <summary>
    /// Fade in door
    /// </summary>
    /// <param name="door"></param>
    public void FadeInDoor(Door door)
    {
        // Create a new material to fade in
        Material material = new Material(GameResources.Instance.variableLitShader);

        if (!isLit)
        {
            SpriteRenderer[] spriteRendererArray = GetComponentsInParent<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRendererArray)
            {
                StartCoroutine(FadeInDoorRoutine(spriteRenderer, material));
            }
            isLit = true;
        }
    }

    /// <summary>
    /// Fade in door coroutine
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="material"></param>
    /// <returns></returns>
    private IEnumerator FadeInDoorRoutine(SpriteRenderer spriteRenderer, Material material)
    {
        spriteRenderer.material = material; // need to fade in the alpha of this material

        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        spriteRenderer.material = GameResources.Instance.litMaterial;
    }

    // call fade in door routine when player approaches door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FadeInDoor(door);
    }
}
