using System;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    public int level = 1;
    public float health;
    public int maxCarry;
    public int carryingNow = 0;
    public Dictionary<String, int> _inventory = new Dictionary<String, int>(){{"Wood", 0}, {"Stone", 0}};
    public GameObject PrimaryRole;
    public Laborer SecondaryRole;

    public void TakeDamage(int amount)
    {
        health -= amount;
    }

    private void Awake()
    {
        health = 100;
        maxCarry = 20;
    }
}