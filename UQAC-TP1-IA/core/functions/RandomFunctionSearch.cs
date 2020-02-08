using System;
using System.Collections.Generic;

namespace UQAC_TP1_IA.core.functions
{
    public class RandomFunctionSearch : AgentFunction
    {
        /// <summary>
        /// @param Problem 
        /// @return
        /// </summary>
        public override List<IAction> Search(IProblem problem)
        {
            var rand = new Random();
            var actionIndex = rand.Next(problem.Actions(problem.InitialState()).Count);
            return new List<IAction> {problem.Actions(problem.InitialState())[actionIndex]};
        }


        public override List<Node> Expand(Node node, IProblem problem) {
            throw new NotImplementedException();
        }
    }
}