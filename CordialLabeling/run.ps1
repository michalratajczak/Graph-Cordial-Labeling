$ModelCode = "int: n;
int: k;

set of int: N = 1..n; 
set of int: K = 1..k;  

set of int: LABEL = 0..1;

array[N] of var LABEL: vertices; 
array[K,1..2] of N: edges; 

constraint abs(n - sum(i in N)(vertices[i]) * 2) <= 1;
constraint abs(k - sum(j in K)(abs(vertices[edges[j,1]] - vertices[edges[j,2]])) * 2) <= 1;

solve satisfy;

output [show(vertices)];"

Start-Process -FilePath "dotnet" -ArgumentList "publish -p:PublishSingleFile=true -r win-x64 -c Release -o `"..\App`"" -WindowStyle Hidden -Wait

'Graphviz', 'MiniZinc', 'Model', 'Result', 'Temp' | ForEach {New-Item -ItemType Directory -Path "..\App\$_"}

$ModelCode | Out-File -FilePath "..\App\Model\CordialLabeling.mzn" -Encoding Ascii
