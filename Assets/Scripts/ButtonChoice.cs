using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChoice : MonoBehaviour
{



    public int cost;
    public bool bought = false;

    public void CheckSprite(string name)
    {
        var parts = FindObjectOfType<ShipAssembly>();  // sets a variable to the ship assembly script so we can locate the parts at will
        var g = FindObjectOfType<GameText>();   //grants acces to the game info output 

        if (name == "bubble 4")     //checks each possible input from the call to see which button is to be turned on
        {
            parts.piece1 = 1;       //the matching piece in the parts array will be enabled in the corresponding ship image 
        }
        else if (name == "bubble 6")
        {
            parts.piece2 = 1;
        }
        else if (name == "bubble 5")
        {
            parts.piece3 = 1;
        }
        else if (name == "bubble 3")
        {
            parts.piece4 = 1;
        }
        else if (name == "bubble 2")
        {
            parts.piece5 = 1;
        }
        else if(name == "enemy 1")
        {
            g.GetComponent<Text>().text =  "Uh oh hes speeding up";
        }

    }
    public void SetButtonSprite()
    {
        var sprites = FindObjectOfType<ShipParts>().shipParts;

        
        if (this.tag == "piece 1")
        {
            var s = this.GetComponent<Image>(); // grabs the Image component of the calling button
            s.sprite = sprites[0];      // sets the game objects sprite to the sprite from the ship array
            CheckSprite(s.sprite.name);     //calls the function that will tell the Ship sprite which image to enable

        }
        else if (this.tag == "piece 2")
        {
            var s = this.GetComponent<Image>();
            s.sprite = sprites[1];
            CheckSprite(s.sprite.name);
        }
        else if (this.tag == "piece 3")
        {
            var s = this.GetComponent<Image>();
            s.sprite = sprites[2];
            CheckSprite(s.sprite.name);
        }
        else if (this.tag == "piece 4")
        {
            var s = this.GetComponent<Image>();
            s.sprite = sprites[3];
            CheckSprite(s.sprite.name);
        }
        else if (this.tag == "piece 5")
        {
            var s = this.GetComponent<Image>();
            s.sprite = sprites[4];
            CheckSprite(s.sprite.name);
        }
        else if (this.tag == "piece 6")
        {
            var s = this.GetComponent<Image>();
            s.sprite = sprites[5];
            CheckSprite(s.sprite.name);

        }
        
    }
    public void Reveal()
    {
        var g = FindObjectOfType<GameText>();
        var c = FindObjectOfType<Board>();
        if (c.coinCount >= cost && bought == false) //check to see if theres enough coins and also if a piece has been bought already
        {
            SetButtonSprite();          //if not succesfully show the sprite hidden behind the button and deduct the cost from coins
            bought = true;
            c.coinCount = c.coinCount - cost;
        }
        else if(bought == true)
        {
            g.GetComponent<Text>().text = ("Already bought this part");  // if bought is true display message to game info
        }
        else
        {
            g.GetComponent<Text>().text = ("You only have " + c.coinCount + " coins " + "You need " + cost);    //tell the player they need more money
        }

    }
    // Start is called before the first frame update
    void Start()
    {
       
        cost = Random.Range(1, 3); //sets the cost of all the buttons with this script attached randomly at the beggining of the game 

    }


}
