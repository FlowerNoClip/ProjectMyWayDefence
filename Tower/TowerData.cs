using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class TowerData : ScriptableObject
{
    private string Towername = null;
    private int ranked = 0;
    private int attackDamage = 0;
    private float attackSpeed = 0f;
    private int cost = 0;

    public string Towername1 { get => Towername; set => Towername = value; }
    public int Ranked { get => ranked; set => ranked = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public int Cost { get => cost; set => cost = value; }
}
