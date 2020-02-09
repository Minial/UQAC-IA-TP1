

namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Représente un environnement
    ///
    /// Trois opérations possibles :
    /// - récupérer le perception de l'envrironnement actuelle avec Observe()
    /// - récupérer la mesure de performance de l'agent
    /// - intéragir sur l'environnment en faisant une ation avec Action(IAction, Agent)
    ///
    /// Note: l'implémentation des fonction dépend des caractéristiques de l'environnement (complétement observable ou
    /// partiellement observable)
    ///
    /// Note: l'interface gère les environnement multi-agents, la fonction Action(IAction, Agent) prend en paramètre
    /// aussi l'agent pour savoir quel agent effectue l'action pour mettre à jour sa mesure de performance (avec
    /// le problème du manoir, ça sera juste ignoré vu que c'est un environnement single-agent)
    ///
    /// Voir [MansionEnv] pour l'implémentation pour le problème du manoir
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// @return IPercept : le perception actuelle de l'environnement
        /// </summary>
        public IPercept Observe();

        /// <summary>
        /// @param action : effectue une action par l'agent actuel dans l'environnement
        /// @param agent : l'agent qui effectue l'action (pour gérer les environnements multi-agents)
        /// </summary>
        public void Action(IAction action, Agent agent);

        /// <summary>
        /// @param agent : l'agent qui demande sa performance (pour gérer les environnements multi-agents)
        /// @return int : la performance de l'agent
        /// </summary>
        public int PerformanceMeasure(Agent agent);
    }
}