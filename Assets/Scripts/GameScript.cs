using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class GameScript : MonoBehaviour
{
    public static GameScript Instance;
    [Header("Prefabs")]
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject noughtPrefab;
    [SerializeField] private GameObject cellsPrefab;
    public int size;
    private int boardSize;
    public enum Seed
    {
        EMPTY, CROSS, NOUGT
    }
    public enum Difficulty
    {
        EASY, HARD
    }

    public Difficulty Mode;
    public Seed Turn;
    private Seed[] player;
    private GameObject[] allSpawns;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        boardSize = size * size;
        player = new Seed[boardSize];
        allSpawns = new GameObject[boardSize];
        Turn = Seed.CROSS;
        
        depth = Mode == Difficulty.EASY ? 3 : 6;
        
        for(int i = 0; i < size*size; i++)
        {   
            player[i] = Seed.EMPTY;
        }
        SpawnBoard(size);
    }

    private int move;
    private bool gameEnd;
    private int depth;

    private void SpawnBoard(int size)
    {
        var a1 = (size-1) / 2;
        var cell0 = new Vector3(a1 * -2.5f, a1 * 2.5f, 0);
        
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                var a = Instantiate(cellsPrefab, cell0 + new Vector3(j * 2.5f, i * (-2.5f), 0),
                    quaternion.identity);
                a.GetComponent<EmptyScript>().id = i * size + j;
                allSpawns[i * size + j] = a;
            }
        }
    }
    
    private bool CheckWinCondition(Seed currentPlayer)
    {
        var isWin = false;
        var winCases = new int[8, 3]
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 },
            { 0, 3, 6 },
            { 1, 4, 7 },
            { 2, 5, 8 },
            { 0, 4, 8 },
            { 2, 4, 6 },
        };
        var winCases2 = new int[48, 3]
        {
            { 0, 1, 2 },
            { 1, 2, 3 },
            { 2, 3, 4 },
            
            { 5, 6, 7 },
            { 6, 7, 8 },
            { 7, 8, 9 },
            
            { 10, 11, 12 },
            { 11, 12, 13 },
            { 12, 13, 14 },
            
            { 15, 16, 17 },
            { 16, 17, 18 },
            { 17, 18, 19},
            
            { 20, 21, 22 },
            { 21, 22, 23 },
            { 22, 23, 24 },
            
            { 0, 5, 10 },
            { 5, 10, 15 },
            { 10, 15, 20},
            
            { 1, 6, 11 },
            { 6, 11, 16 },
            { 11, 16, 21 },
            
            { 2, 7, 12 },
            { 7, 12, 17 },
            { 12, 17 , 22},
            
            { 3, 8, 13 },
            { 8, 13, 18 },
            { 13, 18, 23 },
            
            { 4, 9, 14 },
            { 9, 14, 19 },
            { 14, 19, 24 },
            
            { 10, 16, 22 },
            { 5, 11, 17 },
            { 11, 17, 23 },
            
            { 0, 6, 12 },
            { 6, 12, 18 },
            { 12, 18, 24 },
            
            { 1, 7, 13 },
            { 7, 13, 19 },
            { 2, 8, 14 },
            
            { 2, 6, 10 },
            { 3, 7, 11 },
            { 7, 11, 15 },
            
            { 4, 8, 12 },
            { 8, 12, 16 },
            { 12, 16, 20 },
            
            { 9, 13, 17 },
            { 13, 17, 21 },
            { 14, 18, 22 },
        };
        var winCases1 = new int[12, 5]
        {
            { 0,  1,  2,  3,  4 },
            { 5,  6,  7,  8,  9 },
            { 10, 11, 12, 13, 14 },
            { 15, 16, 17, 18, 19 },
            { 20, 21, 22, 23, 24 },
            
            { 0, 5, 10, 15, 20 },
            { 1, 6, 11, 16, 21 },
            { 2, 7, 12, 17, 22 },
            { 3, 8, 13, 18, 23 },
            { 4, 9, 14, 19, 24 },
            
            { 0, 6, 12, 18, 24 },
            { 4, 8, 12, 16, 20 }
        };
        switch (size)
        {
            case 5:
            {
                for (var i = 0; i < 12; i++)
                {
                    if (player[winCases1[i, 0]] == currentPlayer &&
                        player[winCases1[i, 1]] == currentPlayer &&
                        player[winCases1[i, 2]] == currentPlayer &&
                        player[winCases1[i, 3]] == currentPlayer &&
                        player[winCases1[i, 4]] == currentPlayer)
                    {
                        isWin = true;
                        break;
                    }
                }

                return isWin;
            }
            case 3:
            {
                for (var i = 0; i < 8; i++)
                {
                    if (player[winCases[i, 0]] == currentPlayer &&
                        player[winCases[i, 1]] == currentPlayer &&
                        player[winCases[i, 2]] == currentPlayer )
                    {
                        isWin = true;
                        break;
                    }
                }

                return isWin;
            }
            default:
                return false;
        }
    }

    public void Spawn(int id, GameObject emptycell = null)
    {
        if (move == size * size)
        {
            UIManager.Instance.ShowOrHideWinPanel("DRAW",true);
            return;
        }
        if (gameEnd) return;

        if (Turn == Seed.CROSS)
        {
            allSpawns[id] = Instantiate(crossPrefab, emptycell.transform.position, Quaternion.identity);
            player[id] = Turn;

            if (CheckWinCondition(Turn))
            {
                Turn = Seed.EMPTY;
                gameEnd = true;
                UIManager.Instance.ShowOrHideWinPanel("PLAYER 1 WIN",true);
            }
            else
            {
                move++;
                Turn = Seed.NOUGT;
                Spawn(-1, null);
            }
            Destroy(emptycell);
        }
        else if (Turn == Seed.NOUGT)
        {
            int bestPosition = FindBestMove();
            if (bestPosition > -1)
            {
                var a = Instantiate(noughtPrefab, allSpawns[bestPosition].transform.position, Quaternion.identity);
                Destroy(allSpawns[bestPosition]);
                allSpawns[bestPosition] = a;
                player[bestPosition] = Turn;
            }

            if (CheckWinCondition(Turn))
            {
                Turn = Seed.EMPTY;
                gameEnd = true;
                UIManager.Instance.ShowOrHideWinPanel("PLAYER 2 WIN",true);
            }
            else
            {
                move++;
                Turn = Seed.CROSS;
            }
            Destroy(emptycell);
        }
    }

    private int FindBestMove()
    {
        int bestScore = int.MinValue;
        int bestPosition = -1;

        for (int i = 0; i < boardSize; i++)
        {
            if (player[i] == Seed.EMPTY)
            {
                player[i] = Seed.NOUGT;
                int score = MiniMax(Seed.CROSS, player, int.MinValue, int.MaxValue, depth); // Adjust depth here as needed
                player[i] = Seed.EMPTY;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestPosition = i;
                }
            }
        }

        return bestPosition;
    }

    private int MiniMax(Seed currentPlayer, Seed[] board, int alpha, int beta, int depth)
    {
        // Base cases
        if (depth == 0 || IsDraw()) return Evaluate(board); 
        if (CheckWinCondition(Seed.NOUGT)) return 1;
        if (CheckWinCondition(Seed.CROSS)) return -1; 

        // Maximizing player (NOUGT)
        if (currentPlayer == Seed.NOUGT)
        {
            int maxScore = int.MinValue;
            for (int i = 0; i < boardSize; i++)
            {
                if (board[i] == Seed.EMPTY)
                {
                    board[i] = Seed.NOUGT;
                    int score = MiniMax(Seed.CROSS, board, alpha, beta, depth - 1);
                    board[i] = Seed.EMPTY;

                    maxScore = Math.Max(maxScore, score);
                    alpha = Math.Max(alpha, score);

                    if (alpha >= beta)
                    {
                        break; // Beta cutoff
                    }
                }
            }
            return maxScore;
        }
        else // Minimizing player (CROSS)
        {
            int minScore = int.MaxValue;
            for (int i = 0; i < boardSize; i++)
            {
                if (board[i] == Seed.EMPTY )
                {
                    board[i] = Seed.CROSS;
                    int score = MiniMax(Seed.NOUGT, board, alpha, beta, depth - 1);
                    board[i] = Seed.EMPTY;

                    minScore = Math.Min(minScore, score);
                    beta = Math.Min(beta, score);

                    if (alpha >= beta) break; 
                }
            }
            return minScore;
        }
    }

    private bool IsDraw()
    {
        bool humanWin = CheckWinCondition(Seed.CROSS);
        bool machineWin = CheckWinCondition(Seed.NOUGT);
        bool anyEmpty = IsAnyEmpty(player);

        return !humanWin && !machineWin && !anyEmpty;
    }

    private bool IsAnyEmpty(Seed[] player)
    {
        foreach (var cell in player)
        {
            if (cell == Seed.EMPTY)
            {
                return true; // Found an empty cell
            }
        }
        return false; // No empty cells found
    }

    private int Evaluate(Seed[] board)
    {
        // Implement your board evaluation function here
        // Return a score based on the current board position
        return 0;
    }

    private bool CheckSurroundingCells(int index, Seed[] board)
    {
        var a = 0;
        if (index - 1 >= 0)
        {
            if(board[index - 1] != Seed.EMPTY) a++;
        }
        else a++;

        if (index + 1 < board.Length)
        {
            if (board[index + 1] != Seed.EMPTY) a++;
        }
        else a++;
        
        if (index - size >= 0)
        {
            if(board[index - size] != Seed.EMPTY) a++;
        }
        else a++;
        
        if (index + size < board.Length)
        {
            if (board[index + size] != Seed.EMPTY) a++;
        }
        else a++;
        
        return a > 1;
    }
}
