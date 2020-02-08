using System;
using System.Collections.Generic;
using System.Threading;
using UQAC_TP1_IA.core;
using UQAC_TP1_IA.core.functions;
using UQAC_TP1_IA.mansion;

namespace UQAC_TP1_IA
{
    internal static class Application
    {
        private static void Main(string[] args)
        {
            var mansionEnv = new MansionEnv(5);

            var sensor = new Sensor(mansionEnv);
            var effector = new Effector(mansionEnv);
            //var agent = new MansionAgent(sensor, effector, new BreadthFirstSearch());
            var agent = new MansionAgent(sensor, effector, new Astar());
            mansionEnv.SetAgent(agent, new Position(0, 0));


            var envRef = new ThreadStart(mansionEnv.Run);
            var envThread = new Thread(envRef);
            envThread.Start();
            
            var initialAgentState = new MansionState(new MansionPercept(new Position(0,0)));
            var agentRef = new ThreadStart(() => agent.Run(initialAgentState));
            var agentThread = new Thread(agentRef);
            agentThread.Start();

            
            var render = new MansionConsoleRender(mansionEnv, agent);
            var renderRef = new ThreadStart(render.Process);
            var renderThread = new Thread(renderRef);
            renderThread.Start();
        }

    }
}