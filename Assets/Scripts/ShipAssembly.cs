using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipAssembly : MonoBehaviour
{
    //a list of pieces used to detect which state each button is in
    public int piece1;
    public int piece2;
    public int piece3;
    public int piece4;
    public int piece5;
    public int assembled;
    public bool reassemble;
    
    public GameObject[] pieces;

    // Start is called before the first frame update
    void Start()
    {
        //sets all buttons to inactive until the button and coins are sufficient
        pieces[0].SetActive(!enabled);
        pieces[1].SetActive(!enabled);
        pieces[2].SetActive(!enabled);
        pieces[3].SetActive(!enabled);
        pieces[4].SetActive(!enabled);
        pieces[5].SetActive(!enabled);

    }

    // Update is called once per frame
    void Update()
    {
        if (reassemble)
        {
            pieces[0].SetActive(!enabled);
            pieces[1].SetActive(!enabled);
            pieces[2].SetActive(!enabled);
            pieces[3].SetActive(!enabled);
            pieces[4].SetActive(!enabled);
            pieces[5].SetActive(!enabled);

        }
        if (piece1 == 1)  //checks to see if the button to be enabled matches the button in slot1
        {
            pieces[0].SetActive(enabled); //turns on the gameobject
            piece1 = 2; // sets it to state 2 for possible use later
            assembled += 1; 
        }
        else if(piece2 == 1)
        {
            if (piece5 == 2)  // this is used to check to see if the astronaut was found ahead of the cockpit
            {
                pieces[1].SetActive(enabled); // enables the piece in slot 2 
                pieces[5].SetActive(!enabled); // disables the desk sprite 
                pieces[4].SetActive(enabled);   //enables the overlapped sprite with the astronaut inside the cockpict
            }
            else
            {
                pieces[1].SetActive(enabled); //if the astronaut hasnt been found simply displays the empty cockpit
            }
            piece2 = 2;
            assembled += 1;
        }
        else if(piece3 == 1)
        {
            pieces[2].SetActive(enabled);
            piece3 = 2;
            assembled += 1;
        }
        else if(piece4 == 1)
        {
            pieces[3].SetActive(enabled);
            piece4 = 2;
            assembled += 1;
        }
        else if(piece5 == 1) 
        {
            if (piece2 == 2) // first checks to see if cockpit is found
            {
                pieces[4].SetActive(enabled);  //if it has then it put the pilot inside

            }
            else
            {
                pieces[5].SetActive(enabled); // if not puts the desk pilot image up instead
            }

            piece5 = 2;
            assembled += 1;
        }
        if (assembled == 5)
        {
            var g = FindObjectOfType<GameText>();
            var a = FindObjectOfType<AnimationControl>();
            g.GetComponent<Text>().text = "You did it You Saved Him";
            a.setA = true;

        }

    } 
}
