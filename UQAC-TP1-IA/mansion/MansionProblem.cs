using System;
using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// Classe permettant de gérer le capteur du robot
    /// Champs :
    ///     - _initialState et but : états correspondant à l'état initial et a l'état que l'on souhaite atteindre
    /// Méthodes :
    ///     - TestBut() : tester si on a atteint le but
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
        

        public IState InitialState()
        {
            return _initialState;
        }


        public List<IAction> Actions(IState state)
        {
            var mansionState = (MansionState) state;
            var actions = new List<IAction>();
            actions.Add(MansionAction.PICK); // l'ordre important je pense ??
            actions.Add(MansionAction.CLEAN);
            if (mansionState.PositionAgent.x > 0) 
                actions.Add(MansionAction.LEFT);
            if (mansionState.PositionAgent.x < MansionEnv.SIZE-1) 
                actions.Add(MansionAction.RIGHT);
            if (mansionState.PositionAgent.y > 0) 
                actions.Add(MansionAction.TOP);
            if (mansionState.PositionAgent.y < MansionEnv.SIZE - 1) 
                actions.Add(MansionAction.BOTTOM);
            return actions;
        }


        public Dictionary<IAction, IState> Successors(IState state)
        {
            var possibleActions = Actions(state);
            var successors = new Dictionary<IAction, IState>();
            foreach (var action in possibleActions)
            {
                successors[action] = Successor(state, action);
            }
            return successors;
        }
    

        public IState Successor(IState state, IAction action)
        {
            var mansionState = (MansionState) state;
            var newPosition = mansionState.PositionAgent.Copy();
            var newPercept = mansionState.Percept.Copy();
            if (action == MansionAction.TOP)
                newPosition.y--;
            else if (action == MansionAction.BOTTOM)
                newPosition.y++;
            else if (action == MansionAction.LEFT)
                newPosition.x--;
            else if (action == MansionAction.RIGHT)
                newPosition.x++;
            else if (action == MansionAction.CLEAN)
                newPercept.rooms.ElementAt(mansionState.PositionAgent.ToIndex(MansionEnv.SIZE)).State = RoomStateEnum.Clean;
            else if (action == MansionAction.PICK)
                newPercept.rooms.ElementAt(mansionState.PositionAgent.ToIndex(MansionEnv.SIZE)).State = RoomStateEnum.Clean; // en vrai pas forcement, peut content dirt
            return new MansionState(newPosition, newPercept);
        }


        public bool GoalTest(IState state)
        {
            return ((MansionState) state).IsClean(); 
        }


        public int PathCost(IState initialState, IAction action, IState reachState)
        {
            return 1;
        }

    }
}