using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Main Elements")]
    [SerializeField] HealthComponent healthComponent;



    private void Start()
    {
        HealthComponent healthComponentInstance = new HealthComponent(3);
        healthComponent = healthComponentInstance;
    }



}
