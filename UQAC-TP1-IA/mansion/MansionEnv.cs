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

        public MansionAgent agent;
        public List<Room> rooms;
        public MansionPerformanceMeasure performanceMeasure;


        public MansionEnv(int size)
        {
            InitBoard();
            rooms.ElementAt(1).dirt = true;
            rooms.ElementAt(2).dirt = true;
            // rooms.ElementAt(11).dirt = true;
            // rooms.ElementAt(12).diamond = true;
            // rooms.ElementAt(13).diamond = true;
            // rooms.ElementAt(24).dirt = true;
            // rooms.ElementAt(21).dirt = true;
            // rooms.ElementAt(17).dirt = true;
            // rooms.ElementAt(6).dirt = true;

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
        public Percept Observe() {
            var roomStates = new List<RoomState>();
            foreach (var room in rooms)
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
            return new MansionPercept(roomStates);
        }

        /// <summary>
        /// @param MansionAction
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Action(IAction action, Agent _)
        {
            var positionAgent = agent.PositionAgent;
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
                var room = rooms.ElementAt(positionAgent.x + positionAgent.y * SIZE);
                if (room.diamond) performanceMeasure.diamondClean++;
                if (room.dirt) performanceMeasure.dirtClean++;
                room.Reset();
            }
            else if (action == MansionAction.PICK)
            {
                var room = rooms.ElementAt(positionAgent.ToIndex(SIZE));
                if (room.diamond) performanceMeasure.diamondPick++;
                if (room.dirt) performanceMeasure.dirtPick++;
                room.Reset();
            }
            else if (action == MansionAction.STUCK)
            {
                
            }
        }

        public void SetAgent(Agent agent, Position initialPosition)
        {
            this.agent = (MansionAgent) agent;
            this.agent.PositionAgent = initialPosition;
            performanceMeasure = new MansionPerformanceMeasure();
        }
        
        private void InitBoard()
        {
            rooms = new List<Room>();
            for (var i = 0; i < SIZE; i++)
            {
                for (var j = 0; j < SIZE; j++)
                {
                    rooms.Add(new Room(new Position(j, i)));
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
            {
                rooms.ElementAt(roomNumber).dirt = true;
            }
            else if (randomVal >= 0.30 && randomVal < 0.40)
            {
                rooms.ElementAt(roomNumber).diamond = true;
            }
        }
    }
    


    public class MansionPerformanceMeasure
    {
        public int diamondPick = 0;
        public int dirtClean = 0;
        public int diamondClean = 0;
        public int dirtPick = 0;

        public int Score()
        {
            return diamondPick + dirtClean - diamondClean*2 - dirtPick;
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

    internal class MansionConsoleRender
    {
        private readonly MansionEnv _environment;
        private readonly MansionAgent _agent;

        public MansionConsoleRender(MansionEnv environment, MansionAgent agent)
        {
            _environment = environment;
            _agent = agent;
        }


        public void Process()
        {
            while (true)
            {
                Draw();
                Thread.Sleep(500);
            }
        }

        private void Draw()
        {
            Console.Clear();

            // Dessin du plateau
            var i = 0;
            foreach (var room in _environment.rooms)
            {
                if (i % MansionEnv.SIZE == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine(new string('-', MansionEnv.SIZE * 6));
                }
                Console.Write(" {0}{1}{2} |", room.dirt ? "P" : " ", room.diamond ? "D" : " ", room.pos.Equals(_environment.agent.PositionAgent) ? "A" : " ");
                i++;
            }

            // Dessin du HUD
            Console.WriteLine();
            Console.WriteLine("Score: {0}", _environment.performanceMeasure.Score());
            Console.WriteLine("\tDirt clean (+): {0}", _environment.performanceMeasure.dirtClean);
            Console.WriteLine("\tDiamond pick (+): {0}", _environment.performanceMeasure.diamondPick);
            Console.WriteLine("\tDirt pick (-): {0}", _environment.performanceMeasure.dirtPick);
            Console.WriteLine("\tDiamond clean (-): {0}", _environment.performanceMeasure.diamondClean);
            
            Console.WriteLine(string.Join(" ; ", _agent.MentalState.Intention.Select(a => a.ToString()).ToArray()));
            Console.WriteLine();
        }
    }
}