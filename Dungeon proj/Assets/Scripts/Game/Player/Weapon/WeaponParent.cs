using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//weapon rotation script
public class WeaponParent : MonoBehaviour
{
    public Vector2 PointerPosition  { get; set; }

    private void FixedUpdate()
    {
        WeaponFollowCursor();
    }

    private void WeaponFollowCursor()
    {
        // Get the mouse position in world space
        Vector3 pointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pointerPosition.z = transform.position.z; // Make sure to keep the z-coordinate unchanged

        // Calculate direction vector
        Vector2 direction = (pointerPosition - transform.position).normalized;

        // Rotate the weapon parent towards the mouse position
        transform.right = direction;

        // Adjust the weapon's scale based on the direction
        Vector3 scale = transform.localScale;
        
        if (direction.x < 0)
        {
            scale.y = -1;
        } else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
    }
}
