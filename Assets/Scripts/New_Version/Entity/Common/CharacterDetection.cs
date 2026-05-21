using NUnit.Framework;
using UnityEngine;

public class CharacterDetection : MonoBehaviour
{
    public Transform detectionPoint;
    public LayerMask characterLayer;

    private float DetectionRange = 5.5f;

    public Collider2D[] DetectCharacter()
    {
        CircleCollider2D collider = detectionPoint.GetComponent<CircleCollider2D>();
        DetectionRange = collider.radius;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            detectionPoint.position,
            DetectionRange,
            characterLayer
        );

        return hits;
    }
}
