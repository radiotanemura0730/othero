using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshProの名前空間をインポート

public class PieceManager : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;
    private bool isWhiteTurn = HandicapManager.isStarterWhite;
    private Stack<Move> moveHistory = new Stack<Move>();
    public int whitePieceCount = 2;
    public int blackPieceCount = 2;
    public BoardManager boardManager;
    public BoardEvaluator boardEvaluator;
    public bool isGameEnds;
    public bool isVsCPU = MainMenuManager.isVsCPU;

    void Start()
    {
        whitePieceCount = CountPiece()[0];
        blackPieceCount = CountPiece()[1];

        UpdateScoreDisplay(whitePieceCount, blackPieceCount);

        if (!isWhiteTurn && isVsCPU)
        {
            StartCoroutine(CPUTurn()); // 少し遅れてCPUの手番を開始
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);



            if (hit.collider != null)
            {
                Vector3 hitPosition = hit.transform.position;
                int x = Mathf.RoundToInt(hitPosition.x + 3.5f);
                int y = Mathf.RoundToInt(hitPosition.y + 3.5f);

                if (boardManager.GetPiece(x, y) == null && CanPlacePiece(x, y))
                {
                    PlacePiece(x, y);
                    whitePieceCount = CountPiece()[0];
                    blackPieceCount = CountPiece()[1];

                    UpdateScoreDisplay(whitePieceCount, blackPieceCount);

                    if (!isWhiteTurn && isVsCPU)
                    {
                        StartCoroutine(CPUTurn()); // 少し遅れてCPUの手番を開始
                    }
                }
            }
        }
    }

    IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(1.0f); // CPUが動く前に1秒の遅延を追加

        // CPUが置くべき最適な位置を取得
        int[] bestPosition = boardEvaluator.ChooseBestPosition();

        if (bestPosition != null)
        {
            int x = bestPosition[0];
            int y = bestPosition[1];

            PlacePiece(x, y);
            whitePieceCount = CountPiece()[0];
            blackPieceCount = CountPiece()[1];
            UpdateScoreDisplay(whitePieceCount, blackPieceCount);
            if (!isWhiteTurn)
            {
                StartCoroutine(CPUTurn());
            }
        }
    }

    public void PlacePiece(int x, int y)
    {
        Vector3 position = new Vector3(x - 3.5f, y - 3.5f, -1);
        GameObject piece;
        if (isWhiteTurn)
        {
            piece = Instantiate(whitePiecePrefab, position, Quaternion.identity);
            piece.tag = "White";
            piece.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            piece = Instantiate(blackPiecePrefab, position, Quaternion.identity);
            piece.tag = "Black";
            piece.GetComponent<SpriteRenderer>().color = Color.black;
        }
        piece.transform.parent = boardManager.transform;
        boardManager.PlacePiece(piece, x, y);

        List<Vector2Int> flippedPieces = FlipPieces(x, y, true);
        moveHistory.Push(new Move(x, y, isWhiteTurn, flippedPieces));

        isWhiteTurn = !isWhiteTurn;

        if (!CanAnyPieceBePlaced())
        {
            isWhiteTurn = !isWhiteTurn;

            if (!CanAnyPieceBePlaced())
            {
                isGameEnds = true;
            }
        }
    }


    bool CanAnyPieceBePlaced()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (CanPlacePiece(i, j))
                {
                    return true;
                }
            }
        }
        return false;
    }


    public List<Vector2Int> FlipPieces(int x, int y, bool shouldFlip)
    {
        List<Vector2Int> flippedPieces = new List<Vector2Int>();
        int[,] directions = new int[,]
        {
        {1, 0}, {-1, 0}, {0, 1}, {0, -1},
        {1, 1}, {-1, -1}, {1, -1}, {-1, 1}
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int dx = directions[i, 0];
            int dy = directions[i, 1];
            if (shouldFlip)
            {
                List<Vector2Int> flippedInDirection = FlipInDirection(x, y, dx, dy, true);
                flippedPieces.AddRange(flippedInDirection);
            }
            else
            {
                List<Vector2Int> flippedInDirection = FlipInDirection(x, y, dx, dy, false);
                flippedPieces.AddRange(flippedInDirection);
            }

        }
        return flippedPieces;
    }

    List<Vector2Int> FlipInDirection(int x, int y, int dx, int dy, bool shouldFlip)
    {
        int curX = x + dx;
        int curY = y + dy;
        bool hasOpponentPiece = false;
        List<Vector2Int> piecesToFlip = new List<Vector2Int>();

        while (boardManager.IsValidPosition(curX, curY))
        {
            GameObject piece = boardManager.GetPiece(curX, curY);
            if (piece == null) return new List<Vector2Int>();

            if ((isWhiteTurn && piece.tag == "White") || (!isWhiteTurn && piece.tag == "Black"))
            {
                if (hasOpponentPiece)
                {
                    if (shouldFlip)
                    {
                        FlipInDirectionHelper(x, y, dx, dy);
                    }
                    return piecesToFlip;
                }
                return new List<Vector2Int>();
            }
            else
            {
                hasOpponentPiece = true;
                piecesToFlip.Add(new Vector2Int(curX, curY));
                curX += dx;
                curY += dy;
            }
        }
        return new List<Vector2Int>();
    }

    void FlipInDirectionHelper(int x, int y, int dx, int dy)
    {
        int curX = x + dx;
        int curY = y + dy;

        List<GameObject> piecesToFlip = new List<GameObject>();

        while (boardManager.GetPiece(curX, curY) != null)
        {
            GameObject piece = boardManager.GetPiece(curX, curY);

            if (piece.tag == (isWhiteTurn ? "White" : "Black"))
            {
                foreach (GameObject p in piecesToFlip)
                {
                    p.tag = isWhiteTurn ? "White" : "Black";

                    p.GetComponent<SpriteRenderer>().color = isWhiteTurn ? Color.white : Color.black;
                }
                return;
            }
            else if (piece.tag == (isWhiteTurn ? "Black" : "White"))
            {
                piecesToFlip.Add(piece);
            }
            else
            {
                return;
            }

            curX += dx;
            curY += dy;
        }
    }

    public bool CanPlacePiece(int x, int y)
    {
        if (boardManager.GetPiece(x, y) != null)
        {
            return false;
        }
        else
        {
            int[,] directions = new int[,]
            {
        {1, 0}, {-1, 0}, {0, 1}, {0, -1},
        {1, 1}, {-1, -1}, {1, -1}, {-1, 1}
            };

            for (int i = 0; i < directions.GetLength(0); i++)
            {
                int dx = directions[i, 0];
                int dy = directions[i, 1];
                if (CanFlipInDirection(x, y, dx, dy))
                {
                    return true;
                }
            }
            return false;
        }
    }

    bool CanFlipInDirection(int x, int y, int dx, int dy)
    {
        int curX = x + dx;
        int curY = y + dy;
        bool hasOpponentPiece = false;

        while (boardManager.IsValidPosition(curX, curY))
        {
            GameObject piece = boardManager.GetPiece(curX, curY);
            if (piece == null) return false;

            if ((isWhiteTurn && piece.tag == "White") || (!isWhiteTurn && piece.tag == "Black"))
            {
                return hasOpponentPiece;
            }
            else
            {
                hasOpponentPiece = true;
                curX += dx;
                curY += dy;
            }
        }
        return false;
    }
    List<int> CountPiece()
    {
        int a = 0;
        int b = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject piece = boardManager.GetPiece(i, j);
                if (piece == null) continue;
                else if (piece.GetComponent<SpriteRenderer>().color == Color.white)
                {
                    a++;
                }
                else
                {
                    b++;
                }
            }

        }
        return new List<int> { a, b };
    }
    void UpdateScoreDisplay(int x, int y)
    {
        textDisplay.text = (isWhiteTurn ? "white turn" : "black turn") + "\nwhite:" + x + "\nblack:" + y;

        if (isGameEnds)
        {
            if (x > y)
            {
                textDisplay.text = "white:" + x + "\nblack:" + y + "\nwhite wins!";
            }
            else if (x < y)
            {
                textDisplay.text = "white:" + x + "\nblack:" + y + "\nblack wins!";
            }
            else
            {
                textDisplay.text = "white:" + x + "\nblack:" + y + "\nDraw";
            }
        }
    }

    public void UndoMove()
    {
        if (moveHistory.Count > 0)
        {
            Move lastMove = moveHistory.Pop();
            boardManager.RemovePiece(lastMove.x, lastMove.y);
            isWhiteTurn = lastMove.isWhiteTurn;

            foreach (Vector2Int pos in lastMove.flippedPieces)
            {
                GameObject piece = boardManager.GetPiece(pos.x, pos.y);
                if (piece != null)
                {
                    piece.tag = isWhiteTurn ? "Black" : "White";
                    piece.GetComponent<SpriteRenderer>().color = isWhiteTurn ? Color.black : Color.white;
                }
            }

            whitePieceCount = CountPiece()[0];
            blackPieceCount = CountPiece()[1];
            UpdateScoreDisplay(whitePieceCount, blackPieceCount);

            if (isGameEnds)
            {
                isGameEnds = false;
            }
        }
    }

    public class Move
    {
        public int x;
        public int y;
        public bool isWhiteTurn;
        public List<Vector2Int> flippedPieces;

        public Move(int x, int y, bool isWhiteTurn, List<Vector2Int> flippedPieces)
        {
            this.x = x;
            this.y = y;
            this.isWhiteTurn = isWhiteTurn;
            this.flippedPieces = flippedPieces;
        }
    }

}