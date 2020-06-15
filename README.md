# Graph-Cordial-Labeling
This project has been created as a part of Artificial Intelligence course in Poznan University of Technology. The main goal of this task was to create an algorithm using constraint programming to check possibility of cordial labeling any graph given as input and return labeled graph if it's possible. 
## How does Cordial Labeling work?
A cordial labeled graph has to have assigned labels from the set {0, 1} for each of the vertices. For each edge a label is equal to the absolute value of the incident vertex label difference. Conditions for cordial labeling the graph are met if the difference in the number of vertices and edges labeled "1" and "0" differ at most by 1.
## Application requirements
### Required tools:
- [MiniZinc](https://www.minizinc.org/) (2.4.3 or higher) 
- [Graphviz](https://graphviz.org/download/) (2.44.0 or higher)
- [NET Core SDK and Runtime](https://dotnet.microsoft.com/download) (3.1 or higher)
### How to run this app?
 1. Git clone this repository.
 2. Run "/CordialLabeling/run.ps1" script.
 3. Copy the contents of the directories in which MiniZinc and Graphviz applications have been installed to the corresponding directories in the "../App" directory.
