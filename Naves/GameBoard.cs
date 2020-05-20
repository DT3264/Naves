using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Naves
{
    public partial class GameBoard : Form
    {
        public GameBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// El manager del juego
        /// </summary>
        GameManager gameManager;

        /// <summary>
        /// Al presionar una tecla llama al manejador de estados y al manejador de movimientos del jugador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            gameManager.HandleKey(e, true);
        }
        /// <summary>
        /// Al liberar una tecla llama al manejador de movimientos del jugador
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            gameManager.HandleKey(e, false);
        }

        /// <summary>
        /// Llama al tick de controles del gameManager 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlTimer_Tick(object sender, EventArgs e)
        {
            gameManager.HandleControlTick();
        }

        /// <summary>
        /// Llama al tick spawneador de enemigos del gameManager
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enemySpawner_Tick(object sender, EventArgs e)
        {
            gameManager.HandleEnemySpawnerTick();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameManager = new GameManager(this);
        }
    }


}
