using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvencibilityItem : Item
{
    public void HitByPlayer   (float damage, CharacterBeatController player)
    {
        base.HitByPlayer(damage, player);

        //m_player.SetInvencibility(true);
    }

    public override void ExecuteAction ()
    {
        base.ExecuteAction();
    }

    public override void ExitAction    ()
    {
        base.ExitAction();
        //m_player.SetInvencibility(false);
        Destroy(this.gameObject);
    }
}
