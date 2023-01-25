using Azure.Storage.Queues;

using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataImport.AzureFunctions.Extensions
{
    public static class Extensions
    {
        public const string TransformLoadFolder = "TransformLoadTool";
        public const string TransformLoadExe = "DataImport.Server.TransformLoad.exe";

        public static QueueClient GetQueue()
        {
            var storageConnectionTransformLoadQueue = Environment.GetEnvironmentVariable("ConnectionStrings__storageConnection");
            var dataImportTransformLoadQueueName = Environment.GetEnvironmentVariable("EdGraph__storageConnection__QueueName"); //"DataImport-TransformLoad-Queue"
            QueueClient queueClient = new QueueClient(storageConnectionTransformLoadQueue, dataImportTransformLoadQueueName, new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 });
            // Instantiate a QueueClient to create and interact with the queue        
            queueClient.CreateIfNotExists();
            return queueClient;
        }

        public static Process GetTransformLoadProcess(string dataImportTransformLoadInstanceName)
        {
            //string? pathBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string? pathBase = "/$HOME";
            ///tmp/TransformLoadTool
            var toolPath = Path.Combine(pathBase, TransformLoadFolder);
            var toolExe = Path.Combine(toolPath, TransformLoadExe);


            ProcessStartInfo processStartInfo = new()
            {
                WorkingDirectory = toolPath,
                FileName = toolExe,//toolExe;   
                //Arguments = "";

                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };


            //var systemRootEnv = processStartInfo.EnvironmentVariables["SYSTEMROOT"];
            //NOTE: Patch needed to control various ENV from inheriting
            //NOTE: Breaking in K8
            //processStartInfo.Environment.Clear();
            //processStartInfo.EnvironmentVariables["SYSTEMROOT"] = systemRootEnv;

            processStartInfo.EnvironmentVariables["DOTNET_ENVIRONMENT"] =  Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
            processStartInfo.EnvironmentVariables["ASPNETCORE_ENVIRONMENT"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            processStartInfo.EnvironmentVariables["AppSettings__DatabaseEngine"] = Environment.GetEnvironmentVariable("AppSettings__DatabaseEngine");
            processStartInfo.EnvironmentVariables["ConnectionStrings__storageConnection"] = Environment.GetEnvironmentVariable("ConnectionStrings__storageConnection");// "UseDevelopmentStorage=true";
            processStartInfo.EnvironmentVariables["ConnectionStrings__defaultConnection"] = DbExtensions.SubstituteDataImportInstance(dataImportTransformLoadInstanceName);

            //NOTE: Patch needed if ENV is inherited
            //processStartInfo.EnvironmentVariables["DOTNET_STARTUP_HOOKS"] = "";

            var process = new Process()
            {
                StartInfo = processStartInfo
            };

            return process;
        }


    }
}

