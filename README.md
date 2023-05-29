# File Management WPF windows application

Project walkthrough: https://youtu.be/0t6L8D8Iv48

## Project set-up
1. To run this project, open the windowsApplication folder in Visual Studio
2. go to the `MainWindow.xaml` and click run application
3. To change the path that he system is monitoring (currently hardcoded), go to `MainWindow.xaml.cs` and change the private string currentPath.

### Assumptions on requirements
- The application will only track changes of files but not folders within the directory. It is because folders do not have file extension data, which is a required field in the AppFiles SQL Table.
- The File assignment can only be done by Editing AppFile Window but not Edit User Window. This is to prevent Database conflict as two windows can make changes simultaneously.

### Things to improve if I had more time

### Certain decision making
