using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    Animator ship;


    // Start is called before the first frame update
    void Start()
    {

        ship = gameObject.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.SetActive(true);
        var a = FindObjectOfType<ShipAssembly>();
        if(a.assembled == 5 )
        {
           
            ship.SetTrigger("PlayShipAnim");
            a.assembled = 0;
        }
    }
}
