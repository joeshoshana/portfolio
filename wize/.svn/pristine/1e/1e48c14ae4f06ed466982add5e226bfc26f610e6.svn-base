
$(document).ready(function () {

    Shared();

    CreateFilterEvents();

    $(".row").on('click', '.item', function () {
        window.location.href = '/ItemsWeighing/Weighing/' + $(this).val();
    });

    GetItems();
});

function GetItems()
{
    $.ajax({
        type: 'POST',
        url: '/Items/Get',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(new Filter()) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var items = $("#items");
                items.empty();
                $.each(data.data, function (i, val) {
                    items.append(BuildCard(val));
                });
                
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function BuildCard(data)
{
    var a = $('<a class="card text-white royal_blue justify-content-center item"/>')
        .val(data.GUID).append($('<h1 class="card-title text-center justify-content-center"/>').text(data.Name))
        .append($('<h4 class="card-title text-center justify-content-center"/>').text(data.SN));
    return a;
}

function Filter() {
    this.Name = $("#f_name").val();
    this.SN = $("#f_sn").val();
    this.Active = true;
}

function CreateFilterEvents() {
    $("#f_name").on('input', function () {
        GetItems();
    });

    $("#f_sn").on('input', function () {
        GetItems();
    });
}