using System.Collections.Generic;

namespace UQAC_TP1_IA.core.functions
{
    public class Astar : AgentFunction
    {
        public override List<Node> Expand(Node node, IProblem problem)
        {
            var liste = new List<Node>();
            foreach (var action in problem.Actions(node.State))
            {
                var child = ChildNode(problem, node, action);
                liste.Add(child);
            }
            return liste;
        }

        public override List<IAction> Search(IProblem problem)
        {
            var node = new Node(null, 0, 0, problem.InitialState(), null);
            var children = new List<Node>();
            var frontier = new List<Node>();
            var explored = new List<IState>();
            Node tmp;

            // On ajoute la racine
            frontier.Add(node);
            do
            {
                // Si la frontiere est vide, on arr�te
                if (frontier.Count == 0)
                {
                    return null;
                }
                // Si on trouve la solution, on la retourne
                if (problem.GoalTest(node.State))
                {
                    return Solution(node);
                }
                // On prend le noeud avec le cout le plus petit
                node = frontier[0];
                frontier.RemoveAt(0);
                // On l'ajoute � la lsite des noeuds explor�s si besoin
                if (!explored.Contains(node.State)) explored.Add(node.State);
                // On g�n�re les enfants
                children = Expand(node,problem);
                // On ajoute l'enfant au bon endroit
                for (var i = 0; i < children.Count; i++)
                {
                    if (!explored.Contains(children[i].State))
                    {
                        frontier.Add(children[i]);
                        for (var j = frontier.Count - 1; j > 0; j--)
                        {
                            if (frontier[j].Cost < frontier[j - 1].Cost)
                            {
                                tmp = frontier[j];
                                frontier[j] = frontier[j-1];
                                frontier[j-1] = tmp;
                            }
                        }
                    }
                }
            } while (true);
        }

        /// <summary>
        /// Cr�ation d'un enfant en tenant compte de l'heuristique
        /// </summary>
        /// <returns></returns>
        protected override Node ChildNode(IProblem problem, Node parent, IAction action)
        {
            var childState = problem.Successor(parent.State, action);
            var cost = problem.PathCost(parent.State, action, childState);
            var h = problem.Heuristique(childState);
            return new Node(parent, parent.Depth + 1, parent.Cost + cost + h, problem.Successor(parent.State, action), action);
        }

    }
}