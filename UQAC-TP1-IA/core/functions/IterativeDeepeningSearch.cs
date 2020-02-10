using System;
using System.Collections.Generic;

namespace UQAC_TP1_IA.core.functions
{
    public class IterativeDeepeningSearch : AgentFunction 
    {
        /// <summary>
        /// @param Problem 
        /// @return
        /// </summary>
        public override List<IAction> Search(IProblem problem)
        {
            var depthLimit = 999;//max depth to avoid infinite loop
            for (var depth = 0; depth < depthLimit; depth++)
            {
                var result = DepthLimitedSearchF(problem, depth);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        private List<IAction> DepthLimitedSearchF(IProblem problem, int limit)
        {
            return RecursiveDLS(new Node(null, 0, 0, problem.InitialState(), null), problem, limit);
        }

        private List<IAction> RecursiveDLS(Node node, IProblem problem, int limit)
        {
            var cutoffOccurred = false;
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
                    var successor = ChildNode(problem, node, action);
                    var result = RecursiveDLS(successor, problem, limit);
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