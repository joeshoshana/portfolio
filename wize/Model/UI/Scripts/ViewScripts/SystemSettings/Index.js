var current;
$(document).ready(function () {

    Shared();
    Language();
    Dropzone.autoDiscover = false;
    $('#logo').dropzone({
        acceptedFiles: ".jpeg,.jpg,.png,.gif",
        uploadMultiple: false,
        createImageThumbnails: false,
        previewTemplate : '<div style="display:none"></div>',
        init: function () {



            this.on("complete", function (data) {

                console.log(data);
            });

            /* On Success, do whatever you want */
            this.on("success", function (file, responseText) {
                var str = '<img class="dz-message" src="' + responseText.Message + '" alt="Logo" style="max-width:100%;max-height:100%;">';
                $('#logo').empty();
                $('#logo').append(str);
            });

            this.on("error", function (file, responseText) {
                console.log(file);
                console.log(responseText);
            });
        }
    });

    $('#btnSave').on('click', function () {
        if (current === undefined)
            return;
        SetModalData();
        Save(current);
    });

    Settings();
});

function Save(obj) {

    $.ajax({
        type: 'POST',
        url: '/SystemSettings/Save',
        contentType: 'application/json; charset=utf-8',

        data: JSON.stringify({ data: JSON.stringify(obj)}),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                alert(dictionary.Saved);
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Settings() {
    $.ajax({
        type: 'POST',
        url: '/SystemSettings/Settings',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                current = data.data;
                var str = '';
                if (current.SystemLogoPath != null && current.SystemLogoPath.length > 0)
                {
                    str += '<img class="dz-message" src="' + current.SystemLogoPath + '" alt="Logo" style="max-width:100%;max-height:100%;">';
                    //str += '<img src="' + current.LogoPath + '" alt="Logo">';
                }
                else
                {
                    str += '<div class="dz-message h2 align-items-center">';
                    str += dictionary.AddLogo;
                    str += '</div>';
                }
                $('#logo').empty()
                $('#logo').append(str);
                GetModalData();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function GetModalData() {
    if (current !== undefined) {
        $("#language").val((current.LanguageID == null ? 0 : current.LanguageID));
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.LanguageID = $("#language").val();
    }
}

function Language() {
    $.ajax({
        type: 'POST',
        url: '/Home/Language',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var language = $("#language");
                language.empty();
                language.append($("<option />").val(0).text(dictionary.SelectLang));
                $.each(data.data, function (i, val) {
                    language.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}