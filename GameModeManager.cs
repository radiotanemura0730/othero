using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }

    // 対戦モード (true = CPU, false = 対人)
    public bool IsCpuMode { get; private set; }

    private void Awake()
    {
        // シングルトンの設定
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // モードを設定
    public void SetMode(bool isCpuMode)
    {
        IsCpuMode = isCpuMode;
    }
}