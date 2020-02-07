using UQAC_TP1_IA.core;

namespace UQAC_TP1_IA.mansion
{
    public class MansionAgent : Agent
    {
        
        public Position PositionAgent;

        public MansionAgent(Sensor sensor, Effector effector, AgentFunction agentFunction) : base(sensor, effector, agentFunction)
        {
        }

        protected override bool ImAlive()
        {
            return true;
        }

        protected override IState UpdateState(IState state, IPercept percept)
        {
            return new MansionState(PositionAgent, (MansionPercept) percept);
        }

        protected override IState FormulateGoal(IState state)
        {
            return new MansionState();
        }

        protected override IProblem FormulateProblem(IState state, IState goal)
        {
            return new MansionProblem((MansionState) state, (MansionState) goal);
        }
    }
}