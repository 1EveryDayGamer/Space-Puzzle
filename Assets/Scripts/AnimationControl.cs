using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    // Start is called before the first frame update
    public bool setA = false;
    void Start()
    {
        var p = FindObjectOfType<PlayAnimation>();
        p.gameObject.SetActive(false);  
    }

    // Update is called once per frame
    void Update()
    {
        var x = GetComponent<Transform>().GetChild(0);
        if(setA )
        {
            x.gameObject.SetActive(true);
        }

    }
}
