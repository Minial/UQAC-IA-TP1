using System;
using System.Threading;
namespace UQAC_TP1_IA.mansion
{
    /// <summary>
    /// Juste une classe permettant le rendu du manoir en console
    /// Rien d'interessant ici.
    /// </summary>
    public class MansionConsoleRender
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
            foreach (var room in _environment.Rooms)
            {
                if (i % MansionEnv.SIZE == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine(new string('-', MansionEnv.SIZE * 6));
                }
                Console.Write(" {0}{1}{2} |", room.dirt ? "P" : " ", room.diamond ? "D" : " ", room.pos.Equals(_environment.PositionAgent) ? "A" : " ");
                i++;
            }

            // Dessin du HUD
            Console.WriteLine();

            Console.WriteLine(_environment.PerformancMeasureDetails().ToString());
            Console.WriteLine(_agent.ToString());
 
            Console.WriteLine();
        }
    }
}