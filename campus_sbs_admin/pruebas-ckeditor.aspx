<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pruebas-ckeditor.aspx.cs" Inherits="campus_sbs_admin.pruebas_ckeditor" %>

<!DOCTYPE html>
<html>
        <head>
                <meta charset="utf-8">
                <title>CKEditor</title>
                <%--<script src="https://cdn.ckeditor.com/4.16.2/full/ckeditor.js"></script>--%>
            <script src="/Content/ckeditor/ckeditor.js"></script>
        </head>
        <body>
                <textarea name="editor1"></textarea>
                <script>
                        CKEDITOR.replace( 'editor1' );
                </script>
        </body>
</html>