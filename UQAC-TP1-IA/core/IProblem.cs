using System.Collections.Generic;


namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Modélise un problème à résoudre. Contient les 5 (+1) items suivants :
    /// - L'état initial : [InitialState() : IState]
    /// - La liste des actions possible à partir d'un état : [Actions(IState) : List<IAction>]
    /// - La fonction de succession : [Successors(IState) : Dictionary<IAction, IState>]
    ///     + la fonction permettant de générer l'état obtenue après une action : [Successor(IState, IAction) : IState]
    /// - Le test de but : [GoalTest(IState) : bool]
    /// - Le coût d'un chemin pour aller d'un état à un autre en effectuant un action : [PathCost(IState, IAction, IState) : int]
    ///
    /// Voir [MansionProblem] pour l'implémentation pour le problème du manoir
    /// </summary>
    public interface IProblem
    {
        /// <summary>
        /// @return IState : l'état initial du problème
        /// </summary>
        public IState InitialState();

        /// <summary>
        /// Génère la liste des actions possibles à partir d'un certain état
        /// 
        /// @param state : l'état initial (localement à la fonction)
        /// @return List<IAction> : la liste des actions possible à partir de cet état
        /// </summary>
        public List<IAction> Actions(IState state);
        
        /// <summary>
        /// La fonction de succession du problème
        /// 
        /// @param state : l'état initial (localement à la fonction)
        /// @return Dictionary<IAction, IState> : la map qui associe les actions avec les états obtenus
        /// </summary>
        public Dictionary<IAction, IState> Successors(IState state);

        /// <summary>
        /// Génère l'état successeur à une certain état en effectuant une certaine action
        ///
        /// @param state : l'état initial (localement à la fonction)
        /// @param action : l'action a effectué
        /// @return IState : l'état obtenu
        /// </summary>
        public IState Successor(IState state, IAction action);

        /// <summary>
        /// La fonction de but permettant de savoir si un état est un état désiré ou non
        /// 
        /// @param state : l'état à vérifier
        /// @return bool : vrai si c'est un état désiré (faux sinon
        /// </summary>
        public bool GoalTest(IState state);

        /// <summary>
        /// Le coût du chemin entre 2 état en effectuant une certain action
        ///
        /// @param initialState : l'état de départ
        /// @param action : l'action effectué entre les deux états
        /// @param reachState : l'état d'arrivé
        /// @return int : le coût du chemin
        /// </summary>
        public int PathCost(IState initialState, IAction action, IState reachState);

        /// <summary>
        /// @return int : la mesure d'heuristique d'un état
        /// </summary>
        public int Heuristique(IState state);
    }
}