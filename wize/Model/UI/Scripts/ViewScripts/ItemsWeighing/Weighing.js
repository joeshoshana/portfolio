var current;
$(document).ready(function () {

    Shared();
    GetItems();
    current.GUID = $("#item_id").text();
});

function GetItems() {
    $.ajax({
        type: 'POST',
        url: '/Items/Get',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(current) }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                current = new Current(data.data);
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Current(data) {
    this.GUID = data.GUID;
    this.SN = data.SN;
    this.Name = data.Name;
    this.Active = data.Active;
    this.CompanyID = data.CompanyID;
}