<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="header.ascx.cs" Inherits="campus_sbs_admin.controls.header" %>

<div>
    <button type="button" class="navbar-toggle" data-toggle="offcanvas" data-side="left" data-target="#nav-container" data-canvas="body">
	    <span class="icon-bar"></span>
		<span class="icon-bar"></span>
		<span class="icon-bar"></span>
	</button>	        		
    <div class="row">
        <div class="col-md-7 col-md-offset-0 col-sm-5 col-sm-offset-1 col-xs-6 col-xs-offset-1">
            <img src="/App_Themes/support/img/logo-sbs.png" alt="logo-sbs" class="logo" />
	    </div>   
        <div class="col-md-5 col-sm-6">    
            <div class="pull-right col-md-1 col-sm-1 col-xs-pull-1 padding-tb-10">
			    <button id="exit" title="Salir" class="navbar-toggle-exit right" onclick="exit_user()">
                    <i class="fas fa-sign-out-alt fa-3x text-color-white"></i>
                </button>
			</div>
            <div id="teacher_perfil" class="pull-right col-sm-8 col-sm-pull-1 col-md-7 col-md-pull-1 hidden-xs" runat="server"></div>
            <div id="teacher_user" class="pull-right col-sm-2 col-md-2 col-xs-pull-1 padding5" runat="server"></div>
            <div id="teacher_warnings" class="pull-right col-md-1 col-sm-1 col-xs-pull-2 padding-tb-10" runat="server"></div>			
		</div>             
	</div>
</div>
<div class="fluid-container"><div class="bg-secundary padding-t-10"></div></div>