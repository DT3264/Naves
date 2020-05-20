using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naves
{
    public class Enemy
    {
        /// <summary>
        /// La imagen de la nave
        /// </summary>
        public PictureBox enemyBox;
        /// <summary>
        /// La posición de la nave
        /// </summary>
        Point enemyLocation;
        /// <summary>
        /// La velocidad de la nave
        /// </summary>
        int step;
        /// <summary>
        /// Si ya ha desaparecido de la pantalla
        /// </summary>
        public bool toDelete = false;
        public Enemy(int velocity, int startHeight, int startWidth)
        {
            step = velocity;

            enemyBox = new PictureBox();
            enemyBox.Image = Properties.Resources.enemy;
            enemyBox.Width = enemyBox.Image.Width;
            enemyBox.Height = enemyBox.Image.Height;

            enemyLocation = enemyBox.Location;
            enemyLocation.Y = startHeight;
            enemyLocation.X = startWidth;
            enemyBox.Location = enemyLocation;
        }

        /// <summary>
        /// Mueve el enemigo
        /// y si se pasa de cámara se marca para eliminarse
        /// </summary>
        public void Move()
        {
            enemyLocation.X -= step;
            enemyBox.Location = enemyLocation;
            if (enemyLocation.X + 128 < 0)
            {
                toDelete = true;
            }
        }
    }
}
