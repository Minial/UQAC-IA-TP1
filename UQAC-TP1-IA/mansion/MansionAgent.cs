using System.Linq;
using UQAC_TP1_IA.core;

namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// <inheritdoc cref="Agent"/>
    /// Sous class concrète d'[Agent] permettant de définir le comportement de notre agent de le manoir
    ///
    /// En particulier, on définit les méthodes propres à l'agent :
    ///     - UpdateState
    ///     - FomulateGoal
    ///     - FormulateProblem
    /// 
    /// Voir document [IAction] pour plus d'informations
    /// </summary>
    public class MansionAgent : Agent
    {
        public MansionAgent(Sensor sensor, Effector effector, AgentFunction agentFunction, MansionState desire) : base(sensor, effector, agentFunction)
        {
            MentalState.Desire = desire;
        }
        
        /// <summary>
        /// <inheritdoc cref="Agent.ImAlive"/>
        /// L'agent est toujours en vie
        /// </summary>
        protected override bool ImAlive() =>  true;

        /// <summary>
        /// <inheritdoc cref="Agent.UpdateState"/>
        /// On retourne un nouvel état construit avec les perceptions de l'environnement
        /// </summary>
        protected override IState UpdateState(IState state, IPercept percept) =>  new MansionState((MansionPercept) percept);

        /// <summary>
        /// <inheritdoc cref="Agent.FormulateGoal"/>
        /// On retourne toujours un état vide (le but ne change jamais en fonction de l'état courant ici)
        /// </summary>
        protected override IState FormulateGoal(IState state) =>  MentalState.Desire;
        
        /// <summary>
        /// <inheritdoc cref="Agent.FormulateProblem"/>
        /// On formule notre nouveau problème à partir du nouvel état et de l'objectif à atteindre
        /// Dans notre implémentation on n'utilise pas le but a atteindre. EN effet, dans notre implémentation deux
        /// buts avec deux positions d'agent différents seront différents (utile pour les fonctions d'explorations), donc
        /// on utilise directement la fonction [MansionState.IsClean() : bool]
        /// </summary>
        protected override IProblem FormulateProblem(IState state, IState goal) => new MansionProblem((MansionState) state, (MansionState) MentalState.Desire);

        public override string ToString()
        {
            var toString = "Limite du plan d'action : " + PerformanceUnit.Limit + "\n";
            toString += string.Join(" ; ", MentalState.Intention.Select(a => a.ToString()).ToArray());
            return toString;
        }
    }
}