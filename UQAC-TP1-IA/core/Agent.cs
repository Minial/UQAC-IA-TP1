using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// État interne de l'agent sous la format BDI (Belief - Desire - Intention)
    ///
    /// - Belief: état actuel de l'envrironnement
    /// - Desire: état désiré
    /// - Intention: liste d'action a effectuer
    /// </summary>
    public class BDI
    {
        public IState Belief;
        public IState Desire;
        public List<IAction> Intention;
        
        public BDI(IState initialBelief)
        {
            Belief = initialBelief;
            Intention = new List<IAction>();
        }
    }
    
    /// <summary>
    /// Agent intelligent, plus particulièrement un agent basé sur les buts
    ///
    /// Donc contient l'état actuel de l'environnement  et le but à atteindre (stocké dans son état mental [BDI]
    /// Il contient aussi une fonction d'exporation [AgentFunction] pour trouver la liste d'action a effectuer pour
    /// atteindre son but.
    ///
    /// L'agent peut intéragir avec l'environnement avec son capteur [Sensor] et son effecteur [Effector]. Le capteur
    /// lui permet d'obtenir une perception de l'environnement [IPercept] et l'effecteur permet d'intéragir avec
    /// l'environnement en effectuant une action [IAction]
    ///
    /// C'est un "Simple problem solving agent"
    /// </summary>
    public abstract class Agent
    {
        private readonly Sensor _sensor;
        private readonly Effector _effector;
        private readonly AgentFunction _function;
        public BDI MentalState;
        
        
        protected Agent(Sensor sensor, Effector effector, AgentFunction agentFunction)
        {
            _sensor = sensor;
            _effector = effector;
            _function = agentFunction;
        }
        
        /// <summary>
        /// Boucle infinie tant que l'agent est en vie [ImAlive():bool] :
        /// - observation de l'environnement
        /// - Résolution du problème [SimpleProblemSolvingAgent(IPercept):IAction]
        /// - interaction avec l'environnement
        /// </summary>
        public void Run(IState initialState)
        {
            MentalState = new BDI(initialState);

            while (ImAlive())
            {
                var percept = _sensor.Observe();
                var action = SimpleProblemSolvingAgent(percept);
                if (action != null)
                    _effector.DoAction(action);
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Résolution du problème:
        /// - mise à jour de l'état ([UpdateState(IState, IPercept) : IState])
        /// - si plan d'action vide :
        ///     - formulation de l'objectif ([FormulateGoal(IState) : IState])
        ///     - formulation du problème ([FormulateProblem(IState, Istate]: IProblem)
        ///     - exploration ([AgentFunction.Search(IProblem):List<Action>])
        ///
        /// @param percept: la perception actuelle de l'environnement
        /// @return IAction: la prochaine action a effectué, null si failure
        /// </summary>
        private IAction SimpleProblemSolvingAgent(Percept percept)
        {
            MentalState.Belief = UpdateState(MentalState.Belief, percept);
            if (!MentalState.Intention.Any())
            {
                MentalState.Desire = FormulateGoal(MentalState.Belief);
                var problem = FormulateProblem(MentalState.Belief, MentalState.Desire);
                MentalState.Intention = _function.Search(problem);
            }

            if (MentalState.Intention != null && MentalState.Intention.Any())
            {
                var action = MentalState.Intention.First();
                MentalState.Intention.RemoveAt(0);
                return action;
            }
            return null;
        }

        protected abstract bool ImAlive();

        /// <summary>
        /// @param state : état courant de l'agent (note: pas besoin pour le robot aspirateur) 
        /// @param percept : perception actuelle de l'environnement
        /// @return : le nouvel état généré
        /// </summary>
        protected abstract IState UpdateState(IState state, Percept percept);

        /// <summary>
        /// @param state : l'état actuel de l'agent 
        /// @return IState : l'état désiré (note: toujours le même pour le robot aspirateur)
        /// </summary>
        protected abstract IState FormulateGoal(IState state);

        /// <summary>
        /// @param state : état actuel de l'agent
        /// @param goal : état désiré
        /// @return IProblem : le problème à résoudre
        /// </summary>
        protected abstract IProblem FormulateProblem(IState state, IState goal);
        
        
    }
}