using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoardEvaluator : MonoBehaviour
{
    public BoardManager boardManager; 
    public PieceManager pieceManager; 
    public int rows; 
    public int columns;
    public List<int[]> dangerPositions;
    public List<int[]> notPrefferedPositions;
    public List<int[]> cornerPositions;

    void Awake()
    {

        rows = boardManager.rows;
        columns = boardManager.columns;
        dangerPositions = new List<int[]>
        {
            new int[] {1, 1},
            new int[] {1, rows - 2},
            new int[] {columns - 2, 1},
            new int[] {columns - 2, rows - 2},
        };

        notPrefferedPositions = new List<int[]>
        {
            new int[] {0, 1},
            new int[] {1, 0},
            new int[] {columns - 2, 0},
            new int[] {1, rows - 1},
            new int[] {columns - 2, 0},
            new int[] {columns - 1, 1},
            new int[] {columns - 2, rows - 1},
            new int[] {columns - 1, rows - 2},
        };

        cornerPositions = new List<int[]>
        {
            new int[] {0, 0},
            new int[] {0, rows - 1},
            new int[] {columns - 1, 0},
            new int[] {columns - 1, rows - 1},
        };

    }


    // 盤面の状態を取得し、評価結果を2D配列で返す。置けないマスは-999、置けるマスは1。
    public int[,] EvaluateBoard()
    {
        int[,] evaluationBoard = new int[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // その場所に置けるかどうかをPieceManagerを使って判定
                if (pieceManager.CanPlacePiece(x, y))
                {
                    evaluationBoard[x, y] = 1; // 置ける場合は1
                }
                else
                {
                    evaluationBoard[x, y] = -999; // 置けない場合は-999
                }
            }
        }

        return evaluationBoard;
    }

    public List<int[]> GetPlaceablePositions()
    {
        List<int[]> placeablePositions = new List<int[]>();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // 駒を置けるかどうかをPieceManagerを使って判定
                if (pieceManager.CanPlacePiece(x, y))
                {
                    // 置ける場所をリストに座標として追加する
                    placeablePositions.Add(new int[] { x, y });
                }
            }
        }

        return placeablePositions;
    }

    public int CountEmptySpacesAroundPosition(int x, int y, int[] placeToPiece)
    {
        int[,] directions = Directions.AllDirections;

        int emptyCount = 0;

        pieceManager.PlacePiece(placeToPiece[0], placeToPiece[1]);

        // 各方向についてループ
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int dx = directions[i, 0];
            int dy = directions[i, 1];
            int newX = x + dx;
            int newY = y + dy;

            // 新しい座標が有効範囲内かをチェック
            if (boardManager.IsValidPosition(newX, newY))
            {
                // その位置に駒がない場合（空きマスの場合）
                if (boardManager.boardArray[newX, newY] == null)
                {
                    emptyCount++;
                }
            }
        }

        pieceManager.UndoMove();
        return emptyCount;
    }

    public int MoveEvaluator(int[] evaluatePosition)
    {
        int emptySpaces = 0;


        int placeX = evaluatePosition[0];
        int placeY = evaluatePosition[1];

        // ひっくり返すマスを取得
        List<Vector2Int> flippableFromPosition = pieceManager.FlipPieces(placeX, placeY, false);

        foreach (Vector2Int coordinates in flippableFromPosition)
        {
            int coordinatesX = coordinates.x;
            int coordinatesY = coordinates.y;

            emptySpaces += CountEmptySpacesAroundPosition(coordinatesX, coordinatesY, evaluatePosition);
        }

        return emptySpaces;
    }

    public int[] ChooseBestPosition()
    {
        List<int[]> placeablePositions = GetPlaceablePositions();

        if (placeablePositions.Count == 0)
        {
            return null; // 置ける場所がなければnullを返す
        }

        int[] bestPosition = placeablePositions[0]; // 最初の位置を仮の最適位置とする
        int minEmptySpaces = int.MaxValue; 

        // 各placeablePositionについて評価
        foreach (int[] coordinates in placeablePositions)
        {
            int emptySpaces = MoveEvaluator(coordinates);

            if (dangerPositions.Any(pos => pos[0] == coordinates[0] && pos[1] == coordinates[1]))
            {
                emptySpaces += 20;
            }

            if (notPrefferedPositions.Any(pos => pos[0] == coordinates[0] && pos[1] == coordinates[1]))
            {
                emptySpaces += 10;
            }

            if (cornerPositions.Any(pos => pos[0] == coordinates[0] && pos[1] == coordinates[1]))
            {
                emptySpaces -= 20;
            }

            // emptySpacesが現在の最小値より小さい場合、最適位置を更新
            if (emptySpaces < minEmptySpaces)
            {
                minEmptySpaces = emptySpaces;
                bestPosition = coordinates;
            }
        }

        return bestPosition;
    }

}
