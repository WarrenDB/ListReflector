# ListReflector
Custom Visual Studio debugger inspector for dumping arrays to clipboard formatted to paste directly to Excel

## Installation
Copy ReflectionVisualizer.dll to `{User}\Documents\Visual Studio {Year}\Visualizers` and new "Dump to clipboard for Excel" option will appear under the magnifying glass inspector when inspecting any IEnumerable<> next time Debug mode is started.

![IDE screenshot showing new visualizer option](https://i.imgur.com/nkEP8Nu.png)
![Excel screenshot showing exported data in table](https://i.imgur.com/P1t5yNI.png)

### Notices
- Currently only works with .Net Framework apps. .Net Core support coming soon.
