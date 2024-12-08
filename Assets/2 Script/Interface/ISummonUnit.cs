using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISummonUnit : IFollowTarget
{
    public Summoner summoner{get; set;}
}
