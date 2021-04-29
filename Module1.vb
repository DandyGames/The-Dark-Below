'BUG NOTES
'30/10/2017 BUG - After going to scoreboard menu and returning to the main menu, if invalid input, displays the scoreboard menu
'01/11/2017 FIXED - put select case inside of try catch to allow the game to correctly exit the scoreboard menu
'06/11/2017 BUG - When reading level file into an array to make it easier to manipulate the program is running
'06/11/2017 BUG - the menu try catch and resulting in a "non-integer input error" then returning user to main menu
'06/11/2017 FIXED - removed unneccessary variable that was causing the game to exit play incorrectly
'17/11/2017 BUG - when you complete a level the non-integer input in menu error appears rather than the new game script.
'17/11/2017 FIXED - made for loop go to length of level - 1 so the new game script now runs
'20/11/2017 BUG - monster generation values are not always correct and treasure always = 10
'24/11/2017 FIXED - Treasure value scales as level number increases
'04/12/2017 BUG - Several levels in to game, the hit generation stops working correctly for attacks
'ABOVE BUG  FIXED - Boundaries had missed out a value
'28/03/2018 BUG - Backspace doesn't work when typing in your password.
'
'IMPROVEMENTS
'IDEA -Add splits in the pathway that actually matter (this could be done at player input stage)
'IMPLEMENTED - See play game section
'IDEA - Add a skill tree to give the game a better sense of progression
'IMPLEMENTED - basic skill tree added, cannot upgrade properly though...
'UPDATED - skill tree now works as intended! (kinda, shh don't tell anyone)
'
'TASKS
'RECURSIVE ALGORITHM
'
'NOTES
'If there is an error occuring that isn't in a try except, then players are kicked back to the menu
'When adding new skills, their unlock and stats must be added to Upgrade_Stats and Unlock_Abilities
'
'value = CInt(Math.Floor((upper - lower + 1) * Rnd())) + lower

Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Module Module1
    Public Structure Player                 'Stores all the details about the player
        Public Char_Name As String          'Stores player name
        Public Score As Decimal
        Public Skills(,) As String
        Public Skill_Points As Integer      'Stores how many skill points the player can invest in skills
        Public Inventory(,) As String
        Public Player_Health As Integer
        Public Mana_Amount As Integer       'Stores how much mana the player currently has
        Public Attack_Power As Integer
        Public Defense_Power As Integer
    End Structure
    Sub Main()
        Menu()                                      'runs the menu function at start of program
    End Sub
    Sub Menu()
        Console.Title() = "The Dark Below"
        Dim Option_Chosen As Integer                'stores the choice from the user
        Dim Continue_Menu As Boolean = True
        Do Until Continue_Menu = False              'ensures the menu loops until the user uses a valid input
            Console.WriteLine("-------------------")
            Console.WriteLine("-----MAIN MENU-----")
            Console.WriteLine("-------------------")
            Console.WriteLine("(1)New Game")
            Console.WriteLine("(2)Load Game")
            Console.WriteLine("(3)View Scoreboard")
            Console.WriteLine("(4)Quit")
            Console.WriteLine("-------------------" & vbCrLf)
            Console.Write("Choice > ")
            Try                                     'catches the user inputting a non-integer value
                Option_Chosen = Console.ReadLine()
                Select Case Option_Chosen           'determines which part of the game to load based on the user input
                    Case 1
                        Console.WriteLine(vbCrLf & "Starting Game..." & vbCrLf)
                        New_Game()
                        Continue_Menu = False
                    Case 2
                        Console.WriteLine(vbCrLf & "Loading Game..." & vbCrLf)
                        New_Game()
                        Continue_Menu = False
                    Case 3
                        Console.WriteLine("")
                        Console.WriteLine("")
                        Console.WriteLine(vbCrLf & "Navigating to scoreboard..." & vbCrLf)
                        Console.WriteLine("")
                        Console.WriteLine("")
                        Scoreboard()                'takes the user to the scoreboard. The menu can keep running here as the player will navigate back to the menu when leaving the scoreboard
                    Case 4
                        Console.WriteLine(vbCrLf & "Thank you for playing!" & vbCrLf)
                        Console.WriteLine(vbCrLf & "Press any key to exit")
                        Continue_Menu = False
                        Console.ReadKey()
                    Case Else                       'catches any remaining erroneous inputs from the user
                        Console.WriteLine(vbCrLf & "Input a number between 1 and 3, please" & vbCrLf)
                End Select
            Catch ex As Exception
                Console.WriteLine(vbCrLf & "Please input a number between 1 and 3" & vbCrLf)
            End Try

        Loop
    End Sub

    Sub Scoreboard()
        Dim SB_Menu_Choice As Integer               'stores any menu-related choices the user makes
        Dim SB_Menu_Continue As Boolean = True
        Dim Scores(4) As Decimal
        Do Until SB_Menu_Continue = False           'ensures the menu loops until the user wishes to exit
            Console.WriteLine("--------------------")
            Console.WriteLine("-----SCOREBOARD-----")
            Console.WriteLine("--------------------")
            Console.WriteLine("(1)View Top 5 Scores")
            Console.WriteLine("(2)Return to Menu")
            Console.WriteLine("--------------------" & vbCrLf)
            Console.Write("Choice > ")
            Try                                     'catches the user inputting a non-integer value
                SB_Menu_Choice = Console.ReadLine()
                Select Case SB_Menu_Choice          'determines which path to follow based on the users input
                    Case 1
                        Console.WriteLine("")
                        Console.WriteLine("Loading Scores..." & vbCrLf)
                        Console.WriteLine("--------------------")
                        Console.WriteLine("-------SCORES-------")
                        Console.WriteLine("--------------------")
                        Using Reader As StreamReader = New StreamReader("Scoreboard_File.txt")  'opens the text file containing the scores

                            For i = 0 To 4 Step 1                                               'cycles through the text file
                                Scores(i) = Reader.ReadLine
                                Console.WriteLine("Score " & (i + 1) & ": " & Scores(i))        'outputs each score and its place in the rankings
                            Next
                            Console.WriteLine("--------------------")
                            Console.WriteLine("")
                            Console.WriteLine("Try to beat these scores!" & vbCrLf)
                        End Using
                    Case 2
                        Console.WriteLine("")
                        Console.WriteLine("")
                        Console.WriteLine("Returning to Main Menu..." & vbCrLf)
                        Console.WriteLine("")
                        SB_Menu_Continue = False    'stops the menu looping so the player returns to the main menu
                    Case Else
                        Console.WriteLine("")
                        Console.WriteLine("Please input 1 or 2" & vbCrLf)
                End Select
            Catch ex As Exception
                Console.WriteLine("")
                Console.WriteLine("Please input either a 1 or 2")
                Console.WriteLine("")
            End Try
        Loop
    End Sub
    Sub New_Game()
        Dim Current_Player As Player
        Dim Login As Char
        Dim Login_Accept As Boolean = False
        Const intSkillsRowCount As Integer = 9
        Const intSkillsColumnCount As Integer = 6
        Const intItemColumnCount As Integer = 5
        Dim intItemRowCount As Integer = 4
        Dim Level_No As Integer = 1

        Do Until Login_Accept = True
            Try
                Console.WriteLine("Have you played the game before?")
                Console.Write(vbCrLf & "Y/N > ")
                Login = Console.ReadLine().ToUpper()
                If Login = "Y" Then
                    Console.WriteLine(vbCrLf & "Would you like to continue a previous game?")
                    Console.Write(vbCrLf & "Y/N > ")
                    Login = Console.ReadLine().ToUpper()

                    If Login = "Y" Then
                        Console.WriteLine("Loading game..." & vbCrLf)
                        Login_Accept = Login_User(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemColumnCount, intItemRowCount, Level_No)
                    ElseIf Login = "N" Then
                        Console.WriteLine(vbCrLf & "Starting a new game!" & vbCrLf)
                        New_Player(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemColumnCount, intItemRowCount)
                        Login_Accept = True
                    Else
                        Console.WriteLine("Please use either Y or N")
                    End If

                ElseIf Login = "N" Then
                    Console.WriteLine(vbCrLf & "Starting a new game..." & vbCrLf)
                    New_Player(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemColumnCount, intItemRowCount)
                    Login_Accept = True
                Else
                    Console.WriteLine("Please input either Y or N")
                End If

            Catch ex As Exception
                Console.WriteLine("Please input a Y for yes or an N for no.")
            End Try
        Loop
        Game_Loop(Current_Player, intSkillsRowCount, intSkillsColumnCount, Level_No, intItemRowCount, intItemColumnCount)
    End Sub

    Sub Game_Loop(ByRef Current_Player, ByVal intSkillsRowCount, ByVal intSkillsColumnCount, ByRef Level_No, ByRef intItemRowCount, ByRef intItemColumnCount)

        Dim End_Game As Boolean = False
        Dim Quit_Level As Boolean = False
        Dim Saved_Game As Boolean = False
        Const Max_Player_Health As Integer = 40

        Tutorial_View(Current_Player, intItemRowCount, intSkillsRowCount)                                   'allows the player to view the tutorial

        Do Until End_Game = True                    'loops until the player wishes to stop playing
            Level_Generator(Level_No)               'creates the level for the player to play
            Quit_Level = Play_Game(Current_Player, Max_Player_Health, Level_No, Quit_Level, intItemRowCount, intItemColumnCount, intSkillsRowCount, intSkillsColumnCount)
            Current_Player.Mana_Amount += Level_No
            Level_No += 1                           'increments the level number if the player is successful in completing the level
            If Quit_Level = False Then
                End_Game = End_Of_Level(Current_Player, intSkillsRowCount, intSkillsColumnCount, Level_No, intItemRowCount, intItemColumnCount, Saved_Game)  'determines whether the player wishes to continue
            Else
                Exit Do
            End If
        Loop

        If Current_Player.Player_Health <= 0 Then
            Console.WriteLine("Now that " & Current_Player.Char_Name & " has passed, the game is over...")
            Current_Player.Score = Current_Player.Score * 0.9
        Else
            Current_Player.Score = Current_Player.Score * 1.1                                               'multiplies the players score by 1.1 as they chose to quit
        End If
        If Saved_Game = True Then
            Console.WriteLine(vbCrLf & "Returning to main menu...")
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
        Else
            End_Of_Game(Current_Player, Level_No, intItemRowCount)               'runs the end of the game function where leaderboard presence is determined
        End If
        Menu()
    End Sub

    Sub New_Player(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemColumnCount, intItemRowCount)
        Current_Player.Score = 0
        Current_Player.Player_Health = 40
        Current_Player.Mana_Amount = 10
        Current_Player.Attack_Power = 0
        Current_Player.Defense_Power = -1
        Current_Player.Skill_Points = 1

        Dim Level_No As Integer = 1
        Dim Create_Account As Char
        Dim Account_Creation As Boolean = False

        Console.WriteLine("Pick a name for your hero!" & vbCrLf)
        Console.Write("Name > ")
        Current_Player.Char_Name = Console.ReadLine()
        Current_Player.Inventory = Create_Player_Inventory(intItemRowCount, intItemColumnCount)             'loads the player's inventory
        If Current_Player.Char_Name = "All" Or Current_Player.Char_Name = "Player" Then
            Current_Player.Skills = Dev_Reading(intSkillsRowCount, intSkillsColumnCount)
        Else
            Current_Player.Skills = Reading(intSkillsRowCount, intSkillsColumnCount)                            'loads the player's skill tree
        End If
        If Current_Player.Char_Name = "Groot" Then
            Current_Player.Skill_Points += 100
        End If
        Do Until Account_Creation = True
            Console.WriteLine("Do you have an account?")
            Console.Write(vbCrLf & "Y/N > ")

            Try
                Create_Account = Console.ReadLine().ToUpper()
                If Create_Account = "Y" Then
                    Account_Creation = True
                    Console.WriteLine(vbCrLf & "Game load started...")
                ElseIf Create_Account = "N" Then
                    Console.WriteLine("Would you like to create an account?")
                    Console.WriteLine(vbCrLf & "If you do not create an account, you will not be able to save your current game.")
                    Console.Write(vbCrLf & "Y/N > ")
                    Create_Account = Console.ReadLine().ToUpper()
                    If Create_Account = "Y" Then
                        Create_User()
                        Account_Creation = True
                        Console.WriteLine(vbCrLf & "Account successfully created!")
                    Else
                        Account_Creation = True
                    End If
                Else
                    Console.WriteLine("Please input Y or N")
                End If
            Catch ex As Exception
                Console.WriteLine("Please choose a valid option given.")
            End Try
        Loop
        Game_Loop(Current_Player, intSkillsRowCount, intSkillsColumnCount, Level_No, intItemRowCount, intItemColumnCount)
    End Sub
    Sub Create_User()
        Dim Username As String
        Dim Salt As String
        Dim Hash_Pass As String
        Dim Password As String
        Dim File As String
        Dim User_Available As Boolean = True

        Console.WriteLine("---------------")
        Console.WriteLine("CREATE NEW USER")
        Console.WriteLine("---------------")
        Do While User_Available = True
            Console.Write(vbCrLf & vbCrLf & "Please input your username: ")
            Username = Console.ReadLine()
            Console.Write(vbCrLf & "Please input your password: ")
            Password = Get_Password()
            Salt = Get_Salt()
            Hash_Pass = Hash_512(Password, Salt)
            Using StreamReader As New StreamReader("Potato.txt")
                File = StreamReader.ReadToEnd().TrimEnd()
            End Using
            Dim Split_File As String() = File.Split(";")
            For i = 0 To Split_File.Length - 1
                Dim Single_User() As String = Split_File(i).Split(":")
                If Single_User(0) = Username Then
                    Console.WriteLine(vbCrLf & "This username has already been taken.")
                    User_Available = False
                    Exit For
                End If
            Next
            If User_Available = True Then
                File += Username + ":" + Salt + ":" + Hash_Pass + ";"
                Using StreamWriter As New StreamWriter("Potato.txt")
                    StreamWriter.Write(File)
                End Using
                User_Available = False
            End If
        Loop
    End Sub
    Function Login_User(ByRef Current_Player, ByVal intSkillsRowCount, ByVal intSkillsColumnCount, ByRef intItemColumnCount, ByRef intItemRowCount, ByRef Level_No)
        Dim Username As String
        Dim Password_Attempt As String
        Dim File As String
        Dim Salt As String = ""
        Dim Password As String
        Dim Found As Boolean = False
        Dim Hash_Pass As String
        Dim Exit_Loop As Boolean = False
        Dim New_Attempt As Char
        Do Until Exit_Loop = True
            Console.Write("Please input your username: ")
            Username = Console.ReadLine()
            Console.Write(vbCrLf & "Please input your password: ")
            Password_Attempt = Get_Password()

            Using StreamReader As New StreamReader("Potato.txt")
                File = StreamReader.ReadToEnd().Trim(" ")
            End Using

            Dim Rows() As String = File.Split(";")
            For i = 0 To Rows.Length - 1 Step 1
                Dim Single_Row() As String = Rows(i).Split(":")
                If Single_Row(0) = Username Then
                    Salt = Single_Row(1)
                    Password = Single_Row(2)
                    Found = True
                    Exit For
                End If
            Next

            If Found = True Then
                Hash_Pass = Hash_512(Password_Attempt, Salt)

                If Hash_Pass = Password Then
                    Console.WriteLine(vbCrLf & vbCrLf & "You have logged in" & vbCrLf)
                    Console.ReadKey()

                    Read_Player_Data(Current_Player, Username, intSkillsRowCount, intSkillsColumnCount, intItemRowCount, intItemColumnCount, Level_No)
                    Return True
                Else
                    Console.WriteLine(vbCrLf & "Invalid Password")
                    Console.ReadKey()
                    Console.WriteLine("Would you like to try again?")
                    Console.Write(vbCrLf & "Y/N > ")
                    New_Attempt = Console.ReadLine().ToUpper()
                    If New_Attempt = "N" Then
                        Exit_Loop = True
                        Return False
                    End If
                End If

            Else
                Console.WriteLine(vbCrLf & "User not found")
                Console.ReadKey()
                Console.WriteLine("Would you like to try again?")
                Console.Write(vbCrLf & "Y/N > ")
                New_Attempt = Console.ReadLine().ToUpper()
                If New_Attempt = "N" Then
                    Exit_Loop = True
                    Return False
                End If
            End If

        Loop

    End Function
    Sub Read_Player_Data(ByRef Current_Player, ByRef Username, ByRef intSkillsRowCount, ByRef intSkillsColumnCount, ByRef intItemRowCount, ByRef intItemColumnCount, ByRef Level_No)
        Dim File_Name As String = Username & ".txt"
        Dim File As String

        Using Reader As New StreamReader(File_Name)
            File = Reader.ReadToEnd()
        End Using

        Dim String_Original As Byte() = Convert.FromBase64String(File)
        Dim Player_Data As String = Encoding.UTF8.GetString(String_Original)
        Dim Data_Array() As String = Player_Data.Split(":")
        Dim Skills As String(,)

        intItemRowCount = Data_Array(10)
        ReDim Skills(intSkillsRowCount - 1, intSkillsColumnCount - 1)
        Current_Player.Char_Name = Data_Array(0)
        Current_Player.Score = Data_Array(1)

        Dim Split_Skills() As String = Data_Array(2).Split(";")
        For i = 0 To intSkillsRowCount - 2 Step 1
            Dim Skill_Holder() As String = Split_Skills(i).Split(",")
            For j = 0 To intSkillsColumnCount - 1 Step 1
                Try
                    Skills(i, j) = Skill_Holder(j)
                Catch ex As Exception

                End Try
            Next
        Next

        Current_Player.Skills = Skills
        Current_Player.Skill_Points = Data_Array(3)

        Dim Item_List() As String = Data_Array(4).Split(";")
        Dim Items As String(,)
        ReDim Items(intItemRowCount - 1, intItemColumnCount - 1)
        For i = 0 To intItemRowCount - 1
            Dim Temp_Item() As String = Item_List(i).Split(",")
            For j = 0 To intItemColumnCount - 1 Step 1
                Items(i, j) = Temp_Item(j)
            Next
        Next

        Current_Player.Inventory = Items
        Current_Player.Player_Health = Data_Array(5)
        Current_Player.Mana_Amount = Data_Array(6)
        Current_Player.Attack_Power = Data_Array(7)
        Current_Player.Defense_Power = Data_Array(8)
        Level_No = Data_Array(9)
    End Sub

    Function Get_Password()
        Dim Password As String = ""
        While True
            Dim Key = Console.ReadKey(True)
            If Key.Key = ConsoleKey.Enter Then
                Exit While
            ElseIf Key.Key = ConsoleKey.Backspace Then
                Password = Password.Remove(Password.Length - 1, 1)
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)
                Console.Write(" ")
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop)
            Else
                Console.Write("*")
                Password += Key.KeyChar
            End If
        End While
        Return Password
    End Function
    Function Get_Salt() As String
        Dim Mix As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+=][}{<>"
        Dim Salt As String = ""
        Dim rnd As New Random
        For i = 1 To 100
            Dim x As Integer = rnd.Next(0, Mix.Length - 1)
            Salt &= (Mix.Substring(x, 1))
        Next
        Return Salt
    End Function
    Public Function Hash_512(password As String, salt As String) As String
        Dim convertedToBytes As Byte() = Encoding.UTF8.GetBytes(password & salt)
        Dim hashType As HashAlgorithm = New SHA512Managed()
        Dim hashBytes As Byte() = hashType.ComputeHash(convertedToBytes)
        Dim hashedResult As String = Convert.ToBase64String(hashBytes)
        Return hashedResult
    End Function

    Sub Level_Generator(ByVal Level_No)
        Dim Max_Total_Rooms As Integer = 3 * Level_No

        Dim Gen_No_P As Integer = 0                 'stores no of generated rooms of each type
        Dim Gen_No_M As Integer = 0
        Dim Gen_No_T As Integer = 0

        Dim Previous As Char = "S"                  'contains the value of the last generated room
        Dim Random_Choice As Integer                'stores the randomly generated room value
        Dim Room_Type As Boolean = False

        Using Writer As StreamWriter = New StreamWriter("Level.txt")
            Writer.Write("S,")                      'Indicates the start of the level
            For i = 1 To Max_Total_Rooms Step 1
                Randomize()                         'Sets the randomiser seed
                Do Until Room_Type = True           'Generates a room type until a valid room type is chosen
                    Random_Choice = CInt(Math.Floor((5 - 1) * Rnd())) + 0
                    Select Case Random_Choice       'Edits the Level file based on the randomly generated room value
                        Case 0
                            If Gen_No_P = Level_No Then  'Ensures number of Path splits is not more than necessary for level_no
                            Else                    'Adds the pathway split to the file. Deadend is always to the left currently.
                                Writer.Write("PD,")
                                Gen_No_P += 1
                                Room_Type = True    'Exits the loop as a valid room type has been generated
                                Previous = "P"
                            End If
                        Case 1
                            If Gen_No_M = Level_No Then
                            Else
                                Writer.Write("M,")  'Writes monster room to the file and adds the comma to denote a new room slot
                                Gen_No_M += 1
                                Room_Type = True
                                Previous = "M"      'stores previous room generated
                            End If
                        Case 2
                            If Gen_No_T = Level_No Then
                            Else
                                Writer.Write("T,")
                                Gen_No_T += 1       'increments the number of this type of room generated so far
                                Room_Type = True
                                Previous = "T"
                            End If
                        Case 3
                            If Previous = "S" Or Previous = "N" Then
                                Previous = ""
                                'checks if the previous room was the start or empty so there aren't too many empty rooms together
                            Else
                                Writer.Write("N,")
                                Room_Type = True
                                Previous = "N"
                            End If
                        Case Else                   'deals with any incorrectly generated values and informs the player that the value generated was wrong
                            Console.WriteLine("Invalid Random Value Generated, Fix Code")
                    End Select
                Loop
                Room_Type = False                   'sets Room_Type to False so that the nested do...until will run and won't be skipped
            Next
            Writer.Write("E")                       'adds the exit of the level to the generated series of rooms
        End Using
    End Sub

    'Skills Unlocks, upgrades etc
    Function Reading(ByRef intSkillsRowCount, ByRef intSkillsColumnCount)
        Dim Skills As String(,)
        Dim SkillsFile As String

        ReDim Skills(intSkillsRowCount - 1, intSkillsColumnCount - 1)

        Using Reader As StreamReader = New StreamReader("SkillTree.txt")    'reads in a blank skill tree from a file
            SkillsFile = Reader.ReadToEnd()
        End Using

        Dim SplitSkills() As String = SkillsFile.Split(";")                 'splits the file into individual skills

        For i = 0 To intSkillsRowCount - 1
            Dim TempSkills() As String = SplitSkills(i).Split(",")          'splits single skills into seperate attributes

            For j = 0 To intSkillsColumnCount - 1
                Skills(i, j) = TempSkills(j)                                'stores the skills in the player's skill array
            Next
        Next
        Return Skills
    End Function
    Sub Use_Skill_Points(ByRef Current_Player, ByVal Level_No, ByRef intSkillsRowCount, ByRef intSkillsColumnCount)
        Dim Use_Points As Char
        Dim Exit_Loop As Boolean = False
        Dim Use_Remaining As Char
        Dim Remaining_Loop As Boolean = False

        Current_Player.Skill_Points += (Level_No * 3 / 2) - 1
        Do Until Exit_Loop = True
            Console.WriteLine(vbCrLf)
            Console.WriteLine(Current_Player.Char_Name & " has " & Current_Player.Skill_Points & " skill points.")
            Console.WriteLine("")
            Console.WriteLine("Would you like to use your skill points?")
            Console.Write(vbCrLf & "Y/N >")
            Try
                Use_Points = Console.ReadLine().ToUpper()
                Select Case Use_Points
                    Case "Y"
                        Console.WriteLine("")
                        Gain_Abilities(Current_Player, Level_No, intSkillsRowCount, intSkillsColumnCount)
                        Upgrade_Stats(Current_Player)
                        Unlock_Abilities(Current_Player)
                        Do Until Remaining_Loop = True
                            Try
                                If Current_Player.Skill_Points > 0 Then
                                    Console.WriteLine("Would you like to use your remaining " & Current_Player.Skill_Points & " points?")
                                    Console.Write(vbCrLf & "Y/N >")
                                    Use_Remaining = Console.ReadLine.ToUpper()
                                    If Use_Remaining = "N" Then
                                        Console.WriteLine("Exiting upgrade menu..." & vbCrLf)
                                        Remaining_Loop = True
                                        Exit_Loop = True
                                    ElseIf Use_Remaining = "Y" Then
                                        Remaining_Loop = True
                                    Else
                                        Console.WriteLine("Please input a Y for yes or an N for no.")
                                    End If
                                Else
                                    Console.WriteLine(vbCrLf & "You have no points remaining!" & vbCrLf)
                                    Console.WriteLine("Exiting upgrade menu...")
                                    Remaining_Loop = True
                                    Exit_Loop = True
                                End If
                            Catch ex As Exception
                                Console.WriteLine("Please input either yes (Y) or no (N) to using your points.")
                            End Try
                        Loop
                    Case "N"
                        Console.WriteLine(Current_Player.Char_Name & " does not use their skill points.")
                        Console.WriteLine("")
                        Exit_Loop = True
                    Case Else
                        Console.WriteLine("Please input either yes (Y) or no (N) for using skill points.")
                End Select
            Catch ex As Exception
                Console.WriteLine("Please input either yes (Y) or no (N)")
            End Try
        Loop

    End Sub
    Sub Gain_Abilities(ByRef Current_Player, ByVal Level_No, ByRef intSkillsRowCount, ByRef intSkillsColumnCount)
        Dim Ability_Value As Integer
        Dim No_Of_Points As Integer
        Dim Upgradable_Abilities(intSkillsRowCount) As String
        Dim No_Of_Abilities As Integer = Output_Upgrade_Abilities(Current_Player, intSkillsRowCount, intSkillsColumnCount, Upgradable_Abilities)
        If Current_Player.Skill_Points > 0 Then
            Console.WriteLine("Please input the number of the skill you wish to upgrade:")
            Console.Write(vbCrLf & ">")
            Try
                Ability_Value = Console.ReadLine()
                If Ability_Value <= No_Of_Abilities And Ability_Value > 0 Then
                    Ability_Value -= 1
                    For i = 0 To intSkillsRowCount - 1 Step 1
                        If Upgradable_Abilities(Ability_Value) = Current_Player.Skills(i, 0) Then
                            Ability_Value = i
                            Exit For
                        End If
                    Next
                    Console.WriteLine(Upgradable_Abilities(Ability_Value) & " will be upgraded.")
                    Console.WriteLine("How many points do you wish to use? You have " & Current_Player.Skill_Points)
                    Console.Write(vbCrLf & ">")
                    No_Of_Points = Console.ReadLine()
                    If No_Of_Points >= 0 AndAlso No_Of_Points <= Current_Player.Skill_Points And Current_Player.Skills(Ability_Value, 3) <> Current_Player.Skills(Ability_Value, 4) Then

                        If No_Of_Points + Current_Player.Skills(Ability_Value, 4) >= Current_Player.Skills(Ability_Value, 3) Then
                            Console.WriteLine(Upgradable_Abilities(Ability_Value) & " will be maxed out")
                            No_Of_Points = (Current_Player.Skills(Ability_Value, 3) - Current_Player.Skills(Ability_Value, 4))
                            Current_Player.Skill_Points -= No_Of_Points
                            Current_Player.Skills(Ability_Value, 4) = Current_Player.Skills(Ability_Value, 3)
                        Else
                            Console.WriteLine(Current_Player.Skills(Ability_Value, 0) & " will be upgraded by " & No_Of_Points & ".")
                            Current_Player.Skills(Ability_Value, 4) += No_Of_Points
                            Current_Player.Skill_Points -= No_Of_Points
                        End If
                    ElseIf Current_Player.Skills(Ability_Value, 3) = Current_Player.Skills(Ability_Value, 4) Then
                        Console.WriteLine("This ability is maxed out!")
                    Else
                        Console.WriteLine("Please input a number of points less than " & Current_Player.Skill_Points)
                    End If

                Else
                    Console.WriteLine("Please input a number that corresponds to a skill.")
                End If
            Catch ex As Exception
                Console.WriteLine("Please input an integer value")
            End Try
        Else
            Console.WriteLine("")
            Console.WriteLine("You have no skill points available!")
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
        End If
    End Sub
    Sub Upgrade_Stats(ByRef Current_Player)
        For i = 0 To 7 Step 1
            If Current_Player.Skills(i, 1) = "STAT" Then                    'upgrades the player's stats (attack, defense, total mana etc)
                If Current_Player.Skills(i, 4) > 0 And Current_Player.Skills(i, 0) = "Attack Up" Then
                    Current_Player.Attack_Power = Current_Player.Skills(i, 4) + 2
                ElseIf Current_Player.Skills(i, 4) > 0 And Current_Player.Skills(i, 0) = "Defense Up" Then
                    Current_Player.Defense_Power = Current_Player.Skills(i, 4) + 1
                ElseIf Current_Player.Skills(i, 4) > 0 And Current_Player.Skills(i, 0) = "Mana Total Up" Then
                    Current_Player.Mana_Amount += Current_Player.Skills(i, 4)
                ElseIf Current_Player.Skills(i, 4) > 0 And Current_Player.Skills(i, 0) = "Super Mana Up" Then
                    Current_Player.Mana_Amount += Current_Player.Skills(i, 4) + 2
                End If
            End If
        Next
    End Sub
    Sub Unlock_Abilities(ByRef Current_Player)
        If Current_Player.Skills(0, 3) = Current_Player.Skills(0, 4) And Current_Player.Skills(3, 5) <> "Available" Then
            Current_Player.Skills(3, 5) = "Available"
        ElseIf Current_Player.Skills(1, 3) = Current_Player.Skills(1, 4) And Current_Player.Skills(4, 5) <> "Available" Then
            Current_Player.Skills(4, 5) = "Available"
        ElseIf Current_Player.Skills(2, 3) = Current_Player.Skills(2, 4) And Current_Player.Skills(6, 5) <> "Available" Then
            Current_Player.Skills(6, 5) = "Available"
        ElseIf Current_Player.Skills(6, 3) = Current_Player.Skills(6, 4) And Current_Player.Skills(7, 5) <> "Available" Then
            Current_Player.Skills(7, 5) = "Available"
        ElseIf Current_Player.Skills(5, 3) = Current_Player.Skills(5, 4) And Current_Player.Skills(8, 5) <> "Available" Then
            Current_Player.Skills(8, 5) = "Available"
        ElseIf Current_Player.Skills(3, 3) = Current_Player.Skills(3, 4) And Current_Player.Skills(4, 3) = Current_Player.Skills(4, 4) Then
            Current_Player.Skills(5, 5) = "Available"
        End If
    End Sub
    Function Output_Upgrade_Abilities(ByRef Current_Player, ByRef intSkillsRowCount, ByRef intSkillsColumnCount, ByRef Upgradable_Abilities)
        Dim No_Of_Abilities As Integer = 0
        '------------

        Console.WriteLine("-----------------------------------------------------------")
        Console.WriteLine("--------------------SKILLS UPGRADE MENU--------------------")
        Console.WriteLine("-----------------------------------------------------------")
        Console.WriteLine("Number" & vbTab & "Name" & vbTab & vbTab & "Max Points" & vbTab & "Current Points")
        Console.WriteLine("-----------------------------------------------------------")
        For i = 0 To intSkillsRowCount - 1 Step 1
            If Current_Player.Skills(i, 4) > 0 Or Current_Player.Skills(i, 5) = "Available" Then
                Upgradable_Abilities(No_Of_Abilities) = Current_Player.Skills(i, 0)
                No_Of_Abilities += 1
                Console.WriteLine("(" & No_Of_Abilities & ")" & vbTab & Current_Player.Skills(i, 0) & vbTab & Current_Player.Skills(i, 3) & vbTab & vbTab & Current_Player.Skills(i, 4))
            End If
        Next
        Console.WriteLine("-----------------------------------------------------------")
        Console.WriteLine(vbCrLf & "-----------------------------------------------------------")
        Return No_Of_Abilities
    End Function
    Function Dev_Reading(ByRef intSkillsRowCount, ByRef intSkillsColumnCount)
        Dim Skills As String(,)
        Dim SkillsFile As String

        ReDim Skills(intSkillsRowCount - 1, intSkillsColumnCount - 1)

        Using Reader As StreamReader = New StreamReader("DevSkillTree.txt")
            SkillsFile = Reader.ReadToEnd()
        End Using

        Dim SplitSkills() As String = SkillsFile.Split(";")
        For i = 0 To intSkillsRowCount - 1
            Dim TempSkills() As String = SplitSkills(i).Split(",")

            For j = 0 To intSkillsColumnCount - 1
                Skills(i, j) = TempSkills(j)
            Next
        Next

        Return Skills
    End Function

    'Tutorial
    Sub Tutorial_View(Current_Player, intItemRowCount, intSkillsRowCount)
        Dim Tutorial_View As Char
        Dim Continue_Try As Boolean = True          'continues to loop until the user inputs a valid selection
        Do Until Continue_Try = False
            Try
                Console.WriteLine("")
                Console.WriteLine("Would you like to view the tutorial" & vbCrLf)
                Console.Write("Y/N > ")
                Tutorial_View = Console.ReadLine.ToUpper()
                Select Case Tutorial_View           'selects which path to follow based on whether the user wishes to view the tutorial
                    Case "Y"
                        Console.WriteLine(vbCrLf)
                        Tutorial(Current_Player, intItemRowCount, intSkillsRowCount)
                        Continue_Try = False
                    Case "N"
                        Console.WriteLine("")
                        Continue_Try = False
                        Exit Sub
                    Case Else
                        Console.WriteLine("")
                        Console.WriteLine("You need to pick Yes (Y) or No (N)")
                        Console.WriteLine("")
                End Select
            Catch ex As Exception                   'catches any non-character inputs by the user
                Console.WriteLine("")
                Console.WriteLine("Come on, input a single letter, it's not that hard")
                Console.WriteLine("")
            End Try
        Loop
    End Sub
    Sub Tutorial(Current_Player, intItemRowCount, intSkillsRowCount)
        Dim End_Run As Boolean = False
        Dim Tutorial_Option As Integer
        Do Until End_Run = True
            Tutorial_Menu()
            Try
                Tutorial_Option = Console.ReadLine()
                Console.WriteLine()
                Select Case Tutorial_Option
                    Case 1
                        Navigation_Tutorial()
                    Case 2
                        Combat_Tutorial(Current_Player, intItemRowCount, intSkillsRowCount)
                    Case 3
                        Traps_Tutorial(Current_Player, intItemRowCount)
                    Case 10
                        Console.WriteLine("")
                        Console.WriteLine("Thank you for viewing the tutorial.")
                        Console.WriteLine("Please enjoy the game!")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine(vbCrLf)
                        End_Run = True
                    Case Else
                        Console.WriteLine("Please input an integer between 1 and 3, or 10 to Quit." & vbCrLf)
                End Select
            Catch ex As Exception
                Console.WriteLine("Please input an integer.")
            End Try
        Loop
    End Sub
    Sub Tutorial_Menu()
        Console.WriteLine("--------------")
        Console.WriteLine("---TUTORIAL---")
        Console.WriteLine("--------------")
        Console.WriteLine("(1)Navigation")
        Console.WriteLine("(2)Combat")
        Console.WriteLine("(3)Traps")
        Console.WriteLine("(10)Quit")
        Console.WriteLine("--------------")
        Console.WriteLine(vbCrLf & "Input a number that you wish to learn about:")
        Console.Write(vbCrLf & ">")
    End Sub
    Sub Navigation_Tutorial()
        Console.WriteLine("As you progress through a dungeon, you may encounter splits in the path.")
        Console.WriteLine("At a path split, you may go left (L) or right (R)")
        Console.Write(vbCrLf & vbCrLf & ">")
        Console.ReadKey()
        Console.WriteLine("If you pick the wrong path, you will return to the path split.")
        Console.WriteLine("There your character will take the correct path.")
        Console.Write(vbCrLf & vbCrLf & ">")
        Console.ReadKey()
        Console.WriteLine(vbCrLf)
    End Sub
    Sub Combat_Tutorial(Current_Player, intItemRowCount, intSkillsRowCount)
        Dim Combat_View As Integer
        Dim Continue_Loop As Boolean = True
        Dim temp As Integer
        Do Until Continue_Loop = False
            Console.WriteLine("")
            Console.WriteLine("During combat, you will see the following menu:")
            Console.WriteLine("")
            Console.WriteLine("==================================")
            Fight_Menu()
            Console.WriteLine("" & vbCrLf)
            Console.WriteLine("==================================")
            Console.WriteLine(vbCrLf & "If you would like to know more about an option input its integer.")
            Console.WriteLine("Else, input 10")
            Console.Write(vbCrLf & ">")
            Try
                Combat_View = Console.ReadLine()
                Select Case Combat_View
                    Case 1
                        Console.WriteLine("")
                        Console.WriteLine("You attack the enemy straight on, dealing damage based on strength.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                    Case 2
                        Console.WriteLine("")
                        Console.WriteLine("You defend against enemy attacks, reducing damage taken.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                    Case 3
                        Console.WriteLine("")
                        Console.WriteLine("This allows you to use combat skills.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("A menu like this will be displayed: ")
                        Console.WriteLine("========================")
                        Output_Skills_Available(Current_Player.Skills, intSkillsRowCount)
                        Console.WriteLine("")
                        Console.WriteLine("========================")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("Skills cost mana to use, which regenerates at the end of each dungeon.")
                        Console.WriteLine("Skills can be unlocked by using skill points at the end of each level.")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                    Case 4
                        Console.WriteLine("")
                        Console.WriteLine("This allows you to use an item in combat.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("A menu like this will be displayed: ")
                        Console.WriteLine("========================")
                        temp = Item_Array_Output(Current_Player, intItemRowCount)
                        Console.WriteLine("")
                        Console.WriteLine("========================")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("Not all items are useful in a fight.")
                        Console.WriteLine("As you play, you will learn which items can help you.")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                    Case 5
                        Console.WriteLine("")
                        Console.WriteLine("This allows you to attempt to run away from the enemy you are facing.")
                        Console.WriteLine("You cannot run away from boss fights and any monster over level 5.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                    Case 10
                        Console.WriteLine("Leaving combat tutorial...")
                        Console.WriteLine("")
                        Continue_Loop = False
                    Case Else
                        Console.WriteLine("Please input a number between 1 & 5, or 10 to exit.")
                        Console.WriteLine("")
                End Select
            Catch ex As Exception
                Console.WriteLine("Please input an integer value between 1 and 5")
                Console.WriteLine("")
            End Try
        Loop

    End Sub
    Sub Traps_Tutorial(Current_Player, intItemRowCount)
        Dim Trap_View As Integer
        Dim Continue_Loop As Boolean = True
        Dim temp As Integer
        Do Until Continue_Loop = False
            Console.WriteLine("When you encounter a trap, a brief description will display.")
            Console.WriteLine("Below that, a menu like this will be displayed:")
            Console.WriteLine("")
            Console.WriteLine("==================================")
            Trap_Menu()
            Console.WriteLine("" & vbCrLf)
            Console.WriteLine("==================================")
            Console.WriteLine(vbCrLf & "If you would like to know more about an option input its integer.")
            Console.WriteLine("Else, input 10")
            Console.Write(vbCrLf & ">")
            Try
                Trap_View = Console.ReadLine()
                Select Case Trap_View
                    Case 1      'Examine Trap
                        Console.WriteLine("")
                        Console.WriteLine("You will be given a hint on how to solve the trap.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                    Case 2      'Search Through Bag
                        Console.WriteLine("")
                        Console.WriteLine("This allows you to use an item in a trap.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("A menu like this will be displayed: ")
                        Console.WriteLine("========================")
                        temp = Item_Array_Output(Current_Player, intItemRowCount)
                        Console.WriteLine("")
                        Console.WriteLine("========================")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("Not all items are useful in all traps.")
                        Console.WriteLine("As you play, you will learn which items can help you in specific traps.")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                    Case 3      'Smash Door
                        Console.WriteLine("")
                        Console.WriteLine("You will attempt to break the door down using brute force.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                    Case 4      'Despair
                        Console.WriteLine("")
                        Console.WriteLine("You will despair at the inevitability of your demise.")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                    Case 10     'Quit
                        Console.WriteLine("Leaving trap tutorial...")
                        Console.WriteLine("")
                        Continue_Loop = False
                    Case Else
                        Console.WriteLine("Please input a number between 1 & 5, or 10 to exit.")
                        Console.WriteLine("")
                End Select
            Catch ex As Exception
                Console.WriteLine("Please input an integer value between 1 and 5")
                Console.WriteLine("")
            End Try
        Loop
    End Sub

    'Inventory (Non-specific)
    Function Create_Player_Inventory(ByRef intItemRowCount, ByRef intItemColumnCount)
        Dim Items As String(,)
        Dim Item_File As String

        ReDim Items(10 - 1, intItemColumnCount - 1)

        Using Reader As StreamReader = New StreamReader("InitialItems.txt")
            Item_File = Reader.ReadToEnd()
        End Using

        Dim Split_Items() As String = Item_File.Split(";")

        For i = 0 To intItemRowCount - 1
            Dim Temp_Items() As String = Split_Items(i).Split(",")
            For j = 0 To intItemColumnCount - 1
                Items(i, j) = Temp_Items(j)
            Next
        Next
        Return Items
    End Function
    Function Item_Array_Output(ByRef Current_Player, ByRef intItemRowCount)
        Dim Quit_Option As Integer
        Console.WriteLine("")
        Console.WriteLine("-------------------")
        Console.WriteLine("---BAG INVENTORY---")
        Console.WriteLine("-------------------")
        For i = 0 To intItemRowCount - 1 Step 1
            If i = intItemRowCount - 1 Then
                Console.WriteLine("(" & i & ")" & Current_Player.Inventory(i, 0))
                Console.WriteLine("(" & (i + 1) & ")" & "Quit")
                Quit_Option = i + 1
            Else
                Console.WriteLine("(" & i & ")" & Current_Player.Inventory(i, 0))
            End If
        Next
        Console.WriteLine("-------------------")
        Console.WriteLine()
        Return Quit_Option
    End Function
    Sub Delete_Item(ByRef Current_Player, ByRef intItemColumnCount, ByRef intItemRowCount, ByVal Item_Number)
        Dim Resulting_Item As String
        Dim Waste_Item As String()

        If Current_Player.Inventory(Item_Number, 2) = "-" Then
            For i = 0 To intItemColumnCount - 1
                Current_Player.Inventory(Item_Number, i) = ""
            Next
            Organise_Inventory(intItemRowCount, intItemColumnCount, Current_Player)
        Else
            Resulting_Item = Current_Player.Inventory(Item_Number, 2)
            Waste_Item = Get_Waste_Item(intItemRowCount, intItemColumnCount, Resulting_Item)
            For i = 0 To intItemColumnCount - 1
                Current_Player.Inventory(Item_Number, i) = Waste_Item(i)
            Next
        End If
    End Sub
    Function Get_Waste_Item(ByRef intItemRowCount, ByRef intItemColumnCount, ByVal Item_Name)
        Dim Item_File As String

        Using Reader As StreamReader = New StreamReader("Items.txt")
            Item_File = Reader.ReadToEnd()
        End Using

        Dim Split_Items() As String = Item_File.Split(";")
        For i = 0 To Split_Items.Length() - 1
            Dim Temp_Items() As String = Split_Items(i).Split(",")
            If Temp_Items(0) = Item_Name Then
                Return Temp_Items
            End If
        Next
    End Function
    Sub Organise_Inventory(ByRef intItemRowCount, ByRef intItemColumnCount, Current_Player)
        Dim Temp_Array(intItemColumnCount) As String
        For i = 0 To intItemRowCount - 1
            If Current_Player.Inventory(i, 0) = "" Then
                For j = 0 To intItemColumnCount - 1
                    If i < intItemRowCount - 1 Then
                        Temp_Array(j) = Current_Player.Inventory(i + 1, j)
                        Current_Player.Inventory(i, j) = Temp_Array(j)
                        Current_Player.Inventory(i + 1, j) = ""
                    Else
                        Exit For
                    End If
                Next
            End If
        Next
        intItemRowCount -= 1
    End Sub
    Function Item_Pick(ByRef Current_Player, ByRef Item_Number, ByRef intItemRowCount, ByRef intItemColumnCount)
        Dim Valid_Item As Boolean = False
        Dim Quit_Option As Integer
        Do Until Valid_Item = True
            Quit_Option = Item_Array_Output(Current_Player, intItemRowCount)
            Console.WriteLine("To use an item, type its item number.")
            Console.WriteLine("")
            Console.Write("Choice > ")
            Try
                Item_Number = Console.ReadLine.ToUpper()
                If Item_Number = Quit_Option Then
                    Return False
                ElseIf Item_Number <= Quit_Option - 1 And Item_Number >= 0 Then
                    Return True
                Else
                    Console.WriteLine("Invalid item number.")
                End If
            Catch ex As Exception
                Console.WriteLine(vbCrLf & "To use an item, please input its item number.")
            End Try
        Loop
    End Function
    Sub Remove_Item(ByRef Current_Player, ByRef intItemRowCount, ByRef intItemColumnCount)
        Dim Item_Drop_Number As Integer
        Dim Valid_Option As Boolean = False
        Do Until Valid_Option = True
            Item_Array_Output(Current_Player, intItemRowCount)
            Console.WriteLine("Which item number would you like to destroy: ")
            Console.Write(vbCrLf & ">")
            Try
                Item_Drop_Number = Console.ReadLine()
                If Item_Drop_Number >= intItemRowCount - 1 Or Item_Drop_Number < -1 Then
                    Console.WriteLine("No items will be destroyed.")
                    Console.WriteLine("")
                    Valid_Option = True
                Else
                    Delete_Item(Current_Player, intItemColumnCount, intItemRowCount, Item_Drop_Number)
                    Valid_Option = True
                End If
            Catch ex As Exception
                Console.WriteLine("Please input a valid item number.")
            End Try
        Loop
    End Sub

    'Main Game
    Function Play_Game(ByRef Current_Player, ByVal Max_Player_Health, ByVal Level_No, ByRef Quit_Level, ByRef intItemRowCount, ByRef intItemColumnCount, ByRef intSkillsRowCount, ByRef intSkillsColumnCount)
        Dim Level_File As String                    'stores values read directly from generated level file
        Using Reader As StreamReader = New StreamReader("Level.txt")
            Level_File = Reader.ReadToEnd()         'reads the entire text file into the Level_File variable
        End Using
        Dim Level_Array() As String = Level_File.Split(",") 'splits level_file by "," to put it into an array
        'generation variables
        Dim Treasure_Value As Decimal
        Dim Boss_Or_Treasure As Integer
        Dim Encounter_Type As Char
        Dim Random_Direction As Integer

        Dim Level_Over As Boolean = False
        'Player data variables
        Dim Temp As String
        Dim Path_Choice As String
        Dim Picked_Path As Boolean = False


        'other variables
        Dim Dev_Tools As Boolean = False            'Sets Dev Tools to on or off
        Dim Dev_Skip As Boolean = False
        Dim Random_Text As Integer                  'Stores randomly generated numbers that allow for different dialogue options

        If Current_Player.Char_Name = "bob" Or Current_Player.Char_Name = "All" Then
            Dev_Tools = True
            Dev_Skip = True
        End If

        Do Until Current_Player.Player_Health <= 0 Or Level_Over = True
            For i = 0 To (Level_Array.Length() - 1) Step 1    'Moves the player through the dungeon step by step
                Randomize()
                If Dev_Tools = True Then                'allows developer/player who knows cheat code to skip rooms to speed up gameplay
                    Do Until Dev_Skip = False           'this will break if player or dev opts to skip room code "E"
                        Console.WriteLine("Room is " & Level_Array(i) & ", run room? Y/N")
                        Temp = Console.ReadLine.ToUpper()
                        If Temp = "Y" Then
                            Dev_Skip = False
                        ElseIf Temp = "N" Then
                            i += 1
                        Else
                            Console.WriteLine("Invalid Dev tools input")
                        End If
                    Loop
                End If

                Select Case Level_Array(i)              'Selects which code section to run based on the room type
                    Case "S"
                        Console.WriteLine("======================================================" & vbCrLf)
                        Console.WriteLine("")
                        Console.WriteLine(Current_Player.Char_Name & " approaches the dungeon.")
                        Console.WriteLine("")
                        Random_Text = CInt(Math.Floor(5 - 1 + 1) * Rnd()) + 1
                        Select Case Random_Text         'Adds some variety to the games opening, to prevent players getting bored.
                            Case 1
                                Console.WriteLine("The caverns are dark and cold, " & Current_Player.Char_Name & " shivers...")
                            Case 2
                                Console.WriteLine(Current_Player.Char_Name & " hears cackling laughter emanating from within...")
                            Case 3
                                Console.WriteLine("As " & Current_Player.Char_Name & " creeps inside, their torch goes out! Leaving them in inky darkness...")
                            Case 4
                                Console.WriteLine("'Are you ready to face us, " & Current_Player.Char_Name & "?' a voice whispers...")
                            Case 5
                                Console.WriteLine("Scuttling echoes around " & Current_Player.Char_Name & ", who looks around nervously...")
                            Case Else
                                Console.WriteLine("The stench of rotting flesh overwhelms " & Current_Player.Char_Name & "'s senses...")
                        End Select
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine(vbCrLf & "======================================================")
                    Case "N"                            '"Story Room" has no consequence on the player's action, but adds to the atmosphere
                        Console.WriteLine("")

                        Random_Text = CInt(Math.Floor(5 - 1 + 1) * Rnd()) + 1
                        'Random_Text = 4                'Testing only
                        Console.WriteLine("======================================================" & vbCrLf)
                        Select Case Random_Text         'Adds description, to prevent players getting bored.
                            Case 1
                                Console.WriteLine(Current_Player.Char_Name & " whips their head back and forth, nerves frayed...")
                            Case 2
                                Console.WriteLine(Current_Player.Char_Name & " comes crashing to the floor!")
                                Console.WriteLine("They frantically look for their adversary, only to find a pile of bones...")
                            Case 3
                                Console.WriteLine("Freezing water drips onto " & Current_Player.Char_Name & ", running down their spine like icy tendrils...")
                            Case 4

                                Console.WriteLine("Goosebumps rise on " & Current_Player.Char_Name & "'s skin as the wind howls...")
                            Case 5
                                Console.WriteLine(Current_Player.Char_Name & " peers into the endless darkness, their heart racing...")
                            Case Else
                                Console.WriteLine(Current_Player.Char_Name & " glances over their shoulder, paranoia rising...")
                        End Select
                        Console.WriteLine("")
                        Console.WriteLine("They continue moving further into the darkness.")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine("======================================================")
                    Case "T"                            'trap room
                        Console.WriteLine("")
                        Console.WriteLine("======================================================" & vbCrLf & vbCrLf)
                        Level_Over = Trap(Current_Player, Level_No, intItemRowCount, intItemColumnCount)
                        If Level_Over = True Then
                            Console.WriteLine(vbCrLf & Current_Player.Char_Name & " tragically passes away...")
                            Return True
                        Else
                            Current_Player.Score += 5
                        End If
                    Case "M"                            'monster room
                        Encounter_Type = "M"
                        Monster_Encounter(Current_Player, Level_No, Encounter_Type, intItemRowCount, intItemColumnCount, intSkillsRowCount, intSkillsColumnCount, Max_Player_Health)
                        If Current_Player.Player_Health <= 0 Then
                            Return True
                        End If
                    Case "PD"                            'gives player a pathway split
                        Random_Text = CInt(Math.Floor((4 - 1 + 1) * Rnd())) + 1
                        Do Until Picked_Path = True
                            Picked_Path = Path_Picker(Current_Player, Random_Text, Path_Choice)
                            If Picked_Path = False Then
                                Console.WriteLine("Please choose left (L) or right (R).")
                            End If
                        Loop
                        Picked_Path = False
                        Random_Direction = CInt(Math.Floor(2 - 1 + 1) * Rnd()) + 1
                        If Random_Direction = 1 Then
                            If Path_Choice = "L" Then
                                Console.WriteLine(vbCrLf & Current_Player.Char_Name & " finds their way deeper into the dungeon...")
                            Else
                                Console.WriteLine("")
                                Console.WriteLine(Current_Player.Char_Name & " finds themselves in a familiar room.")
                                Console.WriteLine("")
                                Console.WriteLine("They curse under their breath and take the second path.")
                            End If
                        Else
                            If Path_Choice = "R" Then
                                Console.WriteLine(vbCrLf & Current_Player.Char_Name & " finds their way deeper into the dungeon...")
                            Else
                                Console.WriteLine("")
                                Console.WriteLine(Current_Player.Char_Name & " walks in circles and ends up back at the path split.")
                                Console.WriteLine("")
                                Console.WriteLine("They curse under their breath and take the first path.")
                            End If
                        End If
                        Path_Choice = ""
                        Console.WriteLine(vbCrLf & "======================================================" & vbCrLf & vbCrLf)
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                    Case "E"                            'takes player to the end of the level
                        Console.WriteLine(vbCrLf & Current_Player.Char_Name & " has reached the final room in this dungeon!")
                        Console.WriteLine("")
                        Console.WriteLine("======================================================")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()

                        Boss_Or_Treasure = CInt(Math.Floor(2 * Rnd())) + 1
                        Randomize()
                        If Boss_Or_Treasure = 1 Then    'sends the player to a boss fight
                            Console.WriteLine(vbCrLf & Current_Player.Char_Name & " sees a massive monster towering over them!")
                            Encounter_Type = "B"
                            Monster_Encounter(Current_Player, Level_No, Encounter_Type, intItemRowCount, intItemColumnCount, intSkillsRowCount, intSkillsColumnCount, Max_Player_Health)
                            If Current_Player.Player_Health > 0 Then
                                Treasure_Value = CInt(Math.Floor((Level_No * 2) * Rnd())) + Level_No
                                Console.WriteLine("The boss was protecting treasure worth " & Treasure_Value & "!")
                                Current_Player.Score += Treasure_Value
                                Current_Player.Score += 50
                                Console.WriteLine("Your Score is now: " & Current_Player.Score & vbCrLf)
                                Console.Write(vbCrLf & ">")
                                Console.ReadKey()
                                Level_Over = True
                            Else
                                Console.WriteLine(Current_Player.Char_Name & " fought valiantly against the monster...")
                                Return True
                            End If
                        ElseIf Boss_Or_Treasure = 2 Then    'player finds the exit without a battle
                            Console.WriteLine(Current_Player.Char_Name & " walks in to a room filled with glistening treasure!")
                            Console.Write(vbCrLf & ">")
                            Console.ReadKey()
                            Treasure_Value = CInt(Math.Floor((Level_No - 1) * Rnd())) + 10
                            Console.WriteLine("The treasure inside is worth: " & Treasure_Value)
                            Current_Player.Score += Treasure_Value
                            Current_Player.Score += 50
                            Console.WriteLine("Your Score is now: " & Current_Player.Score & vbCrLf)
                            Console.Write(vbCrLf & ">")
                            Console.ReadKey()
                            Level_Over = True
                        Else                            'checks for any invalid generation
                            Console.WriteLine("Invalid Boss/Treasure value generation")
                        End If
                    Case Else                           'gives appropriate error code based on the type of error
                        If i > Level_Array.Length() Then
                            Console.WriteLine("Invalid Count")
                        Else
                            Console.WriteLine("Invalid Room Generation")
                        End If
                End Select
                Dev_Skip = True
            Next
        Loop
        Level_Over = False
        Return False
    End Function

    'Pathway Split
    Function Path_Picker(ByRef Current_Player, ByRef Random_Text, ByRef Path_Choice)
        Console.WriteLine("")

        Select Case Random_Text
            Case 1
                Console.WriteLine(Current_Player.Char_Name & " walks past an unusual rock, finding themselves at a split in the path.")
            Case 2
                Console.WriteLine("The caverns split across a rock formation, leaving two paths in front of " & Current_Player.Char_Name)
            Case 3
                Console.WriteLine(Current_Player.Char_Name & " enters a large, open cavern, with two tunnels leading deeper before them.")
            Case 4
                Console.WriteLine("As " & Current_Player.Char_Name & " feels their way around the dark caverns, they notice the tunnel splits into two.")
            Case Else
                Console.WriteLine(Current_Player.Char_Name & " sees the pathway break in half, leaving them with two ways to go.")
        End Select

        Console.WriteLine("")
        Console.WriteLine("Which path should " & Current_Player.Char_Name & " take?" & vbCrLf)
        Console.Write("L/R > ")
        Path_Choice = Console.ReadLine.ToUpper()
        Select Case Path_Choice
            Case "L"
                Return True
            Case "R"
                Return True
            Case Else
                Return False
        End Select
    End Function

    'Monster Encounter Subs
    Sub Monster_Encounter(ByRef Current_Player, ByRef Level_No, ByVal Encounter_Type, ByRef intItemRowCount, ByRef intItemColumnCount, ByRef intSkillsRowCount, ByRef intSkillsColumnCount, ByVal Max_Player_Health)
        Dim Fight_Over As Boolean = False
        Dim Monster_Health As Integer
        Dim PDefense As Integer = 0
        Dim In_Range As Boolean = True
        Dim Enemy_Name As String
        Dim ECharge As Boolean = False
        Dim Defense_Power As Integer
        Dim Drop_Item As Char
        Monster_Health = Get_Monster_Health(Encounter_Type, Level_No)

        If Encounter_Type = "B" Then
            Enemy_Name = Get_Boss_Name()
        Else
            Enemy_Name = Get_Enemy_Name()
        End If
        Console.WriteLine(vbCrLf & Current_Player.Char_Name & " encounters a " & Enemy_Name & "!" & vbCrLf)
        Console.Write(vbCrLf & ">")
        Console.ReadKey()

        Do Until Fight_Over = True
            Fight_Over = Player_Turn(Current_Player, Level_No, Encounter_Type, intItemRowCount, intItemColumnCount, Monster_Health, intSkillsRowCount, intSkillsColumnCount, PDefense, In_Range, Defense_Power, Enemy_Name)
            If Fight_Over = True Then
                Exit Do
            Else
                Defense_Power = 0

            End If
            If Encounter_Type = "M" Then
                Fight_Over = Enemy_Turn(Current_Player, Level_No, Max_Player_Health, PDefense, In_Range, Enemy_Name, Defense_Power)
            ElseIf Encounter_Type = "B" Then
                Fight_Over = Boss_Turn(Current_Player, Level_No, Max_Player_Health, PDefense, In_Range, Defense_Power, Enemy_Name, Monster_Health, ECharge)
            End If
            PDefense = Current_Player.Defense_Power
            In_Range = True
        Loop
        If Current_Player.Player_Health > 0 Then
            Console.WriteLine(Current_Player.Char_Name & " slayed the monster!")
            Console.WriteLine(Current_Player.Char_Name & "'s health is " & Current_Player.Player_Health)
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
            Dim Loot As String() = Loot_Drop(Current_Player, Level_No, Encounter_Type)
            Try
                Console.WriteLine("")
                Console.WriteLine("")
                If intItemRowCount = 9 And Loot(0) <> "" Then
                    Console.WriteLine("The enemy dropped a " & Loot(0))
                    Console.WriteLine("Inventory Full!")
                    Console.WriteLine("Would you like to destroy an item?")
                    Console.Write(vbCrLf & "Y/N >")
                    Drop_Item = Console.ReadLine.ToUpper()
                ElseIf Loot(0) = "" Then
                    Console.WriteLine("The enemy did not drop any loot" & vbCrLf)
                    Console.WriteLine("Would you like to destroy an item?")
                    Console.Write(vbCrLf & "Y/N >")
                    Drop_Item = Console.ReadLine.ToUpper()
                Else
                    Console.WriteLine("The enemy dropped a " & Loot(0))

                    For i = 0 To intItemColumnCount - 1
                        Current_Player.Inventory(intItemRowCount, i) = Loot(i)
                    Next
                    intItemRowCount += 1
                End If
            Catch ex As Exception
                Console.WriteLine("Invalid choice. " & Current_Player.Char_Name & " does not get to destroy an item...")
            End Try
            If Drop_Item = "Y" Then
                Remove_Item(Current_Player, intItemRowCount, intItemColumnCount)
            End If
            If Encounter_Type = "B" Then
                Console.WriteLine("")
                Console.WriteLine("With the boss defeated, " & Current_Player.Char_Name & " loots the treasure that it was protecting!")
                Console.WriteLine("")
                Current_Player.Score += 10
            Else
                Console.WriteLine("")
                Console.WriteLine(Current_Player.Char_Name & " continues deeper into the inky depths of the dungeon..." & vbCrLf)
                Console.Write(">")
                Console.ReadKey()
                If Level_No <= 5 Then
                    Current_Player.Score += 2
                ElseIf Level_No <= 10 Then
                    Current_Player.Score += 4
                Else
                    Current_Player.Score += 6
                End If
            End If
        End If
    End Sub
    '   Loot drop at end of fight
    Function Loot_Drop(Current_Player, Level_No, Encounter_Type)
        Dim Loot_Number As Integer
        Const Total_Items As Integer = 5
        Dim Loot(4) As String
        Loot_Number = CInt(Math.Floor(10) * Rnd()) + 1
        If Loot_Number > Total_Items Then               'allows for items to not always drop
        ElseIf Encounter_Type = "B" Then                'checks if the fight was a boss battle
            Loot = Get_Loot_Boss(Loot_Number)
        ElseIf Level_No >= 5 Then                       'checks if the level number is five or more
            If Loot_Number > Total_Items Then
                Loot_Number = 3
            End If
            Loot = Get_Loot_High(Loot_Number)
        Else
            Loot = Get_Loot_Low(Loot_Number)
        End If
        Return Loot
    End Function
    Function Get_Loot_Boss(Loot_Number)
        Dim Full_File As String
        Using Reader As StreamReader = New StreamReader("Loot Boss.txt")
            Full_File = Reader.ReadToEnd()
        End Using
        Dim Split_Items() As String = Full_File.Split(";")
        Dim Item() As String = Split_Items(Loot_Number).Split(",")
        Return Item
    End Function
    Function Get_Loot_Low(Loot_Number)
        Dim Full_File As String
        Using Reader As StreamReader = New StreamReader("Loot Low.txt")
            Full_File = Reader.ReadToEnd()
        End Using
        Dim Split_Items() As String = Full_File.Split(";")
        Dim Item() As String = Split_Items(Loot_Number).Split(",")
        Return Item
    End Function
    Function Get_Loot_High(Loot_Number)
        Dim Full_File As String
        Using Reader As StreamReader = New StreamReader("Loot High.txt")
            Full_File = Reader.ReadToEnd()
        End Using
        Dim Split_Items() As String = Full_File.Split(";")
        Dim Item() As String = Split_Items(Loot_Number).Split(",")
        Return Item
    End Function

    '   These All Relate To Player Turns
    Function Player_Turn(ByRef Current_Player, ByVal Level_No, ByVal Encounter_Type, ByRef intItemRowCount, ByRef intItemColumnCount, ByRef Monster_Health, ByRef intSkillsRowCount, ByRef intSkillsColumnCount, ByRef PDefense, ByRef In_Range, ByRef Defense_Power, ByVal Enemy_Name)
        'variables to store inputs from the player
        Dim PChoice As Integer
        Dim Choice_Valid As Boolean = False
        Dim Fight_Over As Boolean = False

        Dim Item_Option As Integer
        Dim Item_Used As Boolean = False
        Dim Quit_Item_Menu_Value As Integer
        Dim Player_Run As Boolean = False

        Dim Available_Skills() As String
        Dim Skill_Option As Integer
        Dim Skill_Choice_Valid As Boolean = False

        Randomize()

        Do Until Choice_Valid = True
            Try
                PChoice = Get_Player_Choice()
                Select Case PChoice
                    Case 1 'player attacks
                        Choice_Valid = True
                        Player_Attack(Current_Player, Monster_Health, Level_No, Defense_Power)
                        If Monster_Health <= 0 Then
                            Return True
                        Else
                            Return False
                        End If
                    Case 2 'player defends
                        Choice_Valid = True
                        Console.WriteLine(Current_Player.Char_Name & " prepares to defend...")
                        Console.Write(vbCrLf & vbCrLf & ">")
                        Console.ReadKey()
                        PDefense = Current_Player.Defense_Power * Level_No - Level_No
                        Choice_Valid = True
                        Return False
                    Case 3 'player uses an ability
                        Dim No_Of_Skills As Integer = 0
                        For i = 0 To intSkillsRowCount - 1 Step 1
                            If Current_Player.Skills(i, 4) > 0 And Current_Player.Skills(i, 1) = "ABL" Then
                                No_Of_Skills += 1
                            End If
                        Next
                        If No_Of_Skills >= 1 Then
                            Do Until Skill_Choice_Valid = True
                                Available_Skills = Output_Skills_Available(Current_Player.Skills, intSkillsRowCount)
                                Skill_Option = Console.ReadLine()
                                If Skill_Option = 10 Then
                                    Skill_Choice_Valid = True
                                ElseIf Skill_Option > Available_Skills.Length - 1 Or Skill_Option < 0 Then
                                    Console.WriteLine("Invalid choice, please input value of a skill you wish to use.")
                                Else
                                    Skill_Choice_Valid = True
                                    Choice_Valid = Ability_Use(Current_Player, Level_No, Encounter_Type, Monster_Health, PDefense, In_Range, Available_Skills, Skill_Option, Defense_Power)
                                    If Monster_Health <= 0 And Choice_Valid = True Then
                                        Return True
                                    ElseIf Choice_Valid = False Then
                                    Else
                                        Return False
                                    End If
                                End If
                            Loop
                        Else
                            Console.WriteLine("No skills unlocked!")
                            Console.Write(vbCrLf & ">")
                            Console.ReadKey()
                        End If
                        Skill_Choice_Valid = False
                    Case 4 'player choses to use an item
                        Quit_Item_Menu_Value = Item_Array_Output(Current_Player, intItemRowCount)
                        Console.WriteLine("To use an item, type the item number")
                        Console.Write(vbCrLf & ">")
                        Item_Option = Console.ReadLine()
                        If Item_Option <> Quit_Item_Menu_Value Then
                            Choice_Valid = Use_Item_Fight(Current_Player, Item_Option, intItemRowCount, intItemColumnCount, Monster_Health)
                            If Current_Player.Player_Health <= 0 Or Monster_Health <= 0 Then
                                Fight_Over = True
                            Else
                                Fight_Over = False
                            End If
                            If Choice_Valid = True Then
                                Return Fight_Over
                            End If
                        End If

                    Case 5 'player chooses to run
                        If Encounter_Type = "B" And Level_No > 5 Then
                            Console.WriteLine("Can't flee a boss fight!")
                            Console.WriteLine("The boss growls at " & Current_Player.Char_Name)
                            Console.WriteLine("")
                            Choice_Valid = True
                            Return False
                        ElseIf Encounter_Type = "B" And Level_No < 5 Then
                            Console.WriteLine("Can't flee a boss fight!")
                            Console.WriteLine("The boss doesn't notice " & Current_Player.Char_Name & "'s blunder...")
                        Else
                            Player_Run = Escape(Player_Run, Level_No, Current_Player.Char_Name)
                            If Player_Run = True Then
                                Return True
                            Else
                                Choice_Valid = True
                            End If
                        End If

                    Case Else
                        Console.WriteLine("Invalid Choice")
                End Select
            Catch ex As Exception
                Console.WriteLine("Please input an integer between 1 and 5")
                Console.Write(vbCrLf & ">")
                Console.ReadKey()
            End Try

        Loop
        Choice_Valid = False
    End Function
    '       Player using skill in combat
    Function Output_Skills_Available(ByRef Skills, ByRef intSkillsRowCount)
        Dim Skill_Number As Integer = 0
        Dim Available_Skills(7) As String
        Dim Empty_Array(0) As String
        Console.WriteLine(vbCrLf)
        Console.WriteLine("--------------------")
        Console.WriteLine("-------SKILLS-------")
        Console.WriteLine("--------------------")
        For i = 0 To intSkillsRowCount - 1 Step 1
            If Skills(i, 1) = "ABL" Then
                If Skills(i, 3) = Skills(i, 4) Then
                    Console.WriteLine(Skill_Number & ")" & Skills(i, 0))
                    Available_Skills(Skill_Number) = Skills(i, 0)
                    Skill_Number += 1
                End If
            End If
        Next
        Console.WriteLine("10)Quit")
        Console.WriteLine("--------------------")
        Console.WriteLine("")
        Console.WriteLine(vbCrLf & "Input a skills' number to use that skill")
        Console.Write(vbCrLf & "> ")
        If Skill_Number = 0 Then
            Console.WriteLine("No abilities unlocked")
            Return Empty_Array
        Else
            Return Available_Skills
        End If
    End Function
    Function Ability_Use(ByRef Current_Player, ByVal Level_No, ByVal Encounter_Type, ByRef Monster_Health, ByRef PDefense, ByRef In_Range, ByRef Available_Skills, ByVal Skill_Option, ByVal Defense_Power)
        Dim Skill_Picked As String
        Dim Use_Skill As Boolean = False
        Dim temp As Boolean = False
        Dim Player_Attack As Integer

        Const CA_Mana As Integer = 5
        Const DS_Mana As Integer = 6
        Const RA_Mana As Integer = 10
        Const DT_Mana As Integer = 20
        Const SA_Mana As Integer = 15

        Randomize()
        Skill_Picked = Available_Skills(Skill_Option)
        Console.WriteLine("Your current mana is: " & Current_Player.Mana_Amount)
        Select Case Skill_Picked
            Case "Charge Attack"
                Use_Skill = Skill_Mana_Cost(Skill_Picked, CA_Mana, Current_Player.Mana_Amount)
                If Use_Skill = True Then
                    Player_Attack = CInt(Math.Floor(Level_No + 2) * Rnd()) + 5
                    Player_Attack -= Defense_Power
                    Current_Player.Mana_Amount -= CA_Mana
                    Console.WriteLine(Current_Player.Char_Name & " charges at the enemy!")
                    Console.WriteLine("Deals " & Player_Attack & " points of damage." & vbCrLf)
                    Monster_Health -= Player_Attack
                    Console.WriteLine("Monster health is now " & Monster_Health)
                    Console.Write(vbCrLf & ">")
                    Console.ReadKey()
                    Return True
                Else
                    Return False
                End If
            Case "Defended Strike"
                Use_Skill = Skill_Mana_Cost(Skill_Picked, DS_Mana, Current_Player.Mana_Amount)
                If Use_Skill = True Then
                    Player_Attack = CInt(Math.Floor(Level_No) * Rnd())
                    Player_Attack -= Defense_Power
                    PDefense = Current_Player.Defense_Power * Level_No - Level_No
                    Current_Player.Mana_Amount -= DS_Mana
                    Console.WriteLine(Current_Player.Char_Name & " slashes at the enemy from a defensive position")
                    Console.WriteLine("Deals " & Player_Attack & " points of damage." & vbCrLf)
                    Monster_Health -= Player_Attack
                    Console.WriteLine("Monster health is now " & Monster_Health)
                    Console.Write(vbCrLf & ">")
                    Console.ReadKey()
                    Return True
                Else
                    Return False
                End If
            Case "Ranged Attack"
                Use_Skill = Skill_Mana_Cost(Skill_Picked, RA_Mana, Current_Player.Mana_Amount)
                If Use_Skill = True Then
                    Player_Attack = CInt(Math.Floor(Level_No + 1) * Rnd()) + 1
                    Player_Attack -= Defense_Power
                    In_Range = False
                    Current_Player.Mana_Amount -= RA_Mana
                    Console.WriteLine(Current_Player.Char_Name & " attacks the enemy before darting away to a safe spot")
                    Console.WriteLine("Deals " & Player_Attack & " points of damage." & vbCrLf)
                    Monster_Health -= Player_Attack
                    Console.WriteLine("Monster health is now " & Monster_Health)
                    Console.Write(vbCrLf & ">")
                    Console.ReadKey()
                    Return True
                Else
                    Return False
                End If
            Case "Double Hit"
                Use_Skill = Skill_Mana_Cost(Skill_Picked, DT_Mana, Current_Player.Mana_Amount)
                If Use_Skill = True Then
                    Player_Attack = CInt(Math.Floor(Level_No * 2) * Rnd()) + 1
                    Player_Attack -= Defense_Power
                    Current_Player.Mana_Amount -= DT_Mana
                    Console.WriteLine(Current_Player.Char_Name & " attacks the enemy twice in quick succession")
                    Console.WriteLine("Deals " & Player_Attack & " points of damage." & vbCrLf)
                    Monster_Health -= Player_Attack
                    Console.WriteLine("Monster health is now " & Monster_Health)
                    Console.Write(vbCrLf & ">")
                    Console.ReadKey()
                    Return True
                Else
                    Return False
                End If
            Case "Slap Attack"
                Use_Skill = Skill_Mana_Cost(Skill_Picked, SA_Mana, Current_Player.Mana_Amount)
                If Use_Skill = True Then
                    Player_Attack = Slap_Attack(Current_Player.Attack_Power)
                    Player_Attack -= Defense_Power
                    Current_Player.Mana_Amount -= SA_Mana
                    Console.WriteLine(Current_Player.Char_Name & " slaps the enemy " & Current_Player.Attack_Power & " times!")
                    Console.WriteLine("Deals " & Player_Attack & " points of damage." & vbCrLf)
                    Monster_Health -= Player_Attack
                    Console.WriteLine("Monster health is now " & Monster_Health)
                    Console.Write(vbCrLf & ">")
                    Console.ReadKey()
                    Return True
                Else
                    Return False
                End If
            Case Else
                Console.WriteLine("Error in Skill_Picked or Available_Skills")
                Return False
        End Select
    End Function
    Function Skill_Mana_Cost(ByRef Skill_Picked, Ability_Mana, ByRef Mana_Total)
        Dim Use_Skill_Confirmed As Boolean = False
        Dim Activate_Skill As Boolean = False
        Console.WriteLine("Mana cost:" & Ability_Mana)
        If Ability_Mana > Mana_Total Then
            Console.WriteLine("Don't have enough mana!")
            Return False
        ElseIf Ability_Mana = Mana_Total Then
            Console.WriteLine("This will use all of your mana")
        Else
            Console.WriteLine("Your remaining mana will be " & (Mana_Total - Ability_Mana))
        End If
        Do Until Use_Skill_Confirmed = True
            Try
                Activate_Skill = Get_Use_Skill(Skill_Picked)
                Use_Skill_Confirmed = True
            Catch ex As Exception
                Console.WriteLine("Invalid input")
            End Try
        Loop
        If Activate_Skill = True Then
            Return True
        Else
            Return False
        End If
    End Function
    Function Get_Use_Skill(ByRef Skill_Picked)
        Dim Confirm_Choice As Char
        Console.WriteLine("Are you sure you wish to use " & Skill_Picked & "? Y/N: ")
        Confirm_Choice = Console.ReadLine.ToUpper()
        If Confirm_Choice = "Y" Then
            Return True
        ElseIf Confirm_Choice = "N" Then
            Console.WriteLine("Skill use cancelled")
            Return False
        Else
            Console.WriteLine("Invalid character input")
            Return False
        End If
    End Function
    Function Slap_Attack(Attack)
        If Attack = 1 Then
            Return 1
        Else
            Attack = Slap_Attack(Attack - 1) + Attack
            Return Attack
        End If
    End Function
    '       Other player options.
    Sub Player_Attack(ByRef Current_Player, ByRef Monster_Health, ByVal Level_No, ByVal Defense_Power)
        Dim PDamage As Integer
        Dim PHit As Integer

        Console.WriteLine(Current_Player.Char_Name & " attacks!")
        Console.Write(vbCrLf & ">")
        Console.ReadKey()
        PDamage = CInt(Math.Floor(Level_No + 2) * Rnd()) + Current_Player.Attack_Power + 1   'calculates how much damage the player will deal
        PDamage -= Defense_Power
        PHit = CInt(Math.Floor(99) * Rnd()) + 1                                              'determines whether the player will hit the enemy
        If PHit > 25 And PHit <= 100 Then                                                    'based on the calculated hit value, determines if the player hits the enemy
            Console.WriteLine(Current_Player.Char_Name & " hits! Deals " & PDamage & " points of damage.")
            Monster_Health = Monster_Health - PDamage                                        'subtracts the damage the player dealt from the monster's total health
            Console.WriteLine("Monster health is now " & Monster_Health)
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
        ElseIf PHit <= 25 Or PHit > 100 Then
            Console.WriteLine(Current_Player.Char_Name & " misses! Tough luck.")
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
        Else
            Console.WriteLine("Invalid Hit value generation")
        End If
    End Sub
    Function Get_Player_Choice()
        Dim PChoice As Integer
        Dim Menu_Stop As Boolean = False
        Do Until Menu_Stop = True
            Fight_Menu()
            Try
                PChoice = Console.ReadLine()
                If PChoice <= 5 And PChoice > 0 Then
                    Menu_Stop = True
                Else
                    Console.WriteLine("Invalid option chosen")
                End If
            Catch ex As Exception
                Console.WriteLine("Non-integer input")
            End Try
        Loop
        Return PChoice
    End Function
    Sub Fight_Menu()
        Console.WriteLine("")
        Console.WriteLine("-------------")
        Console.WriteLine("----FIGHT----")
        Console.WriteLine("-------------")
        Console.WriteLine("(1) Attack")
        Console.WriteLine("(2) Defend")
        Console.WriteLine("(3) Ability")
        Console.WriteLine("(4) Use Item")
        Console.WriteLine("(5) Run")
        Console.WriteLine("-------------")
        Console.WriteLine("")
        Console.WriteLine("")
        Console.Write("Choice > ")
    End Sub
    Function Escape(ByRef Player_Run, ByVal Level_No, ByVal Char_Name)
        Player_Run = CInt(Math.Floor(3) * Rnd())
        If Player_Run = 0 Then
            Console.WriteLine(Char_Name & " successfully gets past the monster!")
            Console.WriteLine("")
            Return True
        Else
            Console.WriteLine("The monster blocks " & Char_Name & "'s path!")
            Console.WriteLine("")
            Return False
        End If
    End Function
    Function Use_Item_Fight(ByRef Current_Player, ByVal Item_Number, ByRef intItemRowCount, ByRef intItemColumnCount, ByRef Monster_Health)
        If Current_Player.Inventory(Item_Number, 3) = "HEL" Then
            Console.WriteLine(Current_Player.Char_Name & " uses their " & Current_Player.Inventory(Item_Number, 0) & " to restore their health")
            Current_Player.Player_Health += Current_Player.Inventory(Item_Number, 4)
            Console.WriteLine(Current_Player.Char_Name & "'s health is now " & Current_Player.Player_Health)
            Console.WriteLine("")
            Delete_Item(Current_Player, intItemColumnCount, intItemRowCount, Item_Number)
            Return True
        ElseIf Current_Player.Inventory(Item_Number, 3) = "DMG" Then
            Console.WriteLine(Current_Player.Char_Name & " throws their " & Current_Player.Inventory(Item_Number, 0) & ", damaging the monster!")
            Monster_Health -= Current_Player.Inventory(Item_Number, 4)
            Delete_Item(Current_Player, intItemColumnCount, intItemRowCount, Item_Number)
            Return True
        ElseIf Current_Player.Inventory(Item_Number, 3) = "ITM" Then
            Console.WriteLine("That isn't going to help you here..." & vbCrLf)
            Delete_Item(Current_Player, intItemColumnCount, intItemRowCount, Item_Number)
            Return False
        Else
            Console.WriteLine("Error in item type")
            Return False
        End If
    End Function

    '   These All Relate To Monster Turns
    Function Get_Monster_Health(ByRef Encounter_Type, ByRef Level_No)
        Dim Monster_Health As Integer
        Dim Random As New Random
        If Encounter_Type = "B" Then
            Monster_Health = Random.Next(1.5 * (Level_No) ^ 2, 2.6 * (Level_No) ^ 2)    'adjusts the monster health calculation so that bosses are harder
        ElseIf Encounter_Type = "M" Then
            Monster_Health = Random.Next(2 * Level_No, 3 * Level_No / 1.1)          'calculates the monster's health so it grows with the level no.
        Else
            Console.WriteLine("Invalid Encounter Type")
            Monster_Health = 10
        End If
        Return Monster_Health
    End Function
    Function Enemy_Turn(ByRef Current_Player, ByVal Level_No, ByVal Max_Player_Health, ByVal PDefense, ByVal In_Range, ByVal Enemy_Name, ByRef Defense_Power)
        Dim Possible_Moves() As String = {"Basic Attack", "Basic Attack", "Basic Attack", "Basic Attack", "Basic Attack",
                                          "Basic Defend", "Basic Defend",
                                          "Level Dependant",
                                          "Hug Attack"}
        Dim Total_Random As Integer
        Dim Random As New Random()
        Total_Random = CInt(Math.Floor(Possible_Moves.Length() - 1) * Rnd())
        Dim Chosen_Move As String = Possible_Moves(Total_Random)

        Dim Attack_Power As Integer = 0

        Select Case Chosen_Move
            Case "Basic Attack"
                Attack_Power = Random.Next(3 * Level_No / 4, 2 * Level_No)
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " attacks!")
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")

            Case "Basic Defend"
                Defense_Power = Random.Next(3 * Level_No / 4, Level_No)
                Console.WriteLine(Enemy_Name & " defends...")
                Console.Write(vbCrLf & ">")
                Console.ReadKey()
                Return False

            Case "Level Dependant"
                If Level_No < 5 Then
                    Console.WriteLine(Enemy_Name & " gazes around the cavern aimlessly...")
                    Console.Write(vbCrLf & ">")
                    Console.ReadKey()
                    Return False

                ElseIf Level_No < 10 Then
                    Attack_Power = Random.Next(Level_No, 3 * Level_No)
                    Attack_Power -= PDefense
                    Console.WriteLine(Enemy_Name & " gathers a fireball and hurls it at " & Current_Player.Char_Name & "!")
                    Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")
                Else
                    Attack_Power = Random.Next(4 * Level_No / 3, 3 * Level_No)
                    Attack_Power -= PDefense
                    Console.WriteLine(Enemy_Name & " kicks " & Current_Player.Char_Name & "'s legs!")
                    Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")
                End If

            Case "Hug Attack"
                Attack_Power = Random.Next(Level_No / 2, Level_No)
                Console.WriteLine(Enemy_Name & " wraps themselves around " & Current_Player.Char_Name & "!")
                Console.WriteLine(vbCrLf & "The hug deals " & Attack_Power & " points of damage.")

            Case Else
                Console.WriteLine("Error in Possible Moves or Chosen Move")

        End Select
        Current_Player.Player_Health -= Attack_Power
        Console.WriteLine(vbCrLf & "Your health is now " & Current_Player.Player_Health)
        Console.Write(vbCrLf & ">")
        Console.ReadKey()

        If Current_Player.Player_Health <= 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Function Boss_Turn(ByRef Current_Player, ByVal Level_No, ByVal Encounter_Type, ByVal PDefense, ByVal In_Range, ByRef Defense_Power, ByVal Enemy_Name, ByRef Monster_Health, ByRef ECharge)
        Dim Possible_Moves() As String = {"Basic Attack", "Basic Attack", "Basic Attack", "Basic Attack", "Basic Attack",
                                          "Basic Attack", "Basic Attack", "Basic Attack",
                                          "Energy Pulse", "Energy Pulse", "Energy Pulse", "Energy Pulse",
                                          "Kick", "Kick", "Kick",
                                          "Charge", "Charge", "Headbutt", "Headbutt",
                                          "Stomp"}
        Dim Total_Random As Integer
        Dim Random As New Random()
        Total_Random = CInt(Math.Floor(Possible_Moves.Length() - 1) * Rnd())
        Dim Chosen_Move As String = Possible_Moves(Total_Random)

        Dim Attack_Power As Integer = 0

        If ECharge = True Then
            Chosen_Move = "Charged Attack"
        End If
        Select Case Chosen_Move
            Case "Basic Attack"
                Attack_Power = Random.Next(3 * Level_No / 2, 3 * Level_No)
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " attacks!")
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")

            Case "Energy Pulse"
                Attack_Power = Random.Next(3 * Level_No / 2, 2 * Level_No)
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " shoots a pulse of energy at " & Current_Player.Char_Name)
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")

            Case "Kick"
                Attack_Power = Random.Next(2 * Level_No, 2 * Level_No + Level_No / 2)
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " slams into " & Current_Player.Char_Name & "'s legs!")
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")

            Case "Charge"

                Console.WriteLine(Enemy_Name & " fills itself with energy...")
                ECharge = True
                Return False

            Case "Charged Attack"
                Attack_Power = Random.Next(2 * Level_No, 4 * Level_No)
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " unleashes a massive attack!")
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage!")
                ECharge = False

            Case "Headbutt"
                Attack_Power = Random.Next(2 * Level_No, 4 * Level_No - (Level_No / 3))
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " charges at " & Current_Player.Char_Name & ", smashing their head into them!")
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage!")

            Case "Stomp"
                Attack_Power = Random.Next(2 * Level_No, 2 * Level_No + Level_No / 2)
                Attack_Power -= PDefense
                Console.WriteLine(Enemy_Name & " stomps on " & Current_Player.Char_Name & " from above!")
                Console.WriteLine(vbCrLf & "Deals " & Attack_Power & " points of damage.")

        End Select
        Current_Player.Player_Health -= Attack_Power
        Console.WriteLine(vbCrLf & "Your health is now " & Current_Player.Player_Health)
        Console.Write(vbCrLf & ">")
        Console.ReadKey()

        If Current_Player.Player_Health <= 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Function Get_Enemy_Name()
        Dim Random_Name As Integer
        Dim Names() As String = {"Green Slime", "Blue Slime", "Red Slime", "Yellow Slime", "Grey Slime",
                                "Crumbling Skeleton", "Skeleton Soldier", "Gnasher",
                                "Syntax Error", "Blue Salamander", "Red Salamander", "Pink Salamander",
                                "Minion 205", "Tiny Robots",
                                "Stabby Shape",
                                "Flink"}
        Random_Name = CInt(Math.Floor(Names.Length() - 1) * Rnd())
        Return Names(Random_Name)
    End Function
    Function Get_Boss_Name()
        Dim Random_Name As Integer
        Dim Names() As String = {"Noor", "Sammi", "Tetris Piece", "Moltron", "Salaman",
                                 "Jinx", "Deathdar", "Radox", "Black Spade",
                                 "Carnage", "Error 404",
                                 "Metal Slime",
                                 "Skeleton General"}
        Random_Name = CInt(Math.Floor(Names.Length() - 1) * Rnd())
        Return Names(Random_Name)
    End Function

    'All Relate to Trap Rooms
    Function Trap(ByRef Current_Player, ByVal Level_No, ByRef intItemRowCount, ByRef intItemColumnCount)
        Randomize()                                                             'seeds the random function
        'trap variables
        Dim Trap_Type As Integer                                                'stores the type of trap
        Dim Turns_Remaining As Integer
        'player variables
        Dim Player_Option As Integer
        Dim Player_Choice As Boolean = False
        Dim Player_Escaped As Boolean = False
        Dim Item_Use_Choice As Boolean
        Dim Item_Number As Integer
        If Level_No > 8 Then
            Turns_Remaining = CInt(Math.Floor(Level_No - 3) * Rnd()) + 1
            If Turns_Remaining > 10 Then
                Turns_Remaining -= 3
            End If
        Else
            Turns_Remaining = CInt(Math.Floor(10 - Level_No)) + 2
        End If

        Randomize()
        Trap_Type = CInt(Math.Floor((4) * Rnd()))                                'generates the type of trap based on the level no
        'Trap_Type = 0                                                           'for testing purposes
        Select Case Trap_Type
            Case 0                                                               'Water trap type
                Console.WriteLine("A door slams shut in front of and behind " & Current_Player.Char_Name & " and gushing fills the chamber.")
                Console.WriteLine("The room is filling with water! " & Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                Console.WriteLine("")
                Console.WriteLine("======================================================" & vbCrLf & vbCrLf)
                Console.Write(">")
                Console.ReadKey()
                Do Until Turns_Remaining = 0 Or Player_Escaped = True            'loops until the player runs out of time or they escape
                    Do Until Player_Choice = True

                        Try
                            Trap_Menu()                                              'outputs the menu of options for the player
                            Player_Option = Console.ReadLine()
                            Select Case Player_Option
                                Case 1                                               'outputs the trap type and a solution of how to solve it
                                    Console.WriteLine("The trap is of type: Water")
                                    Console.WriteLine("Solution could be to block entry point of water." & vbCrLf)
                                    Player_Choice = True
                                Case 2                                               'allows the player to use an item from their bag
                                    Item_Use_Choice = Item_Pick(Current_Player, Item_Number, intItemRowCount, intItemColumnCount)
                                    If Item_Use_Choice = True Then
                                        Player_Choice = Use_Item_Trap(Current_Player, Player_Escaped, Item_Number, intItemRowCount, intItemColumnCount, Trap_Type)
                                    End If
                                Case 3                                              'calls the door breaking function
                                    Player_Escaped = Door_Breaker(Current_Player, Player_Escaped, Level_No)
                                    Player_Choice = True
                                Case 4                                              'player can waste a turn in despair
                                    Console.WriteLine(Current_Player.Char_Name & " despairs at their situation.")
                                    Player_Choice = True
                                Case Else                                           'catches any invalid choices the player makes
                                    Console.WriteLine("Please input a number between 1 and 4")
                            End Select
                        Catch ex As Exception
                            Console.WriteLine("")
                            Console.WriteLine("Please input a number")
                            Console.WriteLine("")
                        End Try
                    Loop
                    Player_Choice = False                                       'sets the player choice to false ready for the next turn
                    If Player_Escaped = False And Turns_Remaining > 0 Then                              'continues counting down the player's timer unless they have escaped
                        Turns_Remaining -= 1
                        Console.WriteLine(Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                    ElseIf Turns_Remaining = 0 Then
                        Console.WriteLine(Current_Player.Char_Name & " dies in the trap!")
                        Return True
                    Else
                        Console.WriteLine(Current_Player.Char_Name & " escapes the trap.")
                        Console.WriteLine("")
                        Console.ReadKey()
                        Return False
                    End If
                Loop
            Case 1                                                              'Earth trap type
                Console.WriteLine("A door slams shut in front of and behind " & Current_Player.Char_Name & " and rumbling fills the chamber.")
                Console.WriteLine("The room is collapsing! " & Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                Console.WriteLine("")
                Console.WriteLine("======================================================" & vbCrLf & vbCrLf)
                Console.Write(">")
                Console.ReadKey()
                Do Until Turns_Remaining = 0 Or Player_Escaped = True            'loops until the player runs out of time or they escape
                    Do Until Player_Choice = True
                        Try
                            Trap_Menu()                                              'outputs the menu of options for the player
                            Player_Option = Console.ReadLine()
                            Select Case Player_Option
                                Case 1                                               'outputs the trap type and a solution of how to solve it
                                    Console.WriteLine("The trap is of type: Earth")
                                    Console.WriteLine("Solution could be to stop the walls closing in." & vbCrLf)
                                    Player_Choice = True
                                Case 2                                               'allows the player to use an item from their bag
                                    Item_Use_Choice = Item_Pick(Current_Player, Item_Number, intItemRowCount, intItemColumnCount)
                                    If Item_Use_Choice = True Then
                                        Player_Choice = Use_Item_Trap(Current_Player, Player_Escaped, Item_Number, intItemRowCount, intItemColumnCount, Trap_Type)
                                    End If
                                Case 3                                              'calls the door breaking function
                                    Player_Escaped = Door_Breaker(Current_Player, Player_Escaped, Level_No)
                                    Player_Choice = True
                                Case 4                                              'player can waste a turn in despair
                                    Console.WriteLine(Current_Player.Char_Name & " despairs at their situation.")
                                    Player_Choice = True
                                Case Else                                           'catches any invalid choices the player makes
                                    Console.WriteLine("Please input a number between 1 and 4")
                            End Select
                        Catch ex As Exception
                            Console.WriteLine("")
                            Console.WriteLine("Please input a number")
                            Console.WriteLine("")
                        End Try                                             'outputs the menu of options for the player
                    Loop
                    Player_Choice = False                                       'sets the player choice to false ready for the next turn
                    If Player_Escaped = False And Turns_Remaining > 0 Then                              'continues counting down the player's timer unless they have escaped
                        Turns_Remaining -= 1
                        Console.WriteLine(Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                    ElseIf Turns_Remaining = 0 Then
                        Console.WriteLine(Current_Player.Char_Name & " dies in the trap!")
                        Return True
                    Else
                        Console.WriteLine(Current_Player.Char_Name & " escapes the trap.")
                        Console.WriteLine("")
                        Console.ReadKey()
                        Return False
                    End If
                Loop
            Case 2                                                              'Fire trap type
                Console.WriteLine("A door slams shut in front of and behind " & Current_Player.Char_Name & " fire shoots out of the floor around " & Current_Player.Char_Name & "!")
                Console.WriteLine("The room is on fire! " & Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                Console.WriteLine("")
                Console.WriteLine("======================================================" & vbCrLf & vbCrLf)
                Console.Write(">")
                Console.ReadKey()
                Do Until Turns_Remaining = 0 Or Player_Escaped = True            'loops until the player runs out of time or they escape
                    Do Until Player_Choice = True
                        Try
                            Trap_Menu()                                              'outputs the menu of options for the player
                            Player_Option = Console.ReadLine()
                            Select Case Player_Option
                                Case 1                                               'outputs the trap type and a solution of how to solve it
                                    Console.WriteLine("The trap is of type: Fire")
                                    Console.WriteLine("Solution could be to put the fire out" & vbCrLf)
                                    Player_Choice = True
                                Case 2                                               'allows the player to use an item from their bag
                                    Item_Use_Choice = Item_Pick(Current_Player, Item_Number, intItemRowCount, intItemColumnCount)
                                    If Item_Use_Choice = True Then
                                        Player_Choice = Use_Item_Trap(Current_Player, Player_Escaped, Item_Number, intItemRowCount, intItemColumnCount, Trap_Type)
                                    End If
                                Case 3                                              'calls the door breaking function
                                    Player_Escaped = Door_Breaker(Current_Player, Player_Escaped, Level_No)
                                    Player_Choice = True
                                Case 4                                              'player can waste a turn in despair
                                    Console.WriteLine(Current_Player.Char_Name & " despairs at their situation.")
                                    Player_Choice = True
                                Case Else                                           'catches any invalid choices the player makes
                                    Console.WriteLine("Please input a number between 1 and 4")
                            End Select
                        Catch ex As Exception
                            Console.WriteLine("")
                            Console.WriteLine("Please input a number")
                            Console.WriteLine("")
                        End Try
                    Loop
                    Player_Choice = False                                       'sets the player choice to false ready for the next turn
                    If Player_Escaped = False And Turns_Remaining > 0 Then                              'continues counting down the player's timer unless they have escaped
                        Turns_Remaining -= 1
                        Console.WriteLine(Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                    ElseIf Turns_Remaining = 0 Then
                        Console.WriteLine(Current_Player.Char_Name & " dies in the trap!")
                        Return True
                    Else
                        Console.WriteLine(Current_Player.Char_Name & " escapes the trap.")
                        Console.WriteLine("")
                        Console.ReadKey()
                        Return False
                    End If
                Loop
                If Turns_Remaining = 0 Then
                    Console.WriteLine(Current_Player.Char_Name & " burns alive!")
                End If
            Case 3                                                              'Electric trap type
                Console.WriteLine("A door slams shut in front of and behind " & Current_Player.Char_Name & " as electrical sparks fly!")
                Console.WriteLine("The room is electrified! " & Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live." & vbCrLf)
                Console.WriteLine("")
                Console.WriteLine("======================================================" & vbCrLf & vbCrLf)
                Console.Write(">")
                Console.ReadKey()
                Do Until Turns_Remaining = 0 Or Player_Escaped = True            'loops until the player runs out of time or they escape
                    Do Until Player_Choice = True
                        Try
                            Trap_Menu()                                              'outputs the menu of options for the player
                            Player_Option = Console.ReadLine()
                            Select Case Player_Option
                                Case 1                                               'outputs the trap type and a solution of how to solve it
                                    Console.WriteLine("The trap is of type: Electric")
                                    Console.WriteLine("Solution could be to stop the current." & vbCrLf)
                                    Player_Choice = True
                                Case 2                                               'allows the player to use an item from their bag
                                    Item_Use_Choice = Item_Pick(Current_Player, Item_Number, intItemRowCount, intItemColumnCount)
                                    If Item_Use_Choice = True Then
                                        Player_Choice = Use_Item_Trap(Current_Player, Player_Escaped, Item_Number, intItemRowCount, intItemColumnCount, Trap_Type)
                                    End If
                                Case 3                                              'calls the door breaking function
                                    Player_Escaped = Door_Breaker(Current_Player, Player_Escaped, Level_No)
                                    Player_Choice = True
                                Case 4                                              'player can waste a turn in despair
                                    Console.WriteLine(Current_Player.Char_Name & " despairs at their situation.")
                                    Player_Choice = True
                                Case Else                                           'catches any invalid choices the player makes
                                    Console.WriteLine("Please input a number between 1 and 4")
                            End Select
                        Catch ex As Exception
                            Console.WriteLine("")
                            Console.WriteLine("Please input a number")
                            Console.WriteLine("")
                        End Try
                    Loop
                    Player_Choice = False                                       'sets the player choice to false ready for the next turn
                    If Player_Escaped = False And Turns_Remaining > 0 Then                              'continues counting down the player's timer unless they have escaped
                        Turns_Remaining -= 1
                        Console.WriteLine(Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                    ElseIf Turns_Remaining = 0 Then
                        Console.WriteLine(Current_Player.Char_Name & " dies in the trap!")
                        Return True
                    Else
                        Console.WriteLine(Current_Player.Char_Name & " escapes the trap.")
                        Console.WriteLine("")
                        Console.ReadKey()
                        Return False
                    End If
                Loop
                If Turns_Remaining = 0 Then
                    Console.WriteLine(Current_Player.Char_Name & " dies from electric shock!")
                End If
            Case 4                                                              'Gas trap type
                Console.WriteLine("A door slams shut in front of and behind " & Current_Player.Char_Name & " as a hissing noise fills the room")
                Console.WriteLine("The room is filling with gas! " & Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live." & vbCrLf)
                Console.WriteLine("")
                Console.WriteLine("======================================================" & vbCrLf & vbCrLf)
                Console.Write(">")
                Console.ReadKey()
                Do Until Turns_Remaining = 0 Or Player_Escaped = True
                    Console.WriteLine("")
                    Do Until Player_Choice = True
                        Try
                            Trap_Menu()
                            Player_Option = Console.ReadLine()
                            Select Case Player_Option
                                Case 1
                                    Console.WriteLine("The trap is of type: Gas")
                                    Console.WriteLine("Solution could be to block the gas pipe." & vbCrLf)
                                    Player_Choice = True
                                Case 2
                                    Item_Use_Choice = Item_Pick(Current_Player, Item_Number, intItemRowCount, intItemColumnCount)
                                    If Item_Use_Choice = True Then
                                        Player_Choice = Use_Item_Trap(Current_Player, Player_Escaped, Item_Number, intItemRowCount, intItemColumnCount, Trap_Type)
                                    End If
                                Case 3
                                    Player_Escaped = Door_Breaker(Current_Player, Player_Escaped, Level_No)
                                    Player_Choice = True
                                Case 4
                                    Console.WriteLine(Current_Player.Char_Name & " despairs at their situation.")
                                    Player_Choice = True
                                Case Else                                           'catches any invalid choices the player makes
                                    Console.WriteLine("Please input a number between 1 and 4")
                            End Select
                        Catch ex As Exception
                            Console.WriteLine("")
                            Console.WriteLine("Please input a number")
                            Console.WriteLine("")
                        End Try
                        Turns_Remaining -= 1
                        If Player_Escaped = False And Turns_Remaining > 0 Then                              'continues counting down the player's timer unless they have escaped
                            Turns_Remaining -= 1
                            Console.WriteLine(Current_Player.Char_Name & " has " & Turns_Remaining & " turns to live.")
                        ElseIf Turns_Remaining = 0 Then
                            Console.WriteLine(Current_Player.Char_Name & " dies in the trap!")
                            Return True
                        Else
                            Console.WriteLine(Current_Player.Char_Name & " escapes the trap.")
                            Console.WriteLine("")
                            Console.ReadKey()
                            Return False
                        End If
                    Loop
                Loop
            Case Else                                                           'Error Catch
                Console.WriteLine("Error in Trap type generation. Fix code.")
        End Select
    End Function
    Function Use_Item_Trap(ByRef Current_Player, ByRef Player_Escaped, ByVal Item_Number, ByRef intItemRowCount, ByRef intItemColumnCount, ByVal Trap_Type)
        If Current_Player.Inventory(Item_Number, 3) = "ITM" Then
            If Current_Player.Inventory(Item_Number, 4) = Trap_Type Then
                Console.WriteLine(Current_Player.Char_Name & " uses their " & Current_Player.Inventory(Item_Number, 0) & " to escape the trap!")
                Player_Escaped = True
                Delete_Item(Current_Player, intItemColumnCount, intItemRowCount, Item_Number)
                Return True
            Else
                Console.WriteLine(Current_Player.Char_Name & " wastes their " & Current_Player.Inventory(Item_Number, 0) & " and is still stuck!" & vbCrLf)
                Delete_Item(Current_Player, intItemColumnCount, intItemRowCount, Item_Number)
                Return True
            End If
        Else
            Console.WriteLine("You can't use that in a trap!")
            Return False
        End If
    End Function
    Function Door_Breaker(ByRef Current_Player, ByVal Player_Escaped, ByRef Level_No)
        Dim Door_Break As Integer
        Console.WriteLine("")
        Console.WriteLine(Current_Player.Char_Name & " attempts to break down the door...")
        If Level_No < 10 Then                                                   'changes the chance of breaking the door based on the level number
            Door_Break = CInt(Math.Floor(99) * Rnd()) + 1
        ElseIf Level_No < 20 Then
            Door_Break = CInt(Math.Floor(64) * Rnd()) + 1
        Else                                                                    'If the player is too far through the game, they cannot break the door
            Console.WriteLine("The door is far too thick for " & Current_Player.Char_Name & " to break!")
            Door_Break = 1
        End If
        If Door_Break = 1 Then                                                  'gives the player different messages and escape based on their door break value
            Console.WriteLine(Current_Player.Char_Name & " hurts themselves trying to break the door.")
            Console.WriteLine("")
            Current_Player.Player_Health -= 2
            Console.WriteLine(Current_Player.Char_Name & "'s health is now " & Current_Player.Player_Health)
            Console.WriteLine("")
        ElseIf Door_Break < 50 Then
            Console.WriteLine(Current_Player.Char_Name & " did not manage to open the door.")
            Console.WriteLine("")
        ElseIf Door_Break >= 50 Then
            Console.WriteLine("")
            Console.WriteLine(Current_Player.Char_Name & " successfully breaks down the door!")
            Console.WriteLine("")
            Player_Escaped = True
        Else
            Console.WriteLine("Error in door break generation")
        End If
        Return Player_Escaped
    End Function
    Sub Trap_Menu()
        Console.WriteLine("")
        Console.WriteLine("----------------------")
        Console.WriteLine("---------TRAP---------")
        Console.WriteLine("----------------------")
        Console.WriteLine("(1)Examine Trap")
        Console.WriteLine("(2)Search through bag")
        Console.WriteLine("(3)Smash door")
        Console.WriteLine("(4)Despair")
        Console.WriteLine("----------------------" & vbCrLf)
        Console.Write("Choice > ")
    End Sub

    'Ends of Things
    Function Base_Encoder(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemRowCount, intItemColumnCount, Level_No)
        Dim String_Set As String = ""
        String_Set = Current_Player.Char_Name & ":" & Current_Player.Score & ":"
        For i = 0 To intSkillsRowCount - 1
            For j = 0 To intSkillsColumnCount - 1
                If i = intSkillsRowCount - 1 Then
                    If j = intSkillsColumnCount - 1 Then
                        String_Set &= Current_Player.Skills(i, j)
                    End If
                Else
                    If j = intSkillsColumnCount - 1 Then
                        String_Set &= Current_Player.Skills(i, j) & ";"
                    Else
                        String_Set &= Current_Player.Skills(i, j) & ","
                    End If
                End If
            Next
        Next
        String_Set &= ":" & Current_Player.Skill_Points & ":"
        For i = 0 To intItemRowCount - 1
            For j = 0 To intItemColumnCount - 1
                If i = intItemRowCount - 1 Then
                    If j = intItemColumnCount - 1 Then
                        String_Set &= Current_Player.Inventory(i, j)
                    Else
                        String_Set &= Current_Player.Inventory(i, j) & ","
                    End If
                Else
                    If j = intItemColumnCount - 1 Then
                        String_Set &= Current_Player.Inventory(i, j) & ";"
                    Else
                        String_Set &= Current_Player.Inventory(i, j) & ","
                    End If
                End If
            Next
        Next
        String_Set &= ":" & Current_Player.Player_Health & ":" & Current_Player.Mana_Amount & ":" & Current_Player.Attack_Power & ":" & Current_Player.Defense_Power
        String_Set &= ":" & Level_No & ":" & intItemRowCount
        Dim Byte_Array As Byte() = Encoding.UTF8.GetBytes(String_Set)

        Dim Converted_String As String = Convert.ToBase64String(Byte_Array)
        Return Converted_String

    End Function
    Function Login_Save(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemRowCount, intItemColumnCount)
        Dim Username As String
        Dim Password_Attempt As String
        Dim File As String
        Dim Salt As String = ""
        Dim Password As String
        Dim Found As Boolean = False
        Dim Hash_Pass As String
        Dim Exit_Loop As Boolean = False
        Dim New_Attempt As Char
        Do Until Exit_Loop = True
            Console.Write("Please input your username: ")
            Username = Console.ReadLine()
            Console.Write(vbCrLf & "Please input your password: ")
            Password_Attempt = Get_Password()

            Using StreamReader As New StreamReader("Potato.txt")
                File = StreamReader.ReadToEnd().Trim(" ")
            End Using

            Dim Rows() As String = File.Split(";")
            For i = 0 To Rows.Length - 1 Step 1
                Dim Single_Row() As String = Rows(i).Split(":")
                If Single_Row(0) = Username Then
                    Salt = Single_Row(1)
                    Password = Single_Row(2)
                    Found = True
                    Exit For
                End If
            Next

            If Found = True Then
                Hash_Pass = Hash_512(Password_Attempt, Salt)

                If Hash_Pass = Password Then
                    Console.WriteLine(vbCrLf & "You have logged in")
                    Console.ReadKey()
                    Return Username
                Else
                    Console.WriteLine(vbCrLf & "Invalid Password")
                    Console.ReadKey()
                    Console.WriteLine("Would you like to try again?")
                    Console.Write(vbCrLf & "Y/N > ")
                    New_Attempt = Console.ReadLine().ToUpper()
                    If New_Attempt = "N" Then
                        Exit_Loop = True
                        Return "DidNotLogin"
                    End If
                End If

            Else
                Console.WriteLine(vbCrLf & "User not found")
                Console.ReadKey()
                Console.WriteLine("Would you like to try again?")
                Console.Write(vbCrLf & "Y/N > ")
                New_Attempt = Console.ReadLine().ToUpper()
                If New_Attempt = "N" Then
                    Exit_Loop = True
                    Return "DidNotLogin"
                End If
            End If
        Loop
    End Function
    Sub Logout_User(User, Data)
        Dim path As String = User
        Dim FS As FileStream = File.Create(path & ".txt")
        FS.Close()
        Using FileStream As New StreamWriter(path & ".txt")
            FileStream.Write(Data)
        End Using
    End Sub
    Function End_Of_Level(ByRef Current_Player, ByRef intSkillsRowCount, ByRef intSkillsColumnCount, ByRef Level_No, ByVal intItemRowCount, ByVal intItemColumnCount, ByRef Saved_Game)                'Returns whether the player wishes to continue playing or exit game
        Dim Continue_Loop As Boolean = True
        Dim Continue_Playing As Char                'Accepts the users input on whether to continue or not
        Dim Save As Char
        Do Until Continue_Loop = False
            Console.WriteLine("Would you like to start a new level?")
            Console.Write(vbCrLf & "Y/N >")
            Try                                     'catches any non-char inputs the user enters
                Continue_Playing = Console.ReadLine.ToUpper()
                Select Case Continue_Playing
                    Case "Y"                        'Returns that the player wishes to continue playing
                        Use_Skill_Points(Current_Player, Level_No, intSkillsRowCount, intSkillsColumnCount)
                        Console.WriteLine("Generating new level...")
                        Console.Write(vbCrLf & ">")
                        Console.ReadKey()
                        Console.WriteLine(vbCrLf & vbCrLf & vbCrLf & vbCrLf)
                        Continue_Loop = False
                        Return False
                    Case "N"                        'Allows the player to quit the game
                        Console.WriteLine("Would you like to save?")
                        Console.Write(vbCrLf & "Y/N >")
                        Save = Console.ReadLine().ToUpper()
                        Select Case Save
                            Case "Y"
                                Dim Data As String = Base_Encoder(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemRowCount, intItemColumnCount, Level_No)
                                Dim User As String = Login_Save(Current_Player, intSkillsRowCount, intSkillsColumnCount, intItemRowCount, intItemColumnCount)
                                Console.WriteLine(vbCrLf & "Exiting game...")
                                Logout_User(User, Data)
                                Saved_Game = True
                                Continue_Loop = False
                                Return True
                            Case "N"
                                Console.WriteLine("Ending run...")
                                Console.Write(vbCrLf & ">")
                                Console.ReadKey()
                                Console.WriteLine(vbCrLf & vbCrLf & vbCrLf & vbCrLf)
                                Continue_Loop = False
                                Return True
                            Case Else
                                Console.WriteLine("Please input Y or N")
                        End Select

                    Case Else                       'catches any invalid inputs that are still characters
                        Console.WriteLine("")
                        Console.WriteLine("Invalid input, please use Y/N")
                End Select
            Catch ex As Exception                   'catches all non-character inputes the user attempts to enter
                Console.WriteLine("Invalid non-char input")
            End Try
        Loop
    End Function
    Sub End_Of_Game(Current_Player, Level_No, intItemRowCount)
        Dim Scores(5) As Double                    'stores the scores that will be read in from file
        Dim Score_On_Leaderboard As Boolean = False
        Dim Temp As Integer                         'serves as a temporary variable for sorting
        Console.WriteLine(Current_Player.Char_Name & "'s score was: " & Current_Player.Score)
        Console.WriteLine("GAME OVER")
        Console.Write(vbCrLf & ">")
        Console.ReadKey()
        Using Reader As StreamReader = New StreamReader("Scoreboard_File.txt") 'accesses the file holding the scoreboard
            For i = 0 To 4 Step 1                   'cycles through the text file
                Scores(i) = Reader.ReadLine
            Next
        End Using                                   'closes the file
        Scores(5) = Current_Player.Score
        For i = 0 To 5 Step 1                       'uses the bubble sort method to sort the scores into order with the players score on it
            For j = 0 To 4 Step 1
                If (Scores(j) < Scores(j + 1)) Then
                    Temp = Scores(j + 1)
                    Scores(j + 1) = Scores(j)
                    Scores(j) = Temp
                End If
            Next
        Next
        For i = 0 To 4 Step 1                       'detects whether the player made it onto the scoreboard
            If Scores(i) = Current_Player.Score Then
                Score_On_Leaderboard = True         'if the player got onto the scoreboard, this tells the program to congratulate them later
            End If
        Next
        Using Writer As StreamWriter = New StreamWriter("Scoreboard_File.txt") 'writes the new scoreboard to the file
            For i = 0 To 4 Step 1
                Writer.WriteLine(Scores(i))
            Next
        End Using
        If Score_On_Leaderboard = True Then         'detects whether the player has made it onto the leaderboard
            Console.WriteLine("")
            Console.WriteLine("You made it onto the leaderboard!")
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
            Console.WriteLine("")
            Console.WriteLine("-------------------")
            Console.WriteLine("----LEADERBOARD----")
            Console.WriteLine("-------------------")
            For i = 0 To 4 Step 1               'outputs the new leaderboard with the player on it
                Console.WriteLine("Score " & (i + 1) & ") " & Scores(i))
            Next
            Console.WriteLine("-------------------")
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
        Else
            Console.WriteLine("")
            Console.WriteLine("You didn't make it onto the leaderboard, tough luck!")
            Console.Write(vbCrLf & ">")
            Console.ReadKey()
        End If
        Console.WriteLine(vbCrLf & vbCrLf & vbCrLf & vbCrLf)
    End Sub
End Module



