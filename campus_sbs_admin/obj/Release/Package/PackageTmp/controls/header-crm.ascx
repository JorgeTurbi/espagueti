<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="header-crm.ascx.cs" Inherits="campus_sbs_admin.controls.header_crm" %>
<div>
    <button type="button" class="navbar-toggle" data-toggle="offcanvas" data-side="left" data-target="#nav-container" data-canvas="body">
	    <span class="icon-bar"></span>
		<span class="icon-bar"></span>
		<span class="icon-bar"></span>
	</button>
    <div class="row">
        <div class="col-5 offset-2 col-sm-5 offset-sm-1 col-lg-6 offset-lg-0">
            <img src="/App_Themes/support/img/logo-sbs.png" alt="logo-sbs" class="logo" />
	    </div>   
        <div class="col-5 col-sm-6">
            <div id="teacher_user" class="col-6 col-sm-3 col-lg-2 padding-t-10 text-center" runat="server"></div>
            <div id="teacher_perfil" class="col-sm-5 col-lg-7 padding-t-15 hidden-xs" runat="server"></div>
            <div class="col-6 col-sm-3 float-right">
                <button id="exit" title="Salir" class="navbar-toggle-exit right" onclick="exit_user()">
                    <i class="fas fa-sign-out-alt fa-3x text-color-white"></i>
                </button>
            </div>
            <div id="teacher_warnings" class="col-1 col-sm-2 col-lg-1 hidden-xs" runat="server"></div>            
		</div>             
	</div>
</div>
<div class="fluid-container"><div class="bg-secundary padding-t-10"></div></div>