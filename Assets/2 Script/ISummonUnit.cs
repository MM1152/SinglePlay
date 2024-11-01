using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISummonUnit : IFollowTarget
{
    public static int unitCount = 0;
    public Summoner summoner{get; set;}
}
