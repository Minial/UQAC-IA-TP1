using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{

    /// <summary>
    /// <inheritdoc cref="IPercept"/>
    /// Représente la perception du manoir à un instant
    /// 
    /// Contient donc l'état de toutes les pièces ([RoomState]) ainsi que la position de l'agent dans l'environnement
    /// </summary>
    public class MansionPercept : IPercept
    {
        public readonly List<RoomState> Rooms;
        public readonly Position PositionAgent;
        
        public MansionPercept(Position positionAgent, List<RoomState> rooms = null)
        {
            Rooms = rooms;
            PositionAgent = positionAgent;
        }

        /// <summary>
        /// Mecanisme de clonage pour créer plusieurs état différents avec plusieurs perception et eventuellement
        /// les modifier si besoin
        /// </summary>
        public MansionPercept Copy()
        {
            var roomsCopy = Rooms.Select(room => new RoomState(room.State, room.Position.Copy())).ToList();
            return new MansionPercept(PositionAgent.Copy(), roomsCopy);
        }
    }
    
    /// <summary>
    /// États possibles pour une pièce
    /// </summary>
    public enum  RoomStateEnum { Clean, Dirt, Diamond, Both}
    
    /// <summary>
    /// Représente l'état d'une pièce avec sa position dans le manoir
    /// </summary>
    public class RoomState
    {
        public RoomStateEnum State;
        public Position Position;

        public RoomState(RoomStateEnum state, Position position)
        {
            State = state;
            Position = position;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoomState)) return false;
            var otherRoom = (RoomState) obj;
            return otherRoom.Position.Equals(Position) && State == otherRoom.State;
        }
    }
}