namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Permet d'oberver l'envrionnement [IEnvironnement] dans le lequel le capteur "vie" et de recupérer la mesure
    /// de performance d'un agent
    ///
    /// Nécessite de connaitre l'environnement lors de sa création, et observe celui-ci grâce à la fonction
    /// [IEnvironnement.Observe() : IPercept].
    ///
    /// Donc possède une unique fonction [Observe() : IPercept] qui permet d'observer l'environnement
    /// </summary>
    public class Sensor 
    {
        private readonly IEnvironment _env;

        public Sensor(IEnvironment env) {
            _env = env;
        }
        
        /// <summary>
        /// Permet d'obtenir la perception de l'environnement
        /// </summary>
        public IPercept Observe()
        {
            return _env.Observe();
        }
        
        /// <summary>
        /// Permet d'obtenir la mesure de performance de l'agent (on passe l'agent pour les env multi-agents)
        /// </summary>
        public int PerformanceMeasure(Agent agent)
        {
            return _env.PerformanceMeasure(agent);
        }
    }
}