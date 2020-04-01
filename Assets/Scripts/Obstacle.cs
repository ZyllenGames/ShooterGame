using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : LivingEntity
{
    public override void Die(Vector3 hitPos, Vector3 hitDir)
    {
        base.Die(hitPos, hitDir);
        
    }

}
