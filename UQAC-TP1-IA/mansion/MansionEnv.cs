using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// Classe représentant un environnement
    /// Champs :
    ///     - posRobto : position du robot dans le manoir
    ///     - tailleManoir : taille du manoir
    ///     - mesurePerf : mesure de performance
    ///     - manoir : carte du manoir
    /// Méthodes :
    ///     - AjouterDP() : ajouter de la poussière / diamant
    ///     - process() : gère le déroulement
    /// </summary>
    public class MansionEnv : IEnvironment
    {
        public const int SIZE = 5;

        public List<Room> Rooms;
        private MansionAgent _agent;
        public Position PositionAgent;
        private MansionPerformanceMeasure _performanceMeasure;

        public MansionEnv()
        {
            InitBoard();;
        }
        
        public void Run()
        {
            while (true)
            {
                SporadicObjectGeneration();
                Thread.Sleep(750); // diminuer pour car si trop de poussiere / dimant les algos ne pourront pas tourner
            }
        }
        

        /// <summary>
        /// @return
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IPercept Observe() {
            var roomStates = new List<RoomState>();
            foreach (var room in Rooms)
            {
                if (room.dirt && room.diamond) 
                    roomStates.Add(new RoomState(RoomStateEnum.Both, room.pos.Copy()));
                else if (!room.dirt && !room.diamond) 
                    roomStates.Add(new RoomState(RoomStateEnum.Clean, room.pos.Copy()));
                else if (room.diamond) 
                    roomStates.Add(new RoomState(RoomStateEnum.Diamond, room.pos.Copy()));
                else 
                    roomStates.Add(new RoomState(RoomStateEnum.Dirt, room.pos.Copy()));
            }
            return new MansionPercept(PositionAgent?.Copy(), roomStates);
        }

        /// <summary>
        /// @param MansionAction
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Action(IAction action, Agent _)
        {
            var positionAgent = PositionAgent;
            if (action == MansionAction.LEFT && positionAgent.x > 0)
                positionAgent.x--;
            else if (action == MansionAction.TOP && positionAgent.y > 0) 
                positionAgent.y--;
            else if (action == MansionAction.RIGHT && positionAgent.x < SIZE - 1)
                positionAgent.x++;
            else if (action == MansionAction.BOTTOM && positionAgent.y < SIZE-1)
                positionAgent.y++;
            else if (action == MansionAction.CLEAN)
            {
                var room = Rooms.ElementAt(positionAgent.x + positionAgent.y * SIZE);
                if (room.diamond) _performanceMeasure.DiamondClean++;
                if (room.dirt) _performanceMeasure.DirtClean++;
                room.Reset();
            }
            else if (action == MansionAction.PICK)
            {
                var room = Rooms.ElementAt(positionAgent.ToIndex(SIZE));
                if (room.diamond) _performanceMeasure.DiamondPick++;
                room.diamond = false;
            }
            _performanceMeasure.Electricity++;
        }

        public int PerformanceMeasure(Agent _) => _performanceMeasure.Score();

        public MansionPerformanceMeasure PerformancMeasureDetails() => _performanceMeasure;
        

        public void SetAgent(Agent agent, Position initialPosition)
        {
            _agent = (MansionAgent) agent;
            PositionAgent = initialPosition;
            _performanceMeasure = new MansionPerformanceMeasure();
        }
        
        private void InitBoard()
        {
            Rooms = new List<Room>();
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    Rooms.Add(new Room(new Position(j, i)));
                }
            }
        }
        
        /// <summary>
        /// valDP : diamant ou poussiere ou les 2
        /// valX  : valeur en X dans le tableau
        /// valY  : valeur en Y dans le tableau 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void SporadicObjectGeneration()
        {
            var random = new Random();
            var randomVal = random.NextDouble();
            var roomNumber = random.Next(0,SIZE*SIZE);

            if (randomVal < 0.30)
                Rooms.ElementAt(roomNumber).dirt = true;
            else if (randomVal >= 0.30 && randomVal < 0.50)
                Rooms.ElementAt(roomNumber).diamond = true;
        }
    }
    


    public class MansionPerformanceMeasure
    {
        public int DiamondPick;
        public int DirtClean;
        public int DiamondClean;
        public int Electricity;
        public int DirtPick;

        public int Score()
        {
            return DiamondPick + DirtClean - DiamondClean*2 - Electricity;
        }

        public override string ToString()
        {
            var toString = "Score:" + Score();
            toString += "\n\tDirt clean (+) : " + DirtClean;
            toString += "\n\tDiamond pick (+) : " + DiamondPick;
            toString += "\n\tDiamond clean (-) : " + DiamondClean;
            toString += "\n\tDirt pick (-) : " + DirtPick;
            toString += "\n\tElectricity (-) : " + Electricity;
            return toString;
        }
    }

    /// <summary>
    /// Classe représentant une piece du manoir
    /// Champs :
    ///     - poussiere : décrit la présence de poussiere ou non
    ///     - diamant : décrit la présence de diamant ou non
    /// </summary>
    public class Room
    {
        public readonly Position pos;
        public bool diamond, dirt;

        public Room(Position pos, bool diamond = false, bool dirt = false)
        {
            this.pos = pos;
            this.diamond = diamond;
            this.dirt = dirt;
        }
        
        public void Reset()
        {
            diamond = false;
            dirt = false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Room)) return false;
            var otherRoom = (Room) obj;
            return otherRoom.pos.Equals(pos) && otherRoom.diamond == diamond && otherRoom.dirt == dirt;
        }
    }

    public class Position 
    {
        public int x, y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public Position Copy() => new Position(x, y);
        
        public bool Equals(Position otherPosition) => x == otherPosition.x && y == otherPosition.y;

        public int ToIndex(int gridWidth) => x + y * gridWidth;
    }
}