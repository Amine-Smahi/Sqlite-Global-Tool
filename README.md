# Sqlite-Global-Tool
SQLite .NET Core global tool project that provides a simple command-line program that allows the user to manually enter and execute SQL statements with or without showing query results against an SQLite database.
## What is .NET Core global tools
A .NET Core global tool is a special NuGet package that contains a console application. When installing a tool, .NET Core CLI will download and make your console tool available as a new command.

## How to install it
The steps are very easy you only have to
* Check if .NET Core sdk version 2.1 installed on your system, you can download it from [Here](https://www.microsoft.com/net/download/dotnet-core/2.1) then check if the instalation has gone correctly by typing
      
      user$ dotnet --version
      user$ 2.1.402
* A single command allows you to download and install Goli
  
      user$ dotnet tool install --global sqlite-global-tool --version 1.0.0 
* Add the tool to the enironement variable 

Windows:

      setx PATH "$env:PATH;$env:USERPROFILE/.dotnet/tools"
    
Linux/macOS:

    echo "export PATH=\"\$PATH:\$HOME/.dotnet/tools\"" >> ~/.bash_profile
* Finaly run

      user$ sqlite-tool --h
* Support me by making a <img style="margin-bottom: -20px;" src="https://user-images.githubusercontent.com/24621701/44811262-193e6e00-abcc-11e8-8e61-e52d8c78d5c9.png" /> for the repo and thank you :D , If you want to contribute to the project and make it better, your help is very welcome. 
## Screenshot
![image](https://user-images.githubusercontent.com/24621701/46575019-41fc1500-c97b-11e8-9688-52bd94895b1b.png)

## License 
This project is under MIT License
