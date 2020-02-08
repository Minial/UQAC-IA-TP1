using System.Collections.Generic;


namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Classe représentant un noeud d'un arbre
    /// 
    /// Champs :
    ///     - State : l'état contenu dans le noeud
    ///     - Action : l'action qui a mené à ce noeud
    ///     - Cost : cout de l'action
    ///     - Depth : profondeur du noeud dans l'arbre
    ///     - Parent : noeud parent du noeud courant
    ///     - Children : les noeuds enfant du noeud courant
    /// Méthodes :
    ///     - AddChild() : ajouter un enfant à la liste d'enfants
    /// </summary>
    public class Node 
    {
        public readonly Node Parent;
        public readonly int Depth;
        public readonly int Cost;
        public readonly IState State;
        public readonly IAction Action;
        public List<Node> Children;
        
        public Node(Node parent, int depth, int cost, IState state, IAction action)
        {
            Parent = parent;
            Depth = depth;
            Cost = cost;
            State = state;
            Action = action;
            Children = new List<Node>();
        }
        
        public void AddChild(Node child)
        {
            Children.Add(child);
        }
    }
}