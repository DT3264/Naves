using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naves
{
    public class Player
    {
        /// <summary>
        /// Los movimientos de la nave
        /// </summary>
        enum Movements
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3
        }

        /// <summary>
        /// La imagen de la nave
        /// </summary>
        public PictureBox playerBox;
        /// <summary>
        /// La posición de la nave
        /// </summary>
        Point playerLocation;
        /// <summary>
        /// La velocidad de la nave
        /// </summary>
        int step = 20;
        /// <summary>
        /// La región de la ventana
        /// </summary>
        Rectangle bounds;
        /// <summary>
        /// Si el jugador sigue vivo
        /// </summary>
        public bool isAlive;
        /// <summary>
        /// Booleanos que indican si debe moverse en esa dirección en el orden
        /// izquierda, derecha, arriba y abajo
        /// </summary>
        bool[] shouldMove = new bool[] { false, false, false, false };

        public Player(Rectangle bounds)
        {
            isAlive = true;
            this.bounds = bounds;
            playerBox = new PictureBox();
            playerBox.Image = Properties.Resources.ship;
            playerBox.Width = playerBox.Image.Width;
            playerBox.Height = playerBox.Image.Height;
            ResetShip();
        }

        /// <summary>
        /// Reinicia la nave a la altura media de la pantalla lo más a la izquierda
        /// </summary>
        public void ResetShip()
        {
            isAlive = true;
            playerLocation.Y = (bounds.Height / 2) - 60;
            playerLocation.X = 20;
            playerBox.Location = playerLocation;
        }

        /// <summary>
        /// Dada una tecla, actualiza el valor respectivo en el arreglo de movimientos al recibido.
        /// </summary>
        /// <param name="e">La tecla presionada</param>
        /// <param name="toMove">True si se activa esa dirección, false si se desactiva</param>
        public void HandleKey(KeyEventArgs e, bool toMove)
        {
            if (e.KeyValue == 38 || e.KeyValue == 87)
            {
                shouldMove[(int)Movements.Up] = toMove;
            }
            if (e.KeyValue == 40 || e.KeyValue == 83)
            {
                shouldMove[(int)Movements.Down] = toMove;
            }
            if (e.KeyValue == 37 || e.KeyValue == 65)
            {
                shouldMove[(int)Movements.Left] = toMove;
            }
            if (e.KeyValue == 39 || e.KeyValue == 68)
            {
                shouldMove[(int)Movements.Right] = toMove;
            }
        }

        /// <summary>
        /// Actualiza la ubiación de la nave a como se requiera y se asigna después a la imagen de la nave
        /// </summary>
        public void Move()
        {
            if (shouldMove[(int)Movements.Up] && !OutOfBounds(playerLocation.X, playerLocation.Y - step))
            {
                playerLocation.Y -= step;
            }
            if (shouldMove[(int)Movements.Down] && !OutOfBounds(playerLocation.X, playerLocation.Y + step))
            {
                playerLocation.Y += step;
            }
            if (shouldMove[(int)Movements.Left] && !OutOfBounds(playerLocation.X - step, playerLocation.Y))
            {
                playerLocation.X -= step;
            }
            if (shouldMove[(int)Movements.Right] && !OutOfBounds(playerLocation.X + step, playerLocation.Y))
            {
                playerLocation.X += step;
            }
            playerBox.Location = playerLocation;
        }

        /// <summary>
        /// Función que dice si el jugador está en una zona no válida de la pantalla
        /// </summary>
        /// <param name="x">El punto en X de la nave</param>
        /// <param name="y">El punto Y de la nave</param>
        /// <returns>Si la nave está fuera de pantalla</returns>
        bool OutOfBounds(int x, int y)
        {
            return (x < 0 || y < 0 || x + 128 > bounds.Width || y + 128 > bounds.Height);
        }
    }
}
