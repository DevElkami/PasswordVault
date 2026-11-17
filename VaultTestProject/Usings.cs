global using Microsoft.VisualStudio.TestTools.UnitTesting;

// Every test works in its own temporary directory, so they can run in parallel.
[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]
