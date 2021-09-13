Imports MSI.Afterburner
Imports MSI.Afterburner.Exceptions
Imports System.Drawing



Public Class Form1

    Dim hwi As New HardwareMonitor
    Dim averagecolortimer As Integer = 5
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If (My.Settings.X = 0) Then
        Else

            Me.SetDesktopLocation(My.Settings.X, My.Settings.Y)
        End If
        Timer1.Enabled = True

    End Sub
    Private Sub Rohdaten()
        Try




            Dim Result As String = ""

            Result = hwi.Header.ToString().Replace(";", Environment.NewLine)
            For Each item As HardwareMonitorEntry In hwi.Entries
                Result &= Environment.NewLine & "#####################################" & Environment.NewLine
                Result &= item.ToString().Replace(";", Environment.NewLine)
            Next


        Catch ex As Exception

        End Try
    End Sub


    Private Function getAverageColor(ByVal bmp As Bitmap) As Color
        Dim totalR As Integer = 0
        Dim totalG As Integer = 0
        Dim totalB As Integer = 0
        For x As Integer = 0 To bmp.Width - 1
            For y As Integer = 0 To bmp.Height - 1
                Dim pixel As Color = bmp.GetPixel(x, y)
                totalR += pixel.R
                totalG += pixel.G
                totalB += pixel.B
            Next
        Next
        Dim totalPixels As Integer = bmp.Height * bmp.Width
        Dim averageR As Integer = totalR \ totalPixels
        Dim averageg As Integer = totalG \ totalPixels
        Dim averageb As Integer = totalB \ totalPixels
        Return Color.FromArgb(averageR, averageg, averageb)
    End Function


    Private Function SetComplementary() As Bitmap
        Dim bmp As New Bitmap(Me.Size.Width, Me.Size.Height)

        Graphics.FromImage(bmp).CopyFromScreen(Me.DesktopLocation, New Point(0, 0), Me.Size)

        Dim averagecolor As Color = getAverageColor(bmp)
        TextBox2.ForeColor = Color.FromArgb(averagecolor.ToArgb() Xor &HFFFFFF)

        Return bmp
    End Function


    Private Sub Aktualisieren()
        Try

            If (averagecolortimer > 5) Then
                SetComplementary()
                averagecolortimer = 0
            Else
                averagecolortimer += 1
            End If



            hwi.ReloadAll()
            Dim Result As String = ""
            Dim Elements As Integer = 0
            For Each item As HardwareMonitorEntry In hwi.Entries


                If (item.Flags.ToString().Contains("LCD")) Then
                    Result &= item.LocalizedSrcName & ":  " & item.Data.ToString("0") & " " & item.LocalizedSrcUnits & Environment.NewLine
                    Elements += 1
                End If

            Next


            Dim newsize As Integer = (Elements * 25) + 0
            If (DesktopLocation.X > -32000) Then
                Me.ClientSize = New System.Drawing.Size(Me.ClientSize.Width, newsize)
            End If
            TextBox2.Text = Result

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Aktualisieren()
    End Sub

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated

        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        If (DesktopLocation.X > -32000) Then
            Me.SetDesktopLocation(DesktopLocation.X - 7, DesktopLocation.Y - 30)

        End If
    End Sub

    Private Sub Form1_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None


        If (DesktopLocation.X > -32000) Then
            Me.SetDesktopLocation(DesktopLocation.X + 7, DesktopLocation.Y + 30)
        End If


    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        My.Settings.X = DesktopLocation.X
        My.Settings.Y = DesktopLocation.Y
        My.Settings.Save()
    End Sub
End Class
