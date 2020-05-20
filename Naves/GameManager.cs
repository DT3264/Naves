using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Naves
{
    public class GameManager
    {
        /// <summary>
        /// The posible states of the game
        /// </summary>
        enum GameState
        {
            Stopped,
            Paused,
            Running
        }
        /// <summary>
        /// Lista de enemigos actuales
        /// </summary>
        List<Enemy> enemyList = new List<Enemy>();
        /// <summary>
        /// El jugador actual
        /// </summary>
        Player player;
        /// <summary>
        /// Las diferentes areas a spawnear enemigos
        /// </summary>
        int[] spawnPositions;
        /// <summary>
        /// El puntaje actual
        /// </summary>
        int score;
        /// <summary>
        /// La velocidad del siguiente enemigo
        /// </summary>
        int speed = 10;
        /// <summary>
        /// El estado actual
        /// </summary>
        GameState state = GameState.Stopped;
        /// <summary>
        /// The board where the game will be played
        /// </summary>
        GameBoard gameBoard;

        /// <summary>
        /// Al cargar, se muestra el mensaje de inicio, 
        /// se cargan las posiciones válidas para spawnear dado el tamaño de la pantalla
        /// se crea un nuevo jugador y se añade a la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public GameManager(GameBoard gameBoard)
        {
            this.gameBoard = gameBoard;
            spawnPositions = new int[]
            {
                0,
                ((gameBoard.Height/4)*1),
                ((gameBoard.Height/4)*2),
                ((gameBoard.Height/4)*3),
            };
            player = new Player(gameBoard.Bounds);
            gameBoard.Controls.Add(player.playerBox);
            ShowInfo("Presione F10 para iniciar o esc para salir");
        }

        /// <summary>
        /// Maneja los estados en caso de que se presione ESC, F10 o F11
        /// </summary>
        /// <param name="e"></param>
        public void HandleKey(KeyEventArgs e, bool isPressed)
        {
            player.HandleKey(e, isPressed);
            if (!isPressed) return;
            if (e.KeyValue == 27)
            {
                if (state == GameState.Stopped || state == GameState.Paused)
                {
                    gameBoard.Close();
                }
            }
            else if (e.KeyValue == 121)
            {
                if (state == GameState.Stopped)
                {
                    StartGame();
                }
                else if (state == GameState.Paused)
                {
                    ResumeGame();
                }
                else if (state == GameState.Running)
                {
                    PauseGame();
                }
            }
            else if (e.KeyValue == 122)
            {
                StartGame();
            }
        }

        /// <summary>
        /// Spawnea un enemigo con posición inicial aleatoria
        /// Además de añadirse al mapa y calcular cuando spawnear otro enemigo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleEnemySpawnerTick()
        {
            int spawnIndex = new Random().Next(0, 10000) % 4;

            Enemy enemy = new Enemy(speed++, spawnPositions[spawnIndex], gameBoard.Width - 128);
            gameBoard.Controls.Add(enemy.enemyBox);
            enemyList.Add(enemy);

            int nextSpawn = new Random().Next(2000, 3000);
            gameBoard.enemySpawnerTimer.Interval = nextSpawn;
        }

        /// <summary>
        /// Al hacer tick, se mueve al personaje de ser necesario y se mueven los enemigos. 
        /// Si alguno colisiona con el jugador, el juego termina.
        /// Además, para cada enemigo, si está fuera de cámara, se elimina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleControlTick()
        {
            player.Move();
            //Mueve, checa colisiones
            enemyList.ForEach((enemy) =>
            {
                enemy.Move();
                if (enemy.enemyBox.Bounds.IntersectsWith(player.playerBox.Bounds))
                {
                    StopGame();
                }
            });

            //Checa si OOB y desinstancia
            List<Enemy> lastStateList = new List<Enemy>(enemyList);
            enemyList.ForEach((enemy) =>
            {
                if (enemy.toDelete)
                {
                    gameBoard.Controls.Remove(enemy.enemyBox);
                    enemy.enemyBox = null;
                    lastStateList.Remove(enemy);
                    UpdateScore();
                }
            });
            enemyList = lastStateList;
        }

        /// <summary>
        /// Actualiza el puntaje en pantalla
        /// </summary>
        void UpdateScore()
        {
            score++;
            gameBoard.lblScore.Text = String.Format("Puntaje: {0}", score);
        }

        /// <summary>
        /// Reinicia el puntaje en pantalla
        /// </summary>
        void ResetScore()
        {
            score = 0;
            gameBoard.lblScore.Text = String.Format("Puntaje: {0}", score);
        }

        /// <summary>
        /// Detiene los timers, muestra la información de Game Over
        /// y asigna el estado a Detenido
        /// </summary>
        void StopGame()
        {
            ShowInfo("Juego terminado, presione F10 para jugar o esc para salir");
            gameBoard.enemySpawnerTimer.Enabled = false;
            gameBoard.controlTimer.Enabled = false;
            state = GameState.Stopped;
        }

        /// <summary>
        /// Pausa los timers, muestra la info de pausa
        /// y asigna el estado a Pausado
        /// </summary>
        void PauseGame()
        {
            ShowInfo("Presione F10 para resumir o esc para salir");
            gameBoard.enemySpawnerTimer.Enabled = false;
            gameBoard.controlTimer.Enabled = false;
            state = GameState.Paused;
        }

        /// <summary>
        /// Resume los timers, oculta la info
        /// y ajusta el estado del juego a Corriendo
        /// </summary>
        void ResumeGame()
        {
            HideInfo();
            gameBoard.enemySpawnerTimer.Enabled = true;
            gameBoard.controlTimer.Enabled = true;
            state = GameState.Running;
        }

        /// <summary>
        /// Oculta la información, reinicia el score
        /// Reubica la nave del jugador en su pos original
        /// Quita de pantalla a todos los enemigos y reinicia la lista de enemigos
        /// Activa los timers
        /// y pone el estado global como Corriendo
        /// </summary>
        void StartGame()
        {
            HideInfo();
            ResetScore();
            player.ResetShip();
            enemyList.ForEach((e) => gameBoard.Controls.Remove(e.enemyBox));
            enemyList = new List<Enemy>();
            gameBoard.controlTimer.Enabled = true;
            gameBoard.enemySpawnerTimer.Enabled = true;
            state = GameState.Running;
        }

        /// <summary>
        /// Muestra el label de información con un mensaje
        /// </summary>
        /// <param name="message"> El mensaje a mostrar</param>
        void ShowInfo(String message)
        {
            gameBoard.lblInfo.Text = message;
            gameBoard.lblInfo.Visible = true;
        }

        /// <summary>
        /// Oculta el label de información
        /// </summary>
        void HideInfo()
        {
            gameBoard.lblInfo.Visible = false;
        }
    }
}
