

namespace UQAC_TP1_IA.core
{
    /// <summary>
    /// Permet d'intéragir avec l'environnement [IEnvironnement] dans lequel l'effecteur "vie"
    ///
    /// Nécessite de connaitre l'environnement lors de sa création, et intéragit avec celui-ci grâce à la fonction
    /// [IEnvironnement.Action(IAction)].
    ///
    /// Donc possède une unique fonction [DoAction(IAction)] qui permet d'effecter une action dans l'environnement
    /// </summary>
    public class Effector 
    {
        
        private readonly IEnvironment _env;

        public Effector(IEnvironment env)
        {
            _env = env;
        }

        public void DoAction(IAction action) {
            _env.Action(action, null);
        }

    }
}