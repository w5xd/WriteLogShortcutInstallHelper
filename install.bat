@rem This is where the user starts--run this install.bat.
@rem install.bat must be in the same directory with these other files:
@rem           ASSEMBLYNAME.dll  <-the assembly to be installed
@rem           ConfigureShortcuts.exe <- built from this project
@rem           CopyShortcutAssembly.exe <- ditto
@rem           CopyShortcutAssembly.exe.config <- ditto
@rem for ASSEMBLYNAME substitute the name of the dll without the .dll extension
@rem for CLASSNAME substitute the name of the class in the assembly
@rem These helpers assume that CLASSNAME is in namespace ASSEMBLYNAME
ConfigureShortcuts ASSEMBLYNAME CLASSNAME /install
