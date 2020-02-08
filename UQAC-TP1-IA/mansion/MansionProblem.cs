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
                newPercept.rooms.ElementAt(mansionState.Percept.PositionAgent.ToIndex(MansionEnv.SIZE)).State = RoomStateEnum.Clean;
            else if (action == MansionAction.PICK)
                newPercept.rooms.ElementAt(mansionState.Percept.PositionAgent.ToIndex(MansionEnv.SIZE)).State = RoomStateEnum.Clean; // en vrai pas forcement, peut content dirt
            return new MansionState(newPercept);
        }


        public bool GoalTest(IState state)
        {
            return ((MansionState) state).IsClean(); 
        }


        public int PathCost(IState initialState, IAction action, IState reachState)
        {
            return 1;
        }

        /// <summary>
        /// Méthode de calcul de l'heuristique : nombre de cases non vide sur le plateau
        /// </summary>
        /// <param name="state"> Etat courant</param>
        /// <returns></returns>
        public int Heuristique(IState state)
        {
            int h = 0;
            List<RoomState> liste = new List<RoomState>();
            liste = ((MansionState)state).Percept.rooms;
            for (int i = 0; i < liste.Count; i++)
            {
                if (liste[i].State != RoomStateEnum.Clean)
                {
                    h++;
                }
            }
            return h;
        }

    }
}