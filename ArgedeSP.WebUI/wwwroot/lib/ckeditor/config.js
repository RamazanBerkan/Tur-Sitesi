/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

    // Simplify the dialog windows.
    config.removeDialogTabs = 'image:advanced;link:advanced';

    config.removeDialogTabs = 'image:advanced;image:Link;link:advanced;link:upload';
    config.filebrowserImageUploadUrl = '/Admin/DosyaIslemleri/CFUpload?dosyaAdi=' + dosyaAdi; //Action for Uploding image  
    config.filebrowserBrowseUrl = '/Admin/DosyaIslemleri/Index?dosyaAdi=' + dosyaAdi;
    config.allowedContent = true;

};
