/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    // Permitir pegar cualquier contenido.
    config.allowedContent = true;
    config.language = 'es';

    config.filebrowserUploadUrl = '/upload.aspx';
    //config.filebrowserBrowseUrl = 'http://media.spainbs.com/recursos_www/recursos_campana/';
    config.skin = 'office2013';

    config.scayt_autoStartup = true;
    config.scayt_sLang = 'es_ES';
    config.wsc_lang = 'es_ES';

    config.extraPlugins = 'emoji';
};
