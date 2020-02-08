using System;
using System.Collections.Generic;

namespace UQAC_TP1_IA.core.functions
{
    public class DepthLimitedSearch : AgentFunction 
    {
        /// <summary>
        /// @param Problem 
        /// @return
        /// </summary>
        public override List<IAction> Search(IProblem problem)
        {
            return IterativeDeepeningSearch(problem);
        }

        public List<IAction> IterativeDeepeningSearch(IProblem problem)
        {
            int depthLimit = 999;//max depth to avoid infinite loop
            var explored = new List<IState>();
            for (int depth = 0; depth < depthLimit; depth++)
            {
                List<IAction> result = DepthLimitedSearchF(problem, depth);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public List<IAction> DepthLimitedSearchF(IProblem problem, int limit)
        {
            return RecursiveDLS(new Node(null, 0, 0, problem.InitialState(), null), problem, limit);
        }

        public List<IAction> RecursiveDLS(Node node, IProblem problem, int limit)
        {
            Boolean cutoffOccurred = false;
            if (problem.GoalTest(node.State))
            {
                Console.WriteLine("solution");
                return Solution(node);
            }
            else if(node.Depth==limit)
            {
                //Console.WriteLine("profondeur limite atteinte");
                return null;
            }
            else
            {
                //Console.WriteLine("recherche en profondeur");
                foreach (var action in problem.Actions(node.State))
                {
                    Node successor = ChildNode(problem, node, action);
                    List<IAction> result = RecursiveDLS(successor, problem, limit);
                    if (result == null)
                    {
                        cutoffOccurred = true;
                    }
                    else
                    {
                        return result;
                    }
                }

            }
            return null;

        }

        /// <summary>
        /// @param Node 
        /// @param Problem 
        /// @return
        /// </summary>
        public override List<Node> Expand(Node node, IProblem problem) {
            throw new NotImplementedException();
        }
    }
}