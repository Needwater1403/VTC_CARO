// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public static class Constants
// {
//     //==============COORDINATES==============
//     public static readonly Vector3 Board5x5_Cell0 = new Vector3(-5f, 5f,0);
//     public static readonly Vector3 Player2StartPos = new Vector3(1.5f, -1.5f,0);
//     
//     //=============PARAMETERS=============
//     public const float VisibleSizeWidth = 5f;
//  public void Spawn(int id, GameObject emptycell = null)
//     {
//         if (move == size*size)
//         {
//             Debug.Log("DRAW");
//             return;
//         }
//         if(gameEnd) return;
//         if(Turn == Seed.CROSS)
//         {
//             allSpawns[id] = Instantiate(crossPrefab, emptycell.transform.position, Quaternion.identity);
//             player[id] = Turn;
//
//             if (CheckWinCondition(Turn))
//             {
//                 Turn = Seed.EMPTY;
//                 gameEnd = true;
//                 Debug.Log("PLAYER 1 WIN");
//             }
//             else
//             {
//                 move++;
//                 Turn = Seed.NOUGT;
//                 Spawn(-1, null);
//             }
//             Destroy(emptycell);
//         }
//         else if(Turn == Seed.NOUGT)
//         {
//             var bestScore = -1;
//             var bestPosition = -1;
//             for (int i = 0; i < boardSize; i++)
//             {
//                 if (player[i] == Seed.EMPTY)
//                 {
//                     player[i] = Seed.NOUGT;
//                     var score = MiniMax(Seed.CROSS, player, -1000, 1000, 2);
//                     player[i] = Seed.EMPTY;
//                     if (bestScore < score)
//                     {
//                         bestScore = score;
//                         bestPosition = i;
//                     }
//                 }
//             }
//             if (bestPosition > -1)
//             {
//                 var a = Instantiate(noughtPrefab, allSpawns[bestPosition].transform.position, Quaternion.identity);
//                 Destroy(allSpawns[bestPosition]);
//                 allSpawns[bestPosition] = a;
//                 player[bestPosition] = Turn;
//             }
//             
//             if (CheckWinCondition(Turn))
//             {
//                 Turn = Seed.EMPTY;
//                 gameEnd = true;
//                 Debug.Log("PLAYER 2 WIN");
//             }
//             else
//             {
//                 move++;
//                 Turn = Seed.CROSS;
//             }
//             Destroy(emptycell);
//         }
//     }
//     private int MiniMax(Seed currentPlayer, Seed[] board, int alpha, int beta, int depth)
//     {
//         // Base cases
//         if (depth == 0 || IsDraw())
//         {
//             return Evaluate(board); // Evaluate the board position if depth is 0 or it's a draw
//         }
//
//         // Check for terminal states (wins)
//         if (CheckWinCondition(Seed.NOUGT))
//         {
//             return 1; // Maximizer (NOUGT) wins
//         }
//         if (CheckWinCondition(Seed.CROSS))
//         {
//             return -1; // Minimizer (CROSS) wins
//         }
//
//         // Maximizing player (NOUGT)
//         if (currentPlayer == Seed.NOUGT)
//         {
//             int maxScore = int.MinValue;
//             for (int i = 0; i < boardSize; i++)
//             {
//                 if (board[i] == Seed.EMPTY)
//                 {
//                     board[i] = Seed.NOUGT;
//                     int score = MiniMax(Seed.CROSS, board, alpha, beta, depth - 1);
//                     board[i] = Seed.EMPTY;
//
//                     maxScore = Math.Max(maxScore, score);
//                     alpha = Math.Max(alpha, score);
//
//                     if (alpha >= beta)
//                     {
//                         break; // Beta cutoff
//                     }
//                 }
//             }
//             return maxScore;
//         }
//         else // Minimizing player (CROSS)
//         {
//             int minScore = int.MaxValue;
//             for (int i = 0; i < boardSize; i++)
//             {
//                 if (board[i] == Seed.EMPTY)
//                 {
//                     board[i] = Seed.CROSS;
//                     int score = MiniMax(Seed.NOUGT, board, alpha, beta, depth - 1);
//                     board[i] = Seed.EMPTY;
//
//                     minScore = Math.Min(minScore, score);
//                     beta = Math.Min(beta, score);
//
//                     if (alpha >= beta)
//                     {
//                         break; // Alpha cutoff
//                     }
//                 }
//             }
//             return minScore;
//         }
//     }
//
//
//     private bool IsAnyEmpty()
//     {
//         return player.Any(cell => cell == Seed.EMPTY);
//     }
//
//     private bool IsDraw()
//     {
//         bool human, machine, anyEmpty;
//         human = CheckWinCondition(Seed.CROSS);
//         machine = CheckWinCondition(Seed.NOUGT);
//         anyEmpty = IsAnyEmpty();
//         return human == false & machine == false & anyEmpty == false;
//     }
//     
// }