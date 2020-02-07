using System.Collections.Generic;
using System.Linq;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    public enum  RoomStateEnum { Clean, Dirt, Diamond, Both}

    public class RoomState
    {
        public RoomStateEnum State;
        public Position Position;

        public RoomState(RoomStateEnum state, Position position)
        {
            State = state;
            Position = position;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is RoomState)) return false;
            var otherRoom = (RoomState) obj;
            return otherRoom.Position.Equals(Position) && State == otherRoom.State;
        }
    }
    /// <summary>
    /// Classe permettant de gérer les perceptions à partir des pieces du manoir
    /// Champs :
    ///     - perception : perception du manoir qu'a l'agent
    ///     - posRobot : position du robot
    /// Méthodes :
    ///     - Convertir() : convertir le manoir en perception du manoir
    /// </summary>
    public class MansionPercept : Percept
    {

        public readonly List<RoomState> rooms; // ou juste liste e booléen ?
        
        public MansionPercept(List<RoomState> rooms)
        {
            this.rooms = rooms;
        }


        public MansionPercept Copy()
        {
            var roomsCopy = new List<RoomState>();
            foreach (var room in rooms)
            {
                roomsCopy.Add(new RoomState(room.State, room.Position.Copy()));
            }
            return new MansionPercept(roomsCopy);
        }

    }
}