

namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Classe permettant de gérer le capteur du robot
    /// Champs :
    ///     - env : environnement
    ///     - percManoir : perception obtenu depuis l'environnement
    /// Méthodes :
    ///     - Observer() : observe le manoir
    /// </summary>
    public class Sensor 
    {
        private readonly IEnvironment _env;

        public Sensor(IEnvironment env) {
            _env = env;
        }
        
        public Percept Observe()
        {
            return _env.Observe();
        }
    }
}