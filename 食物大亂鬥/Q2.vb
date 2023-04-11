Public Class Q2
    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim w As Integer
        w = MsgBox("恭喜你答對了!!! 可以放置食物砲台了喔", 0, "訊息")
        If w = 1 Then
            Select Case Label1.Text
                Case = "1"
                    Form1.allstart()
                Case = "2"
                    第二關.allstart()
                Case = "3"
                    第三關.allstart()
            End Select
            Me.Close()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click, Button3.Click
        Dim w As Integer
        w = MsgBox("你答錯了QQ 會被扣十塊喔~", 0, "訊息")
        If w = 1 Then
            Select Case Label1.Text
                Case = "1"
                    Form1.moneyLB.Text = (CInt(Form1.moneyLB.Text) - 10).ToString '扣錢
                    Form1.allstart()
                Case = "2"
                    第二關.moneyLB.Text = (CInt(第二關.moneyLB.Text) - 10).ToString '扣錢
                    第二關.allstart()
                Case = "3"
                    第三關.moneyLB.Text = (CInt(第三關.moneyLB.Text) - 10).ToString '扣錢
                    第三關.allstart()
            End Select
            Me.Close()
        End If
    End Sub
End Class