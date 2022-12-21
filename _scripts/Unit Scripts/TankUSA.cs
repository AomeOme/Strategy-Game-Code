using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankUSA : MonoBehaviour
{

    public float health = 100;
    public float attack = 20;
    public float defense = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            GameObject g = this.gameObject;
            g.SetActive(false);
        }
    }
}
