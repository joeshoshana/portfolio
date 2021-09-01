var current;
var weight = new Weight();
$(document).ready(function () {
    current = new Object();
    Shared();

    weight.WeightDisplay = $('#weight');

    LoadComboboxes();

    $("#items").combobox();

    $("#btnSave").on('click', function () {

        GetData();
        Save(current, false);
    });

    $("#btnWeigh").on('click', function () {
        var weight = $("#weight").text();
    });

});

function LoadComboboxes() {
    LoadItems();
}

function LoadItems() {
    $.ajax({
        type: 'POST',
        url: '/Items/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var items = $("#items");
                items.empty();
                items.append($("<option />").val(0).text(dictionary.SelectItem));
                $.each(data.data, function (i, val) {
                    items.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function GetData() {

    
}