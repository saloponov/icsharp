﻿

using System.IO;
using System.Threading;
using Common.Logging;
using Common.Logging.Simple;
using iCSharp.Kernel;

namespace iCSharp
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using iCSharp.Messages;

    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine(File.ReadAllText("C:\Users\Zohaib\.ipython\profile_icsharp\security\kernel-7064.json"));
            PrintAllArgs(args);
            if (args.Length <= 0)
            {
                Console.WriteLine("Arguments not provided");
                return;
            }

            ConnectionInformation connectionInformation = GetConnectionInformation(args[0]);

            KernelCreator creator = new KernelCreator(connectionInformation);

            IServer heartBeatServer = creator.HeartBeatServer;
            heartBeatServer.Start();

            IServer shellServer = creator.ShellServer;
            shellServer.Start();

            shellServer.GetWaitEvent().Wait();
            heartBeatServer.GetWaitEvent().Wait();
            //Thread.Sleep(60000);
        }

        private static ConnectionInformation GetConnectionInformation(string filename)
        {
            ILog logger = new ConsoleOutLogger("kernel.log", LogLevel.All, true, true, false, "yyyy/MM/dd HH:mm:ss:fff");

            logger.Info(string.Format("Opening file {0}", filename));
            string fileContent = File.ReadAllText(@filename);
            logger.Debug(fileContent);

            ConnectionInformation connectionInformation =
                JsonConvert.DeserializeObject<ConnectionInformation>(fileContent);

            return connectionInformation;
        }

        private static void PrintAllArgs(string[] args)
        {
            ILog logger = new ConsoleOutLogger("kernel.log", LogLevel.All, true, true, false, "yyyy/MM/dd HH:mm:ss:fff");
            logger.Debug("Hello2");
            foreach (string s in args)
            {
                logger.Debug(s);
            }
        }
    }
}