using UnityEngine;

/// <summary>
/// This script attaches to ‘Background’ object, and would move it up if the object went down below the viewport border. 
/// This script is used for creating the effect of infinite movement. 
/// </summary>

public class BackgroundScroll : MonoBehaviour
{
    [Tooltip("Horizontal size of the sprite in the world space. Attach box collider2D to get the exact size")]
    public float horizontalSize;

    private void Update() {
        if (transform.position.x < -horizontalSize) //if sprite goes down below the viewport move the object up above the viewport
        {
            RepositionBackground();
        }
    }

    void RepositionBackground() {
        Vector2 groundOffSet = new Vector2(horizontalSize * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffSet;
    }
}
