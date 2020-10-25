import ctypes
a = ctypes.cdll.LoadLibrary('bin/Debug/netstandard2.0/connector.dll')
print(a.add(3, 5))