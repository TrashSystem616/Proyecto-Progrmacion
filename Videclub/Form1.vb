Public Class Form1

    'Quienes Somos'
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        QnSomos.Show()
        Me.Hide()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = DateTime.Now.ToLongTimeString
    End Sub

    'Catalago'
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        formCatalago.Show()
        Me.Hide()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        FormCarrito.Show()
        Me.Hide()
    End Sub
End Class
