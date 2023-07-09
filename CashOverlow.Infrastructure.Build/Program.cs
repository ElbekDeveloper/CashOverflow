// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by CashOverflow Team
// --------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace CashOverflow.Infrastructure.Build
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var aDotNetClient = new ADotNetClient();

            var githubPipeline = new GithubPipeline
            {
                Name = "Build & Test CashOverflow",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "master" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "master" }
                    }
                },

                Jobs = new Dictionary<string, Job>
      {
          {
              "build",
              new Job
              {
                  RunsOn = BuildMachines.WindowsLatest,

                  Steps = new List<GithubTask>
                  {
                      new CheckoutTaskV2
                      {
                          Name = "Check out"
                      },

                      new SetupDotNetTaskV1
                      {
                          Name = "Setup .Net",

                          TargetDotNetVersion = new TargetDotNetVersion
                          {
                              DotNetVersion = "8.0.100-preview.5.23303.2",
                              IncludePrerelease = true
                          }
                      },

                      new RestoreTask
                      {
                          Name = "Restore"
                      },

                      new DotNetBuildTask
                      {
                          Name = "Build"
                      },

                      new TestTask
                      {
                          Name = "Test"
                      }
                  }
              }
          }
      }
            };

            string buildScriptPath = "../../../../.github/workflows/build.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            aDotNetClient.SerializeAndWriteToFile(githubPipeline, path: buildScriptPath);
        }
    }
}