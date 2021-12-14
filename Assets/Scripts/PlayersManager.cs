using JUSTGames.Core.Singletons;
using Unity.Netcode;

public class PlayersManager : NetworkSingleton<PlayersManager> {
  NetworkVariable<int> playersInGame = new NetworkVariable<int>();

  public int PlayersInGame {
    get {
      return playersInGame.Value;
    }
  }

  void Start() {
    NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
      if (IsServer) {
        playersInGame.Value++;
        Logger.Instance.LogInfo($"{id} just connected...");
      }
    };

    NetworkManager.Singleton.OnClientDisconnectCallback += (id) => {
      if (IsServer) {
        playersInGame.Value--;
        Logger.Instance.LogInfo($"{id} disconnected...");
      }
    };
  }
}



//Logger.Instance.LogInfo($"{id} just connected...");
//Logger.Instance.LogInfo($"{id} disconnected...");
