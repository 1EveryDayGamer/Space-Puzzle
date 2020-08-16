using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState// sets the game in 1 of 2 states
{
    wait,
    move
}
public enum TileKind// wasnt implemented yet but sets the tiles to a specific type 
{
    Breakable,// would require hit points to be depleted before destruction
    Blank,// allows for the board to be set to different shapes 
    Normal
}
[System.Serializable]
public class TileType// works with TileKind to set which tile will be changed
{
    public int x;
    public int y;
    public bool filled;
    public TileKind tileKind;
    

}


public class Board : MonoBehaviour
{


    public GameState currentState = GameState.move;// allows the board to be interacted with
    public int width;
    public int height;
    public int offSet;
    public int score;
    public int coinCount;
    public GameObject tilePrefab;// used to create different tiles
    public GameObject BreakableTilePrefab;
    public GameObject BlankTilePrefab;
    public GameObject[] dots;//holds dots passed in as needed
    public GameObject DestroyEffect;//a cosmetic effect to make destruction more pleasing to the eye
    public List<TileType> boardLayout;
    private bool[,] blankSpaces;
    public GameObject[,] allDots;//the main array that stores every dot of the current board
    public Dot currentDot;
    public List<int> usedNums = new List<int>();
    private BackgroundTile[,] breakableTiles;
    private BackgroundTile[,] blankSpaceTiles;
    private FindMatches findMatches;// reference to a function that makes the matches
    public Sprite[] choices;
    public Text textComponent;
   

    // Use this for initialization
    void Start()
    {
        breakableTiles = new BackgroundTile[width, height];
        blankSpaceTiles = new BackgroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool [width, height];
        allDots = new GameObject[width, height];
        coinCount = 0;
        SetUp();
    }
    public void Update()
    {
        textComponent.text = ("You have \n" + coinCount + " \nCoins");
    }
    public void GenerateBlankSpaces()// wasnt implemeted yet by me because i have a different use for it
    {
        for (int i = 0; i < boardLayout.Count; i++)
        {
            if(boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(BlankTilePrefab , tempPosition, Quaternion.identity);

                blankSpaceTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }
    public void GenerateBreakableTiles()
    {
        for (int i = 0; i < boardLayout.Count; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Breakable && boardLayout[i].filled == false)
            {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(BreakableTilePrefab, tempPosition, Quaternion.identity);

                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
                boardLayout[i].filled = true;
            }

        }
    }
    


    private void SetUp()
    {

        textComponent.text = (" " + coinCount);//creates the string to display the coins collected
        GenerateBlankSpaces();
        //GenerateBreakableTiles();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "( " + i + ", " + j + " )";

                    int dotToUse = Random.Range(0, dots.Length);

                    int maxIterations = 0;

                    while (MatchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    maxIterations = 0;

                    
                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;
                    dot.transform.parent = this.transform;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                    
                    Debug.Log(dot.GetComponent<Dot>().dotNum);

                    if (dot.GetComponent<Dot>().isBreakable)
                    {
                        TileType temp = new TileType();
                        temp.x = i;
                        temp.y = j;
                        temp.tileKind = TileKind.Breakable;
                        boardLayout.Add(temp);
                        Debug.Log("Temp " + temp.x + " , " + temp.y);
                    }
                }
            }

        }
        GenerateBreakableTiles();
    }


    private bool MatchesAt(int column, int row, GameObject piece)// checks where the actual matches are 
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }

        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    private bool ColumnOrRow()//helper function to make finding bomb matches easier
    {
        int numberHorizontal = 0;
        int numberVertical = 0;
        Dot firstPiece = findMatches.currentMatches[0].GetComponent<Dot>();
        if (firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                Dot dot = currentPiece.GetComponent<Dot>();
                if(dot.row == firstPiece.row)
                {
                    numberHorizontal++;
                }
                if(dot.column == firstPiece.column)
                {
                    numberVertical++;
                }
            }
        }
        return (numberVertical == 5 || numberHorizontal == 5);
    }

    private void CheckToMakeBombs()//creates bombs if criteria are met
    {// makes a frow or column bomb if 4 or 7 match is made
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
        {
            findMatches.CheckBombs();
        }
        //makes a rainbow bomb if the match count is 5 or 8
        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if(ColumnOrRow())
            {
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (otherDot.isColorBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeColorBomb();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                    else
                    {
                        if (currentDot.otherDot != null)
                        {
                            Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                            if (otherDot.isMatched)
                            {
                                if (otherDot.isAdjacentBomb)
                                {
                                    otherDot.isMatched = false;
                                    otherDot.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    private void DestroyMatchesAt(int column, int row)//destroys bombs and matches pieces at their location
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            //How many elements are in the matched pieces list from findmatches?
            if (findMatches.currentMatches.Count >= 4)
            {
                CheckToMakeBombs();
            }
           
            if(breakableTiles[column, row]!= null)
            {
                breakableTiles[column, row].TakeDamage(1);
                Debug.Log(" Hp =" + breakableTiles[column, row].hitPoints);
                if ( breakableTiles[column,row].hitPoints <= 0)
                {
                    

                    breakableTiles[column, row]  = null;
                    for (int i = 0; i < boardLayout.Count; i++)
                    {
                        if(boardLayout[i].x == column && boardLayout[i].y == row)
                        {
                            boardLayout.Remove(boardLayout[i]);
                        }
                    }

                    coinCount += 1;
                    Debug.Log("" + coinCount);
                    textComponent.text = (" " + coinCount);
                }

            }
            GameObject particle = Instantiate(DestroyEffect,allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            /*if (allDots[column, row].tag == "Coin Dot")
            {
                coinCount += 1;
                Debug.Log("" + coinCount);
                textComponent.text = (" " + coinCount);

            }*/
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()//helper function that is called to do the acutal destruction
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {

                    DestroyMatchesAt(i, j);

                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }
    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < width ; i ++)
        {
            for (int j = 0; j < height; j ++)
            {
                if (!blankSpaces[i,j] && allDots[i,j] == null)
                    for( int k = j + 1 ; k < height; k ++)
                    {
                        if(allDots[i,k] != null)
                        {
                            allDots[i, k].GetComponent<Dot>().row = j;

                            allDots[i, k] = null;
                            break;
                        }
                    }
            }
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());

    }

    private IEnumerator DecreaseRowCo()//function that helps the board look refilled from the top
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()// refills the nulls spaces left from destruction
    {
        Debug.Log("refilling Board");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i,j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                    allDots[i, j] = piece;

                    if (piece.GetComponent<Dot>().isBreakable)
                    {
                        TileType temp = new TileType();
                        temp.x = i;
                        temp.y = j;
                        temp.tileKind = TileKind.Breakable;
                        boardLayout.Add(temp);
                        Debug.Log("Temp " + temp.x + " , " + temp.y);
                    }

                }
            }
        }
        GenerateBreakableTiles();
        Debug.Log("finsihed refilling Board");

        CheckForMatches();

    }

    private bool MatchesOnBoard()//checks entire board for matchs
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        Debug.Log("No matches in mAtches on board");
        return false;
    }

    private IEnumerator FillBoardCo()//runs necessary helper fuctions to destroy and refill board
    {
        Debug.Log("filling board");
        RefillBoard();
        yield return new WaitForSeconds(.5f);
        
        //CheckForMatches();
        findMatches.FindAllMatches();
        Debug.Log(MatchesOnBoard());
        while (MatchesOnBoard())
        {
            Debug.Log("in Match check");
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
            CheckForMatches();
            
        }

        findMatches.currentMatches.Clear();
        Debug.Log("allDots matches cleard");
        currentDot = null;
        yield return new WaitForSeconds(.5f);
        if(IsDeadLocked())
        {
            ShuffleBoard();
            Debug.Log("DeadLocked"); 
        }

        currentState = GameState.move;

    }
    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        //take the second piece and save it in a holder
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
        // take first piece and switch with second
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
        //set first dot to be second dot
        allDots[column, row] = holder;
    }

    private bool CheckForMatches()
    {
        for( int i = 0; i< width; i++)
        {
            for( int j = 0; j < height; j++)
            {
                if( allDots[i,j] != null)
                {
                    if (i < width - 2)
                    {
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag && allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                Debug.Log("Match found");
                                Debug.Log("found 2 at : " + "[" + i + "," + j + "]" + "[" +(i + 1) + " , " + j + "]" + "[" + (i + 2) + "," + j + "]" );
                                return true;
                                
                            }
                        }
                    }
                    if (j < height - 2)
                    {
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag && allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                Debug.Log("Match found");
                                Debug.Log("found 2 at : " + "[" + i + "," + j + "]" + "[" + i  + " , " + (j + 1) + "]" + "[" + i  + "," + (j + 2) + "]");

                                return true;


                            }
                        }
                    }
                }
            }
        }
        Debug.Log("No matches from Check for matches");
        return false;

    }
    private bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        Debug.Log("switch");
        if(CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;

        }
        SwitchPieces(column, row, direction);
        return false;
    }
    private bool IsDeadLocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if( allDots[i,j] != null)
                {
                    if (i < width - 1)
                    {
                        if(SwitchAndCheck(i,j,Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }


            return true;

    }
    private void ShuffleBoard()
    {
        //create a list of game objects
        List<GameObject> newBoard = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    int pieceToUse = Random.Range(0, newBoard.Count);
                   
                    int maxIterations = 0;

                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                    maxIterations = 0;

                    piece.column = i;
                    piece.row = j;
                    allDots[i, j] = newBoard[pieceToUse];
                    newBoard.Remove(newBoard[pieceToUse]);

                }
            }
        }
        if(IsDeadLocked())
        {
            ShuffleBoard();
        }
    }

}