using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrenDB.ReflectionVisualizer;

namespace MyTestConsole
{
    internal class Program
    {
        /// <summary>
        /// Test harness for ReflectionVisualizer
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var myList = new List<MyDTO>() {
                new MyDTO("Paul", "Round"),
                new MyDTO("Fred", "Square"),
                new MyDTO(null, "Cake")
            };
            DebuggerVisualizer.TestShowVisualizer(myList);
        }
    }

    internal class MyDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public MyDTO(string name, string description)
        {
            Name = name;
            Description = description;
            DateTime = DateTime.Now;
        }
    }
}
