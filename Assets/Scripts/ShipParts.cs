using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParts : MonoBehaviour
{
    public Sprite[] shipArray;
    public Sprite[] shipParts;

    // Start is called before the first frame update
    void Start()
    {
        List<int> chosen = new List<int>()
            {
                0,1,2,3,4,5 //sets up the list with these values
            };
        
        for (int x = 0; x < shipArray.Length; x ++)//uses the array length to fill the parts with random pieces
        {


            var tryAgain = true;
            
            while (tryAgain == true)//cycles through all options 
            {
                var i = Random.Range(0 , 6);

                if (chosen.Contains(i))
                {
                    shipParts[x] = shipArray[i];  //sets the part in that position to the corresponding piece from the jumbled array
                    chosen.Remove(i);   //removes the int from the array to avoid reusing variables that are already used up succesfully
                    tryAgain = false;
                }

            }

           
        }
    }


}
