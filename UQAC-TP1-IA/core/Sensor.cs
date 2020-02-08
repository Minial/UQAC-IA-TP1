

namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Permet d'oberver l'envrionnement [IEnvironnement] dans le lequel le capteur "vie"
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
        
        public IPercept Observe()
        {
            return _env.Observe();
        }
    }
}