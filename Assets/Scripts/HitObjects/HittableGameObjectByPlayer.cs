using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittableGameObjectByPlayer 
{
    void HitByPlayer(float damage, CharacterBeatController player);
}
