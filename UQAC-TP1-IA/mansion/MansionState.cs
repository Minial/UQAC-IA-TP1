using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// Représente l'état du manoir, contient uniquement une perception de celui-ci et une fonction permettant de savoir
    /// si le manoir est propre dans cet état là
    ///
    /// Il y a également l'implémentation de la fonction Equals pour comparer deux états.
    /// 
    /// Note : dans notre implémentation deux états avec deux positions d'agent différents NE sont PAS égaux (car
    /// les fonction d'exploration doivent considérer ces deux états comme effectivement différents)
    /// </summary>
    public class MansionState : IState 
    {
        public readonly MansionPercept Percept;
        
        public MansionState(MansionPercept percept = null)
        {
            Percept = percept;
        }
        
        /// <summary>
        /// Permet de savoir si l'état est propre (aucune poussière et aucun diamant)
        /// </summary>
        public bool IsClean() => Percept.Rooms.All(room => room.State == RoomStateEnum.Clean);
        

        /// <summary>
        /// Permet de savoir si deux états sont égaux ou non
        ///
        /// TODO: La fonction n'est pas très propre, on pourrait la remanier je pense
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is MansionState))
                return false;
            
            var otherStateMansion = (MansionState) obj;
            if (otherStateMansion.Percept == null ^ Percept == null)
                return false;
            if (otherStateMansion.Percept == null && Percept == null)
                return true;
            if (otherStateMansion.Percept.PositionAgent == null ^ Percept.PositionAgent == null)
                return false;
            
            if (otherStateMansion.Percept.PositionAgent != null && !otherStateMansion.Percept.PositionAgent.Equals(Percept.PositionAgent))
                return false;

            static bool StateCleanPredicate(RoomState room) => room.State == RoomStateEnum.Clean;

            var thisDirtyRooms = new List<RoomState>();
            if (Percept != null)
                thisDirtyRooms = Percept.Rooms.Where(StateCleanPredicate).ToList();

            var otherDirtyRooms = new List<RoomState>();
            if (otherStateMansion.Percept != null)
                otherDirtyRooms = otherStateMansion.Percept.Rooms.Where(StateCleanPredicate).ToList();
            
            return otherDirtyRooms.SequenceEqual(thisDirtyRooms);
        }
    }
}