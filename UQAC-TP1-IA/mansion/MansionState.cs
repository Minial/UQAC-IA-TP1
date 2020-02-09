using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// Représente l'état du manoir, contient uniquement une perception de celui-ci.
    ///
    /// Il y a également l'implémentation de la fonction Equals pour comparer deux états.
    /// 
    /// Dans notre implémentation deux états avec deux positions d'agent différents NE sont PAS égaux (car
    /// les fonction d'exploration doivent considérer ces deux états comme effectivement différents)
    /// Cependant si un état possède un position d'agent null, il peut être égal à un autre état avec  un position
    /// d'agent non null (à condition que l'état des pièces soit identique)
    /// </summary>
    public class MansionState : IState 
    {
        public readonly MansionPercept Percept;
        
        public MansionState(MansionPercept percept)
        {
            Percept = percept;
        }
        

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
            if (otherStateMansion.Percept.PositionAgent != null 
                && Percept.PositionAgent != null 
                && !otherStateMansion.Percept.PositionAgent.Equals(Percept.PositionAgent))
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