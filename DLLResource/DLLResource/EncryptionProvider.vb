Imports System.Text
Imports System.Security.Cryptography
Imports System.IO

Public Class EncryptionProvider

    ''' <summary>
    ''' This function encrypts and hashes a password
    ''' </summary>
    ''' <param name="str">A string of password</param>
    ''' <returns>Encrypted password</returns>
    ''' <remarks></remarks>
    Public Function Encrypt(ByVal str As String) As String
        Dim b() = ASCIIEncoding.Default.GetBytes(str)
        'define key and iv
        Dim k() As Byte = {10, 2, 5, 1, 16, 14, 11, 24, 3, 4, 9, 8, 6, 7, 13, 12, 17, 19, 18, 20, 15, 21, 23, 22}
        Dim iv() As Byte = {10, 1, 5, 3, 8, 6, 2, 9}

        'use 3DES provider
        Using provider As New TripleDESCryptoServiceProvider()
            'write into memory stream
            Using memory As New MemoryStream()
                'encrypt the string
                Using cs As New CryptoStream(memory,
                                             provider.CreateEncryptor(k, iv),
                                             CryptoStreamMode.Write)
                    'write the cryptostream to memory stream
                    cs.Write(b, 0, b.Length)
                    cs.FlushFinalBlock()

                    'hashes encrypted values with MD5
                    Dim pass() As Byte = ASCIIEncoding.Default.GetBytes(BitConverter.ToString(memory.ToArray()))
                    Dim md5 As MD5 = New MD5CryptoServiceProvider()

                    Return BitConverter.ToString(md5.ComputeHash(pass))

                End Using
            End Using
        End Using

        Return Nothing
    End Function


End Class
