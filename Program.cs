﻿using API_C_Sharp.LSharp;
using API_C_Sharp.Model;
using API_C_Sharp.Route;

namespace API_C_Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Data dataInstance = new Data();

            Server app = new Server(8080);

            new UserRoutes(app);
            new PostRoutes(app);
            new CommentRoutes(app);
            new FriendshipRoutes(app);
            new ChatRoutes(app);

            app.Start(dataInstance);

            Console.WriteLine("Servidor iniciado e disponivel em http://localhost:8080/ ");
            Console.WriteLine("Digite 'stop' para encerrar o servidor.");
            while (true)
            {
                string userInput = Console.ReadLine();
                if (userInput.ToLower() == "stop")
                {
                    app.Close();
                    Console.WriteLine("Encerrar encerrado.");
                    break;
                }
            }
        }
    }
}