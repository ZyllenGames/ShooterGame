using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void onHit(float damage, Vector3 hitPos, Vector3 hitDir);

    void Die(Vector3 hitPos, Vector3 hitDir);
}
