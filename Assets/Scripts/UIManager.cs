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

  private void Awake() {
    Cursor.visible = true;
  }

  private void Update() {
    playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
  }

  private void Start() {
    startHostButton.onClick.AddListener(() => {
      if (NetworkManager.Singleton.StartHost()) {
        Logger.Instance.LogInfo("Host started...");
      } else {
        Logger.Instance.LogInfo("Host could not be started...");
      }
    });

    startServerButton.onClick.AddListener(() => {
      if (NetworkManager.Singleton.StartServer()) {
        Logger.Instance.LogInfo("Server started...");
      } else {
        Logger.Instance.LogInfo("Server could not be started...");
      }
    });

    startClientButton.onClick.AddListener(() => {
      if (NetworkManager.Singleton.StartClient()) {
        Logger.Instance.LogInfo("Client started...");
      } else {
        Logger.Instance.LogInfo("Client could not be started...");
      }
    });
  }

}