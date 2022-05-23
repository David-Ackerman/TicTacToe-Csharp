using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.src
{
    public class TicTacToeGame
    {
        int turn = 0;
        bool game = false;
        string[] boardMatrix = new string[9];

        static public void Main(String[] args)
        {
            TicTacToeGame game = new();
            game.Init();
        }

        public void Init()
        {
            // Inicia as variaveis com o valor padrão
            turn = 0;
            game = true;

            for (int i = 0; i < 9; i++)
            {
                this.boardMatrix[i] = " ";
            }


            // Cria as threads dos metodos usados no jogo da velha (thread do jogador A, jogador B, Campo e verificar vencedor)
            Thread board = new(new ThreadStart(Board));
            Thread playerA = new(new ThreadStart(PlayerA));
            Thread playerB = new(new ThreadStart(PlayerB));
            Thread checkWinner = new(new ThreadStart(CheckWinner));

            //Inicia as threads
            board.Start();
            playerA.Start();
            playerB.Start();
            checkWinner.Start();
        }
        private void PlayerA()
        {
            // Lopping para a thread ficar executando enquanto o partida não termina
            while (game)
            {
                // verifica se é a vez do jogador 1
                if (turn == 0)
                {
                    // Verifica se o jogador 1 fez sua jogada para prosseguir com as etapas do jogo
                    if (Play(1))
                    {                        
                        turn = 1;
                    };
                }else
                {
                    Thread.Sleep(500);

                }
            }
        }
        private void PlayerB()
        {
            // Lopping para a thread ficar executando enquanto o partida não termina

            while (game)
            {
                // verifica se é a vez do jogador 2
                if (turn == 3)
                {
                    // Verifica se o jogador 1 fez sua jogada para prosseguir com as etapas do jogo
                    if (Play(2))
                    {
                        turn = 4;
                    };
                }
                else
                {
                    Thread.Sleep(500);

                }
            }
        }

        // Método para checar se houve um vencedor a cada troca de vez
        public void CheckWinner()
        {
            // Lopping para a thread ficar executando enquanto o partida não termina
            while (game)
            {
                // Verifica se é o turno de checagem do jogador 1
                if (turn == 2)
                {
                    // Verifica as jogadas do jogador 1, se ele venceu nessa jogada a paritda é finalizada, se não vencer apenas passa o turno
                    if (this.VerifyPlay("x"))
                    {
                        game = false;
                        Console.WriteLine("Jogador 1 ganhou");
                    }
                    else
                    {
                        turn = 3;

                    }

                }
                // Verifica se é o turno de checagem do jogador 2
                else if (turn == 5)
                {
                    //Verifica as jogadas do jogador 2, se ele venceu nessa jogada a paritda é finalizada, se não vencer apenas passa o turno
                    if (this.VerifyPlay("0"))
                    {
                        game = false;
                        Console.WriteLine("Jogador 2 ganhou");
                    }
                    else
                    {
                        turn = 0;
                    }
                }
                else
                {
                    Thread.Sleep(500);
                }


            }
        }

        private void Board()
        {
            // Lopping para a thread ficar executando enquanto o partida não termina
            while (game)
            {
                // Verifica se a partida está em turno de atualização do campo
                if (turn == 1 || turn == 4)
                {
                    Console.Clear(); // Limpa o console, pra evitar puluição visual
                    string board = "| " ; // Inicia a string que será usada para representar o campo

                    // Laço for usado para verificar cada item do array de jogadas
                    for (int i = 0; i < 9; i++)
                    {
                        board += this.boardMatrix[i] + " | "; // A string do campo concatena com o valor do array de jogadas (x para jogador 1, espaço para nenhum jogada nesse campo, e 0 para jogador 2) e adiciona a | para delimitar o campo;

                        // Verificação para fazer a quebra de linha a cada após 3 items em cada linha
                        if (i == 2 || i == 5)
                        {
                            board += "\n| ";
                        }
                    }

                    // Imprime o campo com as jogadas feitas, e passa o turno;
                    Console.WriteLine(board);
                    turn++; 

                }
                Thread.Sleep(500);
            }
        }

        // Metódo da jogada, faz a leitura do valor digitado pelo jogador e adiciona ao array de jogadas
        private bool Play(int player)
        {

            Console.WriteLine("Digite a posição do tabuleiro (1 a 9): ");
            int pos = readInt(); // Recebe o Valor digitado pelo jogador através do método readInt;
            
            // Verifica se a posição escolhida pelo jogador está livre e se a posição está entre 1 e 9, se não for uma posição valida será solicitada uma nova escolha de posição
            while (pos < 1 || pos > 9 || boardMatrix[pos - 1] != " ")
            {
                Console.WriteLine("Posição inválida! Digite novamente: ");
                pos = readInt();
            }

            
            string character = player == 1 ? "x" : "0"; // Verifica qual é o jogador para definir qual caracter usar na jogada dele (x ou 0)
            boardMatrix[pos - 1] = character; // Adiciona o caracter do jogador no array de jogadas, (pos - 1 porque o index do array começa em 0, e as jogadas são de 1 a 9, precisa subtrair 1)
            return true; // Retorno para indicar que a jogada foi feita
        }

        // Lê do valor digitado pelo jogador e tranforma em um valor inteiro, tratamento de erro para não quebrar a aplicação
        private int readInt()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch
            {
                return 0;
            }
        }

        // Metódo para verificar se o jogador venceu, recebe o caracter do jogador como parâmetro
        private bool VerifyPlay(string playerChar)
        {
            List<int> plays = new(); //Inicia uma lista de tamanhoi dinamico que vai ser preenchida com as jogadas de apenas um jogador

            // Percorre o array de jogadas e adiciona as jogadas do jogador para a nova lista
            for (int i = 0; i < 9; i++)
            {
                if (this.boardMatrix[i] == playerChar)
                {
                    plays.Add(i);
                }
            }
            if (plays.Count < 3) return false; // menos de 3 jogadas do jogador não tem vitoria

            if (plays.Contains(0) && plays.Contains(1) && plays.Contains(2)) return true; // Verifica se ganhou marcando a primeira linha
            if (plays.Contains(3) && plays.Contains(4) && plays.Contains(5)) return true; // Verifica se ganhou marcando a segunda linha
            if (plays.Contains(6) && plays.Contains(7) && plays.Contains(8)) return true; // Verifica se ganhou marcando a terceira linha
            if (plays.Contains(0) && plays.Contains(3) && plays.Contains(6)) return true; // Verifica se ganhou marcando a primeira coluna
            if (plays.Contains(1) && plays.Contains(4) && plays.Contains(7)) return true; // Verifica se ganhou marcando a segunda coluna
            if (plays.Contains(2) && plays.Contains(5) && plays.Contains(8)) return true; // Verifica se ganhou marcando a terceira coluna
            if (plays.Contains(0) && plays.Contains(4) && plays.Contains(8)) return true; // Verifica se ganhou marcando a diagonal esquerda superior até direita inferior
            if (plays.Contains(2) && plays.Contains(4) && plays.Contains(6)) return true; // Verifica se ganhou marcando a diagonal esquerda inferior até direita superio

            // Verifica se ainda tem jogadas disponiveis, se não tiver ocorreu um empate
            if(!boardMatrix.Contains(" "))
            {
                game = false;
                Console.WriteLine("Empatou");
            }
            return false;

        }


    }
}