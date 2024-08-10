using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Data", menuName ="My Game /Player Data")]
public class PlayerData : ScriptableObject
{
    public int level;
    public float respwanTime;
    public int MaxHeath;
    public int damage;
    public int armor;
    public int magicResistance;
    public float attackSpeed;
}
