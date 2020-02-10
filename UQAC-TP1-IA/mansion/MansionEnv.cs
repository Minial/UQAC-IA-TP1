using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UQAC_TP1_IA.core;


namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// <inheritdoc cref="IEnvironment"/>
    /// 
    /// Voir la doc de [IEnvironment] pour davantage d'informations. Implémente simplement les fonctions de l'interface
    /// pour le problème du manoir.
    /// Il contient une liste de pièces (Room), un agent (environnement single-agent), sa position et sa mesure de
    /// performance.
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
        /// <inheritdoc cref="IEnvironment.Observe"/>
        /// @return la perception du manoir
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
        /// <inheritdoc cref="IEnvironment.Action"/>
        /// 
        /// @param action : effectue cette action dans le manoir
        /// @param _ (Agent) : single agent donc on sait forcement quel agent effectue l'action
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
        
        /// <summary>
        /// <inheritdoc cref="IEnvironment.PerformanceMeasure"/>
        /// @return le score de la mesure de performance
        /// </summary>
        public int PerformanceMeasure(Agent _) => _performanceMeasure.Score();

        /// <summary>
        /// @return la mesure de performance au complet
        /// </summary>
        public MansionPerformanceMeasure PerformancMeasureDetails() => _performanceMeasure;
        
        /// <summary>
        /// Défini le nouvel agent du manoir (single-agent, donc supprimer potentiellement l'autre)
        /// </summary>
        public void SetAgent(Agent agent, Position initialPosition)
        {
            _agent = (MansionAgent) agent;
            PositionAgent = initialPosition;
            _performanceMeasure = new MansionPerformanceMeasure();
        }
        
        /// <summary>
        /// Initiliase les pièces du manoir
        /// </summary>
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
        /// Méthode de génération de poussière ou de diamant dans une pièce aléatoire
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
    

    /// <summary>
    /// Mesure de performance pour un agent évoluant dans le manoir
    ///
    /// Le score est simplement une combinaison linéaire des informations contenu dans le mesure de performance
    /// (DiamondPick, DirtClean, DiamondClean, Electricity)
    /// </summary>
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
    ///     - dirt : décrit la présence de poussiere ou non
    ///     - diamond : décrit la présence de diamant ou non
    ///     - pos: position de la pièce dans le manoir
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
    
    /// <summary>
    /// Position dans un espace à 2 dimensions
    /// </summary>
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