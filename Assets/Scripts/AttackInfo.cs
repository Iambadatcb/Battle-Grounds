using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackInfo
{
    public string name;
    public float delay;
    public GameObject vfx;
    public Transform target;
    public AudioClip clip;
}
