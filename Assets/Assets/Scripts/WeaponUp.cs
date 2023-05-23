using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="MyWeapon",menuName ="Myweapon",order =1)]
public class WeaponUp : ScriptableObject 
{
    public string skill;
    public int skillLevel;
    public int skillDamage;
    public int skillNumber;
    public Sprite sprite;
}
