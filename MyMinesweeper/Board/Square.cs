using System.Text;

namespace MyMinesweeper
{

    /// <summary>
    /// Klassen representerar en ruta på spelplanen
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Används för test. Visar vilken position rutan har på spelplanen
        /// </summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// Används när jag räknar ut antalet grannar som är minor
        /// </summary>
        public int MineAsNeighbourCount { get; set; } = 0;

        /// <summary>
        /// Visas data i rutan eller ej
        /// </summary>
        public bool IsOpenSquare { get; set; } = false;

        /// <summary>
        /// Data i rutan
        /// </summary>
        public Cell SquareCell { get; set; } = Cell.CELL_INVALID;


        /// <summary>
        /// Konstruktor
        /// </summary>
        public Square()
        {
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="cell">Data i rutan</param>
        /// <param name="coordinate">Positionen på spelplanen. Använd vid test</param>
        public Square(Cell cell, string coordinate)
        {
            SquareCell = cell;
            Coordinate = coordinate;
        }

        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder("Square");
            strBuild.Append($" {SquareCell} ({Coordinate})");
            return strBuild.ToString();
        }
    }
}
