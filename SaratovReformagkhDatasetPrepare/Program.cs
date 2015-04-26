using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yandex;

namespace SaratovReformagkhDatasetPrepare
{
    class Program
    {
        private const char CSV_DELIMITER = ',';

        static void Main(string[] args)
        {
            string inputFileName = ConfigurationSettings.AppSettings["InputFile"];

            if (String.IsNullOrEmpty(inputFileName))
            {
                Console.WriteLine("Please specify input file in application config");
                return;
            }

            ProcessInputFile(inputFileName);

            Console.ReadLine();
        }

        private static void ProcessInputFile(string inputFileName)
        {
            string[] lines = File.ReadAllLines(inputFileName, Encoding.UTF8);
            List<string> lineList = new List<string>(lines);

            string outputFileName = ConfigurationSettings.AppSettings["OutputFile"];

            if (String.IsNullOrEmpty(outputFileName))
            {
                Console.WriteLine("Please specify outinput file in application config");
                return;
            }

            using (var writer = new StreamWriter(outputFileName))
            {
                Console.WriteLine("Start file processing...");
                int count = 0;
                Parallel.ForEach(lineList, line =>
                {
                   ProcessLine(line, writer);
                });
                Console.WriteLine("Input file is processed. {0} lines", lineList.Count);
            }
        }

        private static void ProcessLine(string line, StreamWriter writer)
        {
            string[] chunks = line.Split(CSV_DELIMITER);

            if (IsLineValid(chunks))
            {
                string adress = chunks[6].Replace("г. ", "");
                string year = chunks[7];

                GeoObjectCollection results = YandexGeocoder.Geocode(chunks[6], 1, LangType.ru_RU);
                if (results.Count() > 0)
                {
                    writer.WriteLine("{0},{1},{2},{3}", adress, year, results.First().Point.Lat, results.First().Point.Long);
                }
            }
        }

        private static bool IsLineValid(string[] chunks)
        {
            int year;
            return int.TryParse(chunks[7], out year);
        }
    }
}
