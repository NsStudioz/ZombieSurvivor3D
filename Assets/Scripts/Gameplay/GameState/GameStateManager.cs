namespace ZombieSurvivor3D.Gameplay.GameState
{
    public class GameStateManager
    {

        private static GameStateManager _instance;

        //Events:
        public delegate void GameStateChangeHandler(GameState newGameState);

        // Constructor:
        public static GameStateManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameStateManager();

                return _instance;
            }
        }

        public enum GameState
        {
            Mainmenu,
            Gameplay,
            Paused,
            Gameover
        }

        public GameState CurrentGameState { get; private set; }

        public void SetState(GameState newGameState)
        {
            if (newGameState == CurrentGameState)
                return;

            CurrentGameState = newGameState;
            EventManager<GameState>.Raise(Events.EventKey.OnGameStateChange.ToString(), newGameState);
        }
    }

}
