Imports System.Text.RegularExpressions
Imports System.Net
Imports System.IO
Imports Microsoft.Win32

Public Class UpdaterFrm
    Dim CurrentVersion As String = "0.0.0" '--- Change this to Current version, needs changing on every update
    Dim ProgramName As String = "BlueSheep" '--- Program Name
    Dim SiteName As String = "http://bluesheepbot.com/update.html" '--- Update Page
    Dim VersionCHK, GetVer, GetVerLink As String
    Dim GetUpd As Integer


    Private Sub UpdaterFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            '    StatusLb.Text = "Vérification des sécurités..."
            '    For Each Process In Diagnostics.Process.GetProcesses()
            '        'Si un des noms de processus correspond
            '        If Process.ProcessName.Contains("wampserver") Then
            '            Me.Close()
            '        ElseIf Process.ProcessName.Contains("wireshark") Then
            '            Me.Close()
            '            '    AlertBox1.Visible = True
            '            '    Close_form()
            '            'ElseIf Process.ProcessName.Contains("javaw") Then
            '            '    Me.Close()
            '        End If
            '    Next
            '    Pbar.Value = 10
            '    Dim chemin As String = Environment.GetEnvironmentVariable("windir") & "\system32\drivers\etc\hosts"
            '    If File.Exists(chemin) Then
            '        Dim X As System.IO.StreamWriter = New System.IO.StreamWriter(chemin)
            '        Dim Contenu As String = "127.0.0.1       localhost"
            '        X.WriteLine(Contenu)
            '        X.Close()
            '    End If
            Pbar.Value = 20
            Pbar.Value = 25
            If Not Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep") Then
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep")
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\BlueSheep.exe") Then
                AutoUpdate()
            Else
                Dim Thread As New Threading.Thread(AddressOf Download)
                Thread.Start()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Sub AutoUpdate()
        Dim WebRequest As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(SiteName)
        Dim WebResponse As System.Net.HttpWebResponse = WebRequest.GetResponse
        Dim STR As System.IO.StreamReader = New System.IO.StreamReader(WebResponse.GetResponseStream())
        Dim ReadSource As String = STR.ReadToEnd
        Dim Regex As New System.Text.RegularExpressions.Regex(ProgramName & "=(\d+).(\d+).(\d+(\,\d{1}))=(.*?).exe")
        Dim matches As MatchCollection = Regex.Matches(ReadSource)
        'Dim reg As RegistryKey
        Dim c As Single
        Dim reg As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\BlueSheep")
        'If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\version.bs") Then
        'If cs.Contains("Software\\BlueSheep") Then
        If reg IsNot Nothing Then
            'reg = Registry.CurrentUser.CreateSubKey("Software\BlueSheep")
            CurrentVersion = reg.GetValue("Version").ToString()
            c = reg.GetValue("Minor")
            'Dim reader As New StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\version.bs")
            'Dim BVer As String = reader.ReadLine()
            'Dim decByte As Byte() = Convert.FromBase64String(BVer)
            'CurrentVersion = System.Text.Encoding.ASCII.GetString(decByte)
            'reader.Close()
        Else
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\BlueSheep.exe")
            Dim Thread As New Threading.Thread(AddressOf Download)
            Thread.Start()
            Exit Sub
        End If
        For Each match As Match In matches
            Dim RegSplit() As String = Split(match.ToString, "=")
            GetVer = RegSplit(1)
            GetVerLink = RegSplit(2)
        Next
        Pbar.Value = 100
        Dim major As Integer = Convert.ToInt32(GetVer.Split(".")(0))
        Dim minor As Integer = Convert.ToInt32(GetVer.Split(".")(1))
        Dim release As Single = Convert.ToSingle(GetVer.Split(".")(2))
        Dim currentmajor As Integer
        Dim currentminor As Integer
        If (CurrentVersion.Contains(",")) Then
            currentmajor = Convert.ToInt32(CurrentVersion.Split(",")(0))
            currentminor = Convert.ToInt32(CurrentVersion.Split(",")(1))
        Else
            currentmajor = Convert.ToInt32(CurrentVersion.Split(".")(0))
            currentminor = Convert.ToInt32(CurrentVersion.Split(".")(1))
        End If
        Dim currentrelease As Single = c

        If major > currentmajor OrElse minor > currentminor OrElse release > currentrelease Then
            'My.Computer.Network.DownloadFile(GetVerLink, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & IO.Path.GetFileName(GetVerLink))
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\BlueSheep.exe")
            Dim Thread As New Threading.Thread(AddressOf Download)
            Thread.Start()
        Else
            client_DownloadCompleted(Nothing, Nothing)
        End If
    End Sub

    Private Sub client_ProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        Me.BeginInvoke(DirectCast(Sub() Pbar.Value = e.ProgressPercentage, MethodInvoker))
        Me.BeginInvoke(DirectCast(Sub() StatusLb.Text = "Téléchargement en cours... " & e.ProgressPercentage & " %", MethodInvoker))
    End Sub

    Private Sub client_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\BlueSheep.exe", "ok")
            Me.BeginInvoke(DirectCast(Sub() Me.Close(), MethodInvoker))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub dll_DownloadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)
        Dim Thread As New Threading.Thread(AddressOf Download)
        Thread.Start()
    End Sub

    Private Sub Download()
        Try
            If Not File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\MetroFramework.dll") Then
                Dim Thread As New Threading.Thread(AddressOf Dll)
                Thread.Start()
                Exit Sub
            End If
            Dim Client As WebClient = New WebClient
            AddHandler Client.DownloadProgressChanged, AddressOf client_ProgressChanged
            AddHandler Client.DownloadFileCompleted, AddressOf client_DownloadCompleted
            Client.DownloadFileAsync(New Uri("http://bluesheepbot.com/BlueSheep.exe"), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\BlueSheep.exe")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Dll()
        Dim Client As WebClient = New WebClient
        AddHandler Client.DownloadProgressChanged, AddressOf client_ProgressChanged
        Client.DownloadFileAsync(New Uri("http://bluesheepbot.com/MetroFramework.dll"), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\MetroFramework.dll")
        Dim Client2 As WebClient = New WebClient
        AddHandler Client2.DownloadProgressChanged, AddressOf client_ProgressChanged
        Client2.DownloadFileAsync(New Uri("http://bluesheepbot.com/MetroFramework.Design.dll"), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\MetroFramework.Design.dll")
        Dim Client3 As WebClient = New WebClient
        AddHandler Client3.DownloadProgressChanged, AddressOf client_ProgressChanged
        AddHandler Client3.DownloadFileCompleted, AddressOf dll_DownloadCompleted
        Client3.DownloadFileAsync(New Uri("http://bluesheepbot.com/MetroFramework.Fonts.dll"), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\BlueSheep\MetroFramework.Fonts.dll")
    End Sub
End Class
