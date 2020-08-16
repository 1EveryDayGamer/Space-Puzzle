﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{

    private Board board;//possible because there will be only one board object called 
    public List<GameObject> currentMatches = new List<GameObject>();


    void Start()
    {
        board = FindObjectOfType<Board>();//stated above that only 1 board will ever be used m
    }
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }
    private List<GameObject> IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }
        if (dot2.isAdjacentBomb)//columnBomb at upperedge in row
        {
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }
        if (dot3.isAdjacentBomb)//rowBomb at bottomedge in row
        {
            currentMatches.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }
        return currentDots;

    }
    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isRowBomb)//columnBomb in row
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }
        if (dot2.isRowBomb)//columnBomb at upperedge in row
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }
        if (dot3.isRowBomb)//rowBomb at bottomedge in row
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }
        return currentDots;


    }
    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isColumnBomb)//columnBomb in row
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
        }
        if (dot2.isColumnBomb)//columnBomb at upperedge in row
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
        }
        if (dot3.isColumnBomb)//rowBomb at bottomedge in row
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
        }
        return currentDots;


    }
    private void AddToListAndMatch(GameObject dot)
    {
        Debug.Log("hello wheres the match");
        if(!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }
        
        dot.GetComponent<Dot>().isMatched = true;
    }
    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);

        AddToListAndMatch(dot2);

        AddToListAndMatch(dot3);
    }
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.3f);

        for (int i = 0; i < board.width; i++)
        {
            //Debug.Log(" = " + width + " , " + i);
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();

                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];

                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            if (leftDot != null && rightDot != null)
                            {
                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                                {
                                    currentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));

                                    currentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));

                                    currentMatches.Union(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));

                                    GetNearbyPieces(leftDot, currentDot, rightDot);

                                }
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];

                        GameObject downDot = board.allDots[i, j - 1];

                        if (downDot != null && upDot != null)
                        {

                            Dot downDotDot = downDot.GetComponent<Dot>();
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            if (upDot != null && downDot != null)
                            {
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                                {


                                    currentMatches.Union(IsColumnBomb(upDotDot, currentDotDot, downDotDot));

                                    currentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));

                                    currentMatches.Union(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));

                                    GetNearbyPieces(upDot, currentDot, downDot);

                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void MatchPiecesOfColor(string color)
    {
        for(int i = 0; i < board.width; i ++ )
        {
            for( int j = 0; j < board.height; j ++)
            {
                if( board.allDots[i,j] != null)
                {
                    if (board.allDots[i,j].tag == color)
                    {
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                        Debug.Log("found 1 at : " +  board.allDots[i, j].GetComponent<Transform>().position);
                    }
                }
            }
        }

    }

    List<GameObject> GetAdjacentPieces(int column, int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for( int i = column -1; i <= column +1; i++)
        {
            for( int j = row - 1; j <= row +1; j++)
            {
                if ( i >= 0 && i <  board.width && j >= 0 && j < board.height)
                {
                    dots.Add(board.allDots[i, j]);
                    board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    
                }
            }
        }
        return dots;
    }
    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }
    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row ]!= null)
            {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }
    public void CheckBombs()//WORKING
    {
        if(board.currentDot != null)
        {
            if (board.currentDot.isMatched)
            {
                board.currentDot.isMatched = false;
                /*int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 50)
                {
                    board.currentDot.MakeRowBomb();
                }
                else if(typeOfBomb >= 50)
                {
                    board.currentDot.MakeColumnBomb();
                }*/
                if((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || (board.currentDot.swipeAngle <-135 || board.currentDot.swipeAngle >= 135))
                {
                    board.currentDot.MakeRowBomb();   
                }
                else
                {
                    board.currentDot.MakeColumnBomb();
                }
            }
            else if (board.currentDot.otherDot != null)//other piece matched
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                if(otherDot.isMatched)
                {
                    otherDot.isMatched = false;
                    /*
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        otherDot.MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        otherDot.MakeColumnBomb();
                    }
                    */
                    if ((board.currentDot.swipeAngle >  -45 && board.currentDot.swipeAngle <= 45) || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135))
                    {
                        otherDot.MakeRowBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }

                }
            }
        }
    }
}
