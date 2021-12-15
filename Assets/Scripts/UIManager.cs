using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

  [SerializeField]
  private Button startServerButton;

  [SerializeField]
  private Button startHostButton;

  [SerializeField]
  private Button startClientButton;

  [SerializeField]
  private TextMeshProUGUI playersInGameText;

  private bool hasServerStarted;

  private void Awake() {
    Cursor.visible = true;
  }

  private void Update() {
    playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
  }

  private void Start() {

    // START SERVER
    startServerButton?.onClick.AddListener(() => {
      if (NetworkManager.Singleton.StartServer())
        Logger.Instance.LogInfo("Server started...");
      else
        Logger.Instance.LogInfo("Unable to start server...");
    });

    // START HOST
    startHostButton?.onClick.AddListener(() => {
      if (NetworkManager.Singleton.StartHost())
        Logger.Instance.LogInfo("Host started...");
      else
        Logger.Instance.LogInfo("Unable to start host...");
    });


    startClientButton.onClick.AddListener(() => {
      if (NetworkManager.Singleton.StartClient()) {
        Logger.Instance.LogInfo("Client started...");
      } else {
        Logger.Instance.LogInfo("Client could not be started...");
      }
    });

    // STATUS TYPE CALLBACKS
    NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
      Logger.Instance.LogInfo($"{id} just connected...");
    };

    NetworkManager.Singleton.OnServerStarted += () => {
      hasServerStarted = true;
    };

  }

}