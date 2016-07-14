using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace CPUAndMemoryMonitorWithSpeech
{
    class Program
    {
        static void Main(string[] args)
        {
            #region My Performance Counters
            // This will greet the user in the default voice
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.Speak("Welcome to the CPU and Memory Monitor");

            // This will pull the current CPU load percentage
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            
            // This will pull the current available memory in Megabytes
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");

            // This will pull the current uptime of the system (in seconds)
            PerformanceCounter perfUptimeCount = new PerformanceCounter("System", "System Up Time");
            #endregion

            // Infinite While Loop
            while (true)
            {
                // Get the current performance counter values
                int currentCpuPercentage = (int)perfCpuCount.NextValue();
                int currentAvailableMemory = (int)perfMemCount.NextValue();

                // Every 1 second print the CPU load percentage to the screen
                Console.WriteLine("CPU Load        : {0}%", currentCpuPercentage);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);

                // Tells us when the CPU is above 80% usage
                if(currentCpuPercentage > 80)
                {
                    if (currentCpuPercentage == 100)
                    {
                        string cpuLoadVocalMessage = String.Format("WARNING: CPU is at 100 percent usage", currentCpuPercentage);
                        synth.Speak(cpuLoadVocalMessage);
                    }
                    else
                    {
                        string cpuLoadVocalMessage = String.Format("The current CPU load is {0} percent", currentCpuPercentage);
                        synth.Speak(cpuLoadVocalMessage);
                    }
                }

                // Tells us when memory is below one gigabyte
                if (currentAvailableMemory < 1024)
                {         
                    string memAvailableVocalMessage = String.Format("You currently have {0} megabytes of memory available", currentAvailableMemory);
                    synth.Speak(memAvailableVocalMessage);
                }

                Thread.Sleep(1000);
            } // End of loop
        }
    }
}
