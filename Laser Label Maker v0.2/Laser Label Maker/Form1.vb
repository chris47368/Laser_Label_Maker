Public Class Form1
    Dim presets As String() = {"Red 650nm", "Green 532nm", "Blue 445nm", "Blue 450nm", "Violet 405nm"}
    Dim preset_wavelenth As String() = {"650nm", "532nm", "445nm", "450nm", "405nm"}
    Dim output_power As Double = 5
    Dim power_units As String() = {"nW", "μW", "mW", "W", "KW", "MW", "GW", "TW"}
    Dim laser_class As String() = {"Class II Laser Product", "Class IIIb Laser Product", "Class IV Laser Product"}
    Dim power_unit_index As Integer = 2
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim wavelengths As Byte(,) = {{0, 0, 255, 255}, {0, 255, 0, 255}, {255, 0, 0, 255}, {240, 0, 0, 255}, {255, 0, 100, 255}}
        '                             0,0,255,255  0,255,0,255  255,0,0,255 240,0,0,255 255,0,100,255
        Dim match As Boolean = False
        For i As Integer = 0 To 4
            If ComboBox1.Text = presets(i) Then
                Dim b_value(3) As Byte
                For x As Integer = 0 To 3
                    b_value(x) = wavelengths(i, x)
                Next
                Panel1.BackColor = Color.FromArgb(BitConverter.ToInt32(b_value, 0))
                Wavelength_text.Text = preset_wavelenth(i)
            End If
        Next
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.TextLength > 0 Then
            Try
                output_power = TextBox1.Text
                OutputPower_text.Text = output_power.ToString & ComboBox2.Text
                If CheckBox2.Checked = True Then
                    OutputPower_text.Text = "<" & OutputPower_text.Text
                End If
            Catch ex As Exception

                TextBox1.Text = output_power
            End Try
        End If
        Change_Class()

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        OutputPower_text.Text = output_power.ToString & ComboBox2.Text
        If CheckBox2.Checked = True Then
            OutputPower_text.Text = "<" & OutputPower_text.Text
        End If
        For i As Integer = 0 To power_units.Length - 1
            If ComboBox2.Text = power_units(i) Then
                power_unit_index = i
            End If
        Next
        Change_Class()
    End Sub
    Sub Change_Class()
        If (output_power >= 500 And power_unit_index = 2) Or power_unit_index > 2 Then
            LaserClass_Text.Text = laser_class(2)
        ElseIf (output_power <= 5 And output_power > 1) And power_unit_index = 2 Then
            LaserClass_Text.Text = laser_class(1)
        ElseIf (output_power <= 1 And power_unit_index = 2) Or power_unit_index <= 2 Then
            LaserClass_Text.Text = laser_class(0)
        End If
    End Sub
    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            OutputPower_text.Text = "<" & OutputPower_text.Text
        Else
            OutputPower_text.Text = Mid(OutputPower_text.Text, 2)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveFileDialog1.Title = "Please Select a File"
        SaveFileDialog1.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*"
        SaveFileDialog1.FilterIndex = 1
        SaveFileDialog1.RestoreDirectory = True

        SaveFileDialog1.InitialDirectory = "C:temp"


        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk

        Try
            Using laser_label = New Bitmap(Panel1.Width, Panel1.Height)
                Panel1.DrawToBitmap(laser_label, New Rectangle(0, 0, laser_label.Width, laser_label.Height))
                laser_label.Save(SaveFileDialog1.FileName.ToString)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub SaveFileDialog1_Disposed(sender As Object, e As EventArgs) Handles SaveFileDialog1.Disposed
    End Sub
End Class
