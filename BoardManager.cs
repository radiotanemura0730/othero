using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject whiteTilePrefab;
    public GameObject blackTilePrefab;
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    public int rows = 8;
    public int columns = 8;
    public float tileSize = 1.0f;
    public GameObject[,] boardArray;

    void Start()
    {
        GenerateBoard();
        InitializeBoardArray();
        InitialBoardPieces(HandicapManager.selectedHandicap);
    }

    void GenerateBoard()
    {
        float startX = -3.5f;
        float startY = -3.5f;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject tile;
                if ((x + y) % 2 == 0)
                {
                    tile = Instantiate(whiteTilePrefab, new Vector3(startX + x * tileSize, startY + y * tileSize, 0), Quaternion.identity);
                }
                else
                {
                    tile = Instantiate(blackTilePrefab, new Vector3(startX + x * tileSize, startY + y * tileSize, 0), Quaternion.identity);
                }
                tile.AddComponent<BoxCollider2D>();
                tile.transform.parent = this.transform;
            }
        }
    }

    void InitializeBoardArray()
    {
        boardArray = new GameObject[columns, rows];
    }

    void InitialPiece(bool isWhite, int x, int y)
    {
        Vector3 position = new Vector3(x - 3.5f, y - 3.5f, -1);
        GameObject piece = isWhite ? Instantiate(whitePiecePrefab, position, Quaternion.identity) : Instantiate(blackPiecePrefab, position, Quaternion.identity);
        PlacePiece(piece, x, y);
    }

    void InitialBoardPieces(int handicap)
    {
        InitialPiece(true, 3, 3);
        InitialPiece(false, 4, 3);
        InitialPiece(false, 3, 4);
        InitialPiece(true, 4, 4);

        if (handicap == 1)
        {
            InitialPiece(true, 0, 0);
        }
        else if (handicap == 2)
        {
            InitialPiece(true, 0, 0);
            InitialPiece(true, 7, 7);
        }
        else if (handicap == 3)
        {
            InitialPiece(true, 0, 0);
            InitialPiece(true, 7, 7);
            InitialPiece(true, 0, 7);
            InitialPiece(true, 7, 0);
        }
        else if (handicap == 4)
        {
            for (int i = 0; i < 8; i++)
            {
                InitialPiece(true, 0, i);
            }
        }
        else if (handicap == 5)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    InitialPiece(true, i, j);
                }
            }
        }
        else if (handicap == 6) {
            for (int i = 0; i < 8; i++)
            {
               InitialPiece(true, 0, i);
               InitialPiece(true, 7, i);
            }
        }
    }

    public void PlacePiece(GameObject piece, int x, int y)
    {
        boardArray[x, y] = piece;
    }

    public GameObject GetPiece(int x, int y)
    {
        return boardArray[x, y];
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public void RemovePiece(int x, int y)
    {
        if (IsValidPosition(x, y) && boardArray[x, y] != null)
        {
            Destroy(boardArray[x, y]);
            boardArray[x, y] = null;
        }
    }
}


