using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// <inheritdoc cref="IProblem"/>
    /// Implémentation de [IProblem] pour le problème du manoir. Voir [IProblem] pour plus de détails sur son utilité,
    /// la description de ses fonctions, etc.
    /// </summary>
    public class MansionProblem : IProblem 
    {
        private readonly MansionState _initialState;
        private readonly MansionState _desire;
        
        public MansionProblem(MansionState initialState, MansionState desire)
        {
            _initialState = initialState;
            _desire = desire;
        }

        /// <summary>
        /// <inheritdoc cref="IProblem.InitialState"/>
        /// </summary>
        public IState InitialState() => _initialState;

        
        /// <summary>
        /// <inheritdoc cref="IProblem.Actions"/>
        /// On calcule toutes les actions possibles en fonction de la position de l'agent dans l'état [state]
        /// </summary>
        public List<IAction> Actions(IState state)
        {
            var mansionState = (MansionState) state;
            var actions = new List<IAction> {MansionAction.PICK, MansionAction.CLEAN};
            if (mansionState.Percept.PositionAgent.x > 0) 
                actions.Add(MansionAction.LEFT);
            if (mansionState.Percept.PositionAgent.x < MansionEnv.SIZE-1) 
                actions.Add(MansionAction.RIGHT);
            if (mansionState.Percept.PositionAgent.y > 0) 
                actions.Add(MansionAction.TOP);
            if (mansionState.Percept.PositionAgent.y < MansionEnv.SIZE - 1) 
                actions.Add(MansionAction.BOTTOM);
            return actions;
        }

        /// <summary>
        /// <inheritdoc cref="IProblem.Successors"/>
        /// On retourne tous les états correspondant à toutes les actions possibles
        /// </summary>
        public Dictionary<IAction, IState> Successors(IState state)
        {
            var possibleActions = Actions(state);
            var successors = new Dictionary<IAction, IState>();
            foreach (var action in possibleActions)
                successors[action] = Successor(state, action);
            return successors;
        }
    
        /// <summary>
        /// <inheritdoc cref="IProblem.Successor"/>
        /// </summary>
        public IState Successor(IState state, IAction action)
        {
            var mansionState = (MansionState) state;
            var newPercept = mansionState.Percept.Copy();
            if (action == MansionAction.TOP)
                newPercept.PositionAgent.y--;
            else if (action == MansionAction.BOTTOM)
                newPercept.PositionAgent.y++;
            else if (action == MansionAction.LEFT)
                newPercept.PositionAgent.x--;
            else if (action == MansionAction.RIGHT)
                newPercept.PositionAgent.x++;
            else if (action == MansionAction.CLEAN)
                newPercept.Rooms[mansionState.Percept.PositionAgent.ToIndex(MansionEnv.SIZE)].State = RoomStateEnum.Clean;
            else if (action == MansionAction.PICK)
            {
                var currentRoomState = mansionState.Percept.Rooms.ElementAt(mansionState.Percept.PositionAgent.ToIndex(MansionEnv.SIZE)).State;
                if (currentRoomState == RoomStateEnum.Both)
                    newPercept.Rooms.ElementAt(mansionState.Percept.PositionAgent.ToIndex(MansionEnv.SIZE)).State = RoomStateEnum.Dirt;
                else if (currentRoomState == RoomStateEnum.Diamond)
                    newPercept.Rooms.ElementAt(mansionState.Percept.PositionAgent.ToIndex(MansionEnv.SIZE)).State = RoomStateEnum.Clean;
            }
            return new MansionState(newPercept);
        }

        /// <summary>
        /// <inheritdoc cref="IProblem.GoalTest"/>
        /// Compare l'état avec l'état objectif (l'état objectif contient une position d'agent égal à null, donc
        /// est "équivalent" au 25 états possibles propres)
        /// </summary>
        public bool GoalTest(IState state) => _desire.Equals(state);
        
        /// <summary>
        /// <inheritdoc cref="IProblem.PathCost"/>
        /// Dans notre cas, retourne toujours 1
        /// </summary>
        public int PathCost(IState initialState, IAction action, IState reachState) => 1;


        /// <summary>
        /// <inheritdoc cref="IProblem.Heuristique"/>
        /// Méthode de calcul de l'heuristique : nombre de cases non vide sur le plateau
        /// @param state : état courant
        /// @return la valeur de l'heuristique pour cette pièce
        /// </summary>
        public int Heuristique(IState state)
        {
            var h = 0;
            var liste = new List<RoomState>();
            liste = ((MansionState)state).Percept.Rooms;
            for (var i = 0; i < liste.Count; i++)
            {
                if (liste[i].State != RoomStateEnum.Clean)
                    h++;
            }
            return h;
        }

    }
}