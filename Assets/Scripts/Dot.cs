using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    
    public int column;                       //adjusted inside unity   set to 7    
    public int row;                          //adjusted in untiy set to 10
    public int previousColumn;               //grabs the column in for swapping back if match not found
    public int previousRow;                  //grabs the row in for swapping back if match not found
    public int targetX;                      //position where the x value will go if dragged that direction
    public int targetY;                        //position where the y value will go if dragged that direction
    public int dotNum;                        //random num based on board size for use generating coin tiles
    private FindMatches findMatches;
    public bool isMatched = false;           //bool that checks if match exists when swapped
    private Board board;                     //createsa new board object but not instatited 
    public GameObject otherDot;             //creates an empty other dot object for swapping
    private Vector2 firstTouchPosition;      //vectors used for calcualting swipe positions
    private Vector2 finalTouchPosition;      //vectors used for calculating swipe positions
    private Vector2 tempPosition;            //used to hold the position of the dot for swapping on move

    public float swipeAngle = 0;             //needed to make sure different swipe directions can be calculted   
    public float swipeResist = 1f;           //used to stop mouse clicking as reading as a swipe action


    public bool isBreakable;
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;
    public GameObject adjacentBomb;

    void Awake()                             // Use this for initialization
    {

        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        dotNum = Random.Range(0, 10);
        if(dotNum == 5)
        {
            isBreakable = true;
            Debug.Log("Started");
        }

        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousColumn = column;
        //previousRow = row;
        //Debug.Log("Started");
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRowBomb = true;
            GameObject color = Instantiate(adjacentBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
        }
    }


    void Update()                            // Update is called once per frame
    {
        //FindMatches();
       /* if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }*/
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //moves towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if(board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();

        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            //board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
                                             //moves towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();


        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }

    }
    public IEnumerator CheckMoveCo()   
    {
        if(isColorBomb)
        {
            findMatches.MatchPiecesOfColor(otherDot.tag);
            isMatched = true;
        }
        else if (otherDot.GetComponent<Dot>().isColorBomb)
        {
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherDot.GetComponent<Dot>().isMatched = true;
        }

        yield return new WaitForSeconds(.4f);
        if (otherDot != null)
        {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                Debug.Log("HI1");
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null; 
                board.currentState = GameState.move;

            }
            else
            {
                Debug.Log("HI3");
                board.DestroyMatches();

            }
            Debug.Log("HI2");
            // otherDot = null;
        }




    }
    private void OnMouseDown()
    {
        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

    }
    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
        else
        {
            board.currentState = GameState.move;
        }
    }
    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();

            board.currentDot = this;
        }
        else
        {
            board.currentState = GameState.move;
        }


    }
    void MovePiecesActual(Vector2 direction)
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumn = column;
        if (otherDot != null)
        {
            otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(CheckMoveCo());
        }
        else
        {
            board.currentState = GameState.move;
        }
    }
    void MovePieces()//working
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {

            /*Right swipe
            otherDot = board.allDots[column + 1, row];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
            StartCoroutine(CheckMoveCo());*/
            MovePiecesActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {

            /*Up swipe
            otherDot = board.allDots[column, row + 1];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
            StartCoroutine(CheckMoveCo());*/
            MovePiecesActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            /*Left swipe
            otherDot = board.allDots[column - 1, row];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
            StartCoroutine(CheckMoveCo());*/
            MovePiecesActual(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            /*Down swipe
            otherDot = board.allDots[column, row - 1];
            previousColumn = column;
            previousRow = row;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
            StartCoroutine(CheckMoveCo());*/
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentState = GameState.move;
        }
    }
    void FindMatches()//Working
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    Debug.Log("in Find Matches.Dot");
                    isMatched = true;
                }
            }

        }
        if (row > 0 && row < board.height - 1)
        {
           
            GameObject upDot1 = board.allDots[column, row + 1]; //checks to see if there is an upward match
            GameObject downDot1 = board.allDots[column, row - 1];  //checks to see if there is a downward match
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    //if a match was found set ismatched to true and pass that to calling functions
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

    }
    public void MakeRowBomb()
    {
        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }
    public void MakeColumnBomb()
    {
        isColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }
    public void MakeColorBomb()
    {
        isColorBomb = true;
        GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
    
    }
    public void MakeAdjacentBomb()
    {
        isAdjacentBomb = true;
        GameObject marker = Instantiate(adjacentBomb, transform.position, Quaternion.identity);
        marker.transform.parent = this.transform;

    }
    


}