using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public int m_health;

    public void HitByPlayer   (float damage, CharacterBeatController player)
    {
        base.HitByPlayer(damage, player);

        m_player.AddHealth(m_health);
    }

    public override void ExecuteAction ()
    {
        base.ExecuteAction();
    }

    public override void ExitAction    ()
    {
        base.ExitAction();
        Destroy(this.gameObject);
    }
}
