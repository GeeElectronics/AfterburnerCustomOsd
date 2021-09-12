Imports MSI.Afterburner
Imports MSI.Afterburner.Exceptions
Imports System.Drawing



Public Class Form1
    Dim active As Boolean = True
    Dim hwi As New HardwareMonitor
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


    Private Function SetComplementary() As Bitmap
        Dim bmp As New Bitmap(Me.Size.Width, Me.Size.Height)
        Graphics.FromImage(bmp).CopyFromScreen(Me.DesktopLocation, New Point(0, 0), Me.Size)
        TextBox2.ForeColor = Color.FromArgb(bmp.GetPixel(Size.Width / 2, Size.Height / 2).ToArgb() Xor &HFFFFFF)

        Return bmp
    End Function


    Private Sub Aktualisieren()
        Try


            SetComplementary()
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

    Private Sub TabPage1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Aktualisieren()
    End Sub

    Private Sub Form1_MouseHover(sender As Object, e As EventArgs) Handles MyBase.MouseHover

    End Sub

    Private Sub Form1_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Form1_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated

        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        If (DesktopLocation.X > -32000) Then
            Me.SetDesktopLocation(DesktopLocation.X - 7, DesktopLocation.Y - 30)
            Me.ControlBox = True
            active = True
        End If
    End Sub

    Private Sub Form1_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        'Me.ControlBox = False

        If (DesktopLocation.X > -32000) Then
            Me.SetDesktopLocation(DesktopLocation.X + 7, DesktopLocation.Y + 30)
        End If
        active = False

    End Sub

    Private Sub Form1_Leave(sender As Object, e As EventArgs) Handles MyBase.Leave
        'Me.ControlBox = False
        'active = False
    End Sub

    Private Sub TextBox2_Click(sender As Object, e As EventArgs) Handles TextBox2.Click


    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        My.Settings.X = DesktopLocation.X
        My.Settings.Y = DesktopLocation.Y
        My.Settings.Save()
    End Sub
End Class
