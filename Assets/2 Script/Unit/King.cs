using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class King : Boss 
{
    
    protected override void Attack()
    {
        base.Attack();
        if (canAttack && gameObject.CompareTag("Boss"))
        {
            ShowWarningArea();
        }
    }

    void ShowWarningArea()
    {
        GameObject WarningArea = PoolingManager.Instance.ShowObject("WarningArea(Clone)");
        InWarningArea inWarningArea = WarningArea.GetComponent<InWarningArea>();
        inWarningArea.Setting(1f, 2f, this);
        inWarningArea.SetPosition(target.transform.position + Vector3.down * 0.6f);
    }

    public override bool WaitSpawn()
    {
        transform.position += Vector3.left * Time.deltaTime * speed;
        return base.WaitSpawn();
    }
}
