using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    
    /// <summary>
    /// <inheritdoc cref="IAction"/>
    /// 
    /// Classe contenant la liste des actions possible de l'agent aspirateur
    ///
    /// Voir document [IAction] pour plus d'informations
    /// </summary>
    public class MansionAction : IAction
    {
        public static readonly MansionAction LEFT = new MansionAction("left");
        public static readonly MansionAction RIGHT = new MansionAction("right");
        public static readonly MansionAction BOTTOM = new MansionAction("bottom");
        public static readonly MansionAction TOP = new MansionAction("top");
        public static readonly MansionAction CLEAN = new MansionAction("clean");
        public static readonly MansionAction PICK = new MansionAction("pick");

        
        private readonly string _value;
        
        private MansionAction(string value)
        {
            _value = value;
        }

        public override string ToString() => _value;
    }
}