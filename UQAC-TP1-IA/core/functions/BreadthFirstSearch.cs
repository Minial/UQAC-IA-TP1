using System.Collections.Generic;
using System.Linq;


namespace UQAC_TP1_IA.core.functions
{
    
    /// <summary>
    /// Simple parcours en largeur
    /// </summary>
    public class BreadthFirstSearch : AgentFunction
    {
        public override List<IAction> Search(IProblem problem)
        {
            var node = new Node(null, 0, 0, problem.InitialState(), null);
            var frontier = new Queue<Node>();
            frontier.Enqueue(node);
            var explored = new List<IState>();

            if (problem.GoalTest(node.State)) 
                return Solution(node);
            do
            {
                if (!frontier.Any()) 
                    return null;
                node = frontier.Dequeue();
                if (!explored.Contains(node.State)) explored.Add(node.State);
                foreach (var action in problem.Actions(node.State))
                {
                    var child = ChildNode(problem, node, action);
                    if (!explored.Contains(child.State) || !explored.Contains(child.State))
                    {
                        if (problem.GoalTest(child.State))
                            return Solution(child);
                        frontier.Enqueue(child);
                    }
                }
            } while (true);
        }
        


        public override List<Node> Expand(Node node, IProblem problem)
        {
            throw new System.NotImplementedException();
        }
    }
}