using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// Classe permettant de gérer les différents états à partir des perceptions
    /// Champs :
    ///     - état : décrit l'état
    /// Méthodes :
    ///     - Equals() : savoir si deux états sont les mêmes ou pas
    /// </summary>
    public class MansionState : IState 
    {
        public readonly MansionPercept Percept;
        
        public MansionState(MansionPercept percept = null)
        {
            Percept = percept;
        }

        public bool IsClean()
        {
            return Percept.rooms.All(room => room.State == RoomStateEnum.Clean);
        }

        /// <summary>
        ///
        /// Plutôt dégueulasse !
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
            {
                thisDirtyRooms = Percept.rooms.Where(StateCleanPredicate).ToList();
            }
            
            var otherDirtyRooms = new List<RoomState>();
            if (otherStateMansion.Percept != null)
            {
                otherDirtyRooms = otherStateMansion.Percept.rooms.Where(StateCleanPredicate).ToList();
            }
            
            return otherDirtyRooms.SequenceEqual(thisDirtyRooms);
        }
    }
}