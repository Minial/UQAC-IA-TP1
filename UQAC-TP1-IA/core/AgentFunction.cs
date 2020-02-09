using System.Collections.Generic;


namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// La fonction d'exploration d'un agent
    ///
    /// Permet de résoudre un problème [IProblem] grâce à la fonction Search  ([Search(IProblem) : List<IAction>])
    /// en utilisant un algorithme d'exploration.
    /// La résolution retourne la liste d'actions à effectuer pour résoudre le problème
    ///
    /// Les autres fonction (MakeNode, ChildNode, Solution) sont des fonction privées pouvant être utilisées par les
    /// algorithme d'exploration
    /// </summary>
    public abstract class AgentFunction 
    {


        /// <summary>
        /// Applique un algorithme d'exploration et retourne la liste d'actions à effectuer pour résoudre le problème
        /// 
        /// @param problem : problème à résourdre
        /// @return List<IAction> : liste d'actions a effectué pour résoudre le problème
        /// </summary>
        public abstract List<IAction> Search(IProblem problem);

        /// <summary>
        /// @param node :
        /// @param problem :
        /// @return List<Node> :
        /// </summary>
        public abstract List<Node> Expand(Node node, IProblem problem);

        /// <summary>
        /// @param state :
        /// @return Node : 
        /// </summary>
        protected Node MakeNode(IState state)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Créer un noeud enfant au noeud [parent] dans le cadre du problème [problem]
        ///
        /// @param problem : problème courant, utile pour calculer le coût de chemin
        /// @param parent : noeud parent
        /// @param action : action permettant d'arriver à l'enfant
        /// @return Node : le noeud enfin généré
        /// </summary>
        protected virtual Node ChildNode(IProblem problem, Node parent, IAction action)
        {
            var childState = problem.Successor(parent.State, action);
            var cost = problem.PathCost(parent.State, action, childState);
            return new Node(parent, parent.Depth+1, parent.Cost + cost, problem.Successor(parent.State, action), action);
        }

        /// <summary>
        /// Génère la liste actions ayant permis d'arriver jusqu'à un certain noeud
        ///
        /// @param node : le noeud à remonter
        /// @return List<IAction> : les actions de la racine jusqu'au noeud
        /// </summary>
        protected List<IAction> Solution(Node node)
        {
            var actions = new List<IAction>();
            while (node.Parent != null)
            {
                actions.Add(node.Action);
                node = node.Parent;
            }
            actions.Reverse();
            return actions;
        }
    }
}