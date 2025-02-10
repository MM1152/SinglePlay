using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : Boss
{

    protected override void Update()
    {

        if (!GameManager.Instance.playingAnimation)
        {
            base.Update();
        }

    }
}
