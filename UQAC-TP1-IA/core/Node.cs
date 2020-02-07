using System.Collections.Generic;


namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Classe générique représentant un noeud d'un arbre
    /// Champs :
    ///     - etatCourant : l'état contenu dans le noeud
    ///     - action : l'action qui a mené à ce noeud
    ///     - cout : cout de l'action
    ///     - profondeur : profondeur du noeud dans l'arbre
    ///     - parent : noeud parent du noeud courant
    ///     - enfants : les noeuds enfant du noeud courant
    /// Méthodes :
    ///     - AjouterEnfant() : ajouter un enfant à la liste d'enfants
    /// </summary>
    /// <typeparam name="T"> En général un état</typeparam>
    /// <typeparam name="U"> En général une action</typeparam>
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