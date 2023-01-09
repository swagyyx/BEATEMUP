using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittableGameObjectByEnemy 
{
    void HitByEnemy(float damage, CharacterBeatController player);
}
