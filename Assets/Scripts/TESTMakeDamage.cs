using System;
using UnityEngine;

public class TESTMakeDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float AmountDamage = 15f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     private void OnTriggerEnter(Collider other) {

        PlayerHealth.Instance.TakeDamage(AmountDamage);
        

    }
}
