using System;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    [SerializeField] private GameObject prefabs1;
    [SerializeField] private GameObject prefabs2;
    public enum Seed
    {
        EMPTY, CROSS, NOUGT
    }

    public Seed Turn;
    public Seed[] player = new Seed[9];
    public GameObject[] allSpawns = new GameObject[9];

    private void Awake()
    {
        Turn = Seed.CROSS;
        for(int i = 0; i < 9; i++)
        {
            player[i] = Seed.EMPTY;
        }
    }

    public void Spawn(int id, GameObject emptycell = null)
    {
        if(Turn == Seed.CROSS)
        {
            allSpawns[id] = Instantiate(prefabs1, emptycell.transform.position, Quaternion.identity);
            player[id] = Turn;

            if (CheckWinCondition(Turn))
            {
                Turn = Seed.EMPTY;
                Debug.Log("Ssss 1");
            }
            else
            {
                Turn = Seed.NOUGT;
            }
            Destroy(emptycell);
        }
        else if(Turn == Seed.NOUGT)
        {
            var bestScore = -1;
            var bestPosition = -1;
            for (int i = 0; i < 9; i++)
            {
                if (player[i] == Seed.EMPTY)
                {
                    player[i] = Seed.NOUGT;
                    var score = MiniMax(Seed.CROSS, player, -1000, 1000);
                    player[i] = Seed.EMPTY;
                    if (bestScore < score)
                    {
                        bestScore = score;
                        bestPosition = i;
                    }
                }
            }

            if (bestPosition > -1)
            {
                
                var a = Instantiate(prefabs2, allSpawns[bestPosition].transform.position, Quaternion.identity);
                Destroy(allSpawns[bestPosition]);
                allSpawns[bestPosition] = a;
                player[bestPosition] = Turn;
            }
            
            if (CheckWinCondition(Turn))
            {
                Turn = Seed.EMPTY;
                Debug.Log("Ssss 2");
            }
            else
            {
                Turn = Seed.CROSS;
            }
            
        }
        
    }

    private void FixedUpdate()
    {
        if (Turn == Seed.NOUGT)
        {
            Spawn(0);
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
        for (var i = 0; i < 8; i++)
        {
            if (player[winCases[i, 0]] == currentPlayer &&
                player[winCases[i, 1]] == currentPlayer &&
                player[winCases[i, 2]] == currentPlayer)
            {
                isWin = true;
                break;
            }
        }
        return isWin;
    }

    private int MiniMax(Seed currentPlayer, Seed[] board, int alpha, int beta)
    {
        if (IsDraw()) return 0;
        if(CheckWinCondition(Seed.NOUGT)) return 1;
        if(CheckWinCondition(Seed.CROSS)) return -1;

        int score = 0;
        if (currentPlayer == Seed.NOUGT)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == Seed.EMPTY)
                {
                    board[i] = Seed.NOUGT;
                    score = MiniMax(Seed.CROSS, board, alpha, beta);
                    board[i] = Seed.EMPTY;
                    
                    // alpha beta pruning
                    if (score > alpha) alpha = score;
                    if (alpha >= beta) break;
                }
            }
            return alpha;
        }
        else 
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i] == Seed.EMPTY)
                {
                    board[i] = Seed.CROSS;
                    score = MiniMax(Seed.NOUGT, board, alpha, beta);
                    board[i] = Seed.EMPTY;
                    
                    // alpha beta pruning
                    if (score < alpha) beta = score;
                    if (alpha > beta) break;
                }
            }
            return beta;
        }
    }

    private bool IsAnyEmpty()
    {
        var empty = false;
        for (int i = 0; i < 9; i++)
        {
            if (player[i] == Seed.EMPTY)
            {
                empty = true;
                break;
            }
        }
        return empty;
    }

    private bool IsDraw()
    {
        bool human, machine, anyEmpty;
        human = CheckWinCondition(Seed.CROSS);
        machine = CheckWinCondition(Seed.NOUGT);
        anyEmpty = IsAnyEmpty();
        return human == false & machine == false & anyEmpty == false;
    }
    
}
