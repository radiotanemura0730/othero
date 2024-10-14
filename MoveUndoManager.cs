using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button undoButton;
    public PieceManager pieceManager;

    void Start()
    {
        undoButton.onClick.AddListener(UndoLastMove);
    }

    void UndoLastMove()
    {
        pieceManager.UndoMove();
        pieceManager.UndoMove();  //CPU対戦時は２手戻す必要がある
    }
}
