using Microsoft.VisualStudio.DebuggerVisualizers;
using WarrenDB.ReflectionVisualizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(DebuggerVisualizer),
    typeof(DebuggerVisualizerSource),
    Target = typeof(IEnumerable<>),
    Description = "Dump to clipboard for Excel")]
namespace WarrenDB.ReflectionVisualizer
{
    /// <summary>
    /// ReflectionVisualizer is a quick and dirty way to dump the ToStrings of every parameter on an IEnumerable of objects to a TSV in the clipboard.
    /// Build and place the resulting ReflectionVisualizer.dll in MyDocuments\%VisualStudioVersion%\Visualizers
    /// Dump an array to the clipboard using the magnifying glass dropdown in the debug inspector and selecting "Dump to clipboard for Excel"
    /// </summary>
    public class DebuggerVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            try
            {
                using (var reader = new StreamReader(objectProvider.GetData()))
                {
                    var data = reader.ReadToEnd();
                    var thread = new Thread(() => Clipboard.SetText(data));
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();
                    MessageBox.Show("Array copied to clipboard", "Dump to clipboard success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } catch (Exception ex) {

                MessageBox.Show($"Something went wrong: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DebuggerVisualizer), typeof(DebuggerVisualizerSource));
            visualizerHost.ShowVisualizer();
        }
    }

    /// <summary>
    /// This class runs in the thread space of the program being debugged.
    /// Because this is in the same project as the Debugger side class it can only be used with .Net Framework projects.
    /// </summary>
    public class DebuggerVisualizerSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var data = target as IEnumerable<object>;
            var writer = new StreamWriter(outgoingData);

            if (data != null)
            {
                var sb = new StringBuilder();
                var genTypeProperties = data.GetType().GetGenericArguments()[0].GetProperties().ToList();
                sb.Append(String.Join("\t", genTypeProperties.Select(p => p.Name)));
                foreach (var obj in data)
                {
                    sb.AppendLine();
                    sb.Append(String.Join("\t", genTypeProperties.Select(x => x.GetValue(obj)?.ToString()).ToArray()));
                }

                writer.WriteLine(sb.ToString());
                writer.Flush();
            }
        }
    }
}
