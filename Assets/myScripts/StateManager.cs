namespace myScripts {
    public class StateManager {

        public StateManager( ) {
            Instance = this;
        }

        public StateManager Instance { get; }
        public bool BaseCreated { get; set; }
        public bool ActivePieces { get; set; }
        public bool SimulationRunning { get; set; }

    }
    public abstract class State {

        

    }
    public class RunSimState : State {

        
        
        

    }
}