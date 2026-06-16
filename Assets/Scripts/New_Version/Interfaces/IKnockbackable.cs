using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(
        Transform knockbackEntityTransform,
        float knockbackForce,
        float knockbackTime,
        float stunTime
    );
}