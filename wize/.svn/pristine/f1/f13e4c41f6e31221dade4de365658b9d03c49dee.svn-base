var silos = [];
var current;
/*
var canvas = document.getElementById("canvas");
var ctx = canvas.getContext("2d");
var cw = canvas.width;
var ch = canvas.height;
ctx.lineCap = "round";

var y = ch - 10;
var drawingColor = "#0092f9";

var a = 2000;*/
var b = 2000;

$(document).ready(function () {

    Shared();
    
    CreateFilterEvents();

    
    LoadScales();
    LoadSilos();
    AllowedRows(silos.length);
    InvisibleFields(1);
    //   SiloProcess();

    $("#f_from_log_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $("#f_to_log_date").datepicker({
        dateFormat: "dd/mm/yy",
        todayBtn: "linked",
        clearBtn: true,
        language: 'he',
        autoclose: true
    });

    $('#btnNew').on('click', function (e) {
        current = new Object();
        GetModalData();
        $("#current").modal();
    });

    $('#btnSave').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();
        Save(current);
    });

    $('#btnLog').on('click', function (e) {
        if (current === undefined)
            return;
        SetModalData();
        Log(current);
    });
    
    //SetSilosScale('1');
    //
    //ActivateSilo('current');

    $('#silos_panel').on('click', '.silo-item', function (e) {
        current = silos[$(this).attr('id')].data
        GetModalData();
        $("#current").modal();
    });

    $('#f_silos_views').on('change', function ()
    {
        SetSilosScale($(this).val());
    });
});

function GetModalData() {
    if (current !== undefined) {
        $("#site_name_current").val(current.SiteName);
        $("#silo_name_current").val(current.Name);
        $("#silo_max_capacity_current").val(current.MaxCapacity);
        $("#scales").val(current.ScaleID);
        $("#silo_is_load_current").prop("checked", current.IsLoad);
        $("#silo_load_interval_current").val(current.LoadInterval);
        $("#silo_load_interval_time_current").val(current.LoadIntervalTime);
        $("#silo_is_unload_current").prop("checked", current.IsUnload);
        $("#silo_is_log_weight_current").prop("checked", current.IsLogWeight);
        $("#silo_unload_interval_current").val(current.UnloadInterval);
        $("#silo_unload_interval_time_current").val(current.UnloadIntervalTime);
        $("#silo_log_weight_time_current").val(current.LogWeightTime);
        
        $("#active").prop("checked", current.Active);
    }
}

function SetModalData() {
    if (current !== undefined) {
        current.SiteName = $("#site_name_current").val();
        current.Name = $("#silo_name_current").val();
        current.MaxCapacity = $("#silo_max_capacity_current").val();
        current.ScaleID = $("#scales").val();
        current.LoadInterval = $("#silo_load_interval_current").val();
        current.LoadIntervalTime = $("#silo_load_interval_time_current").val();
        current.UnloadInterval = $("#silo_unload_interval_current").val();
        current.UnloadIntervalTime = $("#silo_unload_interval_time_current").val();
        current.LogWeightTime = $("#silo_log_weight_time_current").val();        
        current.Active = $("#active").prop("checked");
        current.IsLoad = $("#silo_is_load_current").prop("checked");
        current.IsUnload = $("#silo_is_unload_current").prop("checked");
        current.IsLogWeight = $("#silo_is_log_weight_current").prop("checked");
    }
}

function LoadScales() {
    $.ajax({
        type: 'POST',
        url: '/Scales/Get',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ data: JSON.stringify(new Scale()) }),
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var scales = $("#scales");
                scales.empty();
                scales.append($("<option />").val(0).text(dictionary.SelectScales));
                $.each(data.data, function (i, val) {
                    scales.append($("<option />").val(val.GUID).text(val.Name));
                });
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }
    });
}

function Log(obj) {

    if (obj.Active == false)
        obj.ScaleID = null;

    if (obj.IsLoad == true) {
        if (obj.LoadInterval.length == 0 || obj.LoadIntervalTime.length == 0) {
            alert(dictionary.FillLoadData);
            return;
        }
    }
    else {
        obj.LoadInterval = null;
        obj.LoadIntervalTime = null;
    }

    if (obj.IsUnload == true) {
        if (obj.UnloadInterval.length == 0 || obj.UnloadIntervalTime.length == 0) {
            alert(dictionary.FillUnloadData);
            return;
        }
    }
    else {
        obj.UnloadInterval = null;
        obj.UnloadIntervalTime = null;
    }

    if (obj.IsLogWeight == true) {
        if (obj.LogWeightTime.length == 0) {
            alert(dictionary.FillLogWeightData);
            return;
        }
    }
    else {
        obj.LogWeightTime = null;
    }

    var file = document.getElementById("fileLink");
    file.href = '/SiloWeighing/Log?data=' + JSON.stringify(obj) + '&fromDate=' + $("#f_from_log_date").val() + '&toDate=' + $("#f_to_log_date").val();
    file.click();
}

function Save(obj) {

    if (obj.Active == false)
        obj.ScaleID = null;

    if (obj.IsLoad == true)
    {
        if (obj.LoadInterval.length == 0 || obj.LoadIntervalTime.length == 0) {
            alert(dictionary.FillLoadData);
            return;
        }
    }
    else
    {
        obj.LoadInterval = null;
        obj.LoadIntervalTime = null;
    }

    if (obj.IsUnload == true) {
        if (obj.UnloadInterval.length == 0 || obj.UnloadIntervalTime.length == 0) {
            alert(dictionary.FillUnloadData);
            return;
        }
    }
    else {
        obj.UnloadInterval = null;
        obj.UnloadIntervalTime = null;
    }

    if (obj.IsLogWeight == true) {
        if (obj.LogWeightTime.length == 0) {
            alert(dictionary.FillLogWeightData);
            return;
        }
    }
    else {
        obj.LogWeightTime = null;
    }

    $.ajax({
        type: 'POST',
        url: '/SiloWeighing/Save',
        contentType: 'application/json; charset=utf-8',

        data: JSON.stringify({ data: JSON.stringify(obj)}),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                LoadSilos();
                $("#current").modal('hide');
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function SetSilosScale(scale)
{
    var cardWidth = 250;
    var cardHeight = 460;
    var canvasWidth = 135;
    var canvasHeight = 270;
    var fontsize = 25;
    if (scale != undefined) {
        switch (scale) {
            case '1':
                cardWidth = 250;
                cardHeight = 460;
                canvasWidth = 135;
                canvasHeight = 270;
                fontsize = 30;
                break;
            case '2':
                cardWidth = 90;
                cardHeight = 200;
                canvasWidth = 45;
                canvasHeight = 90;
                fontsize = 15;
                break;
        }
    }
    $('#silos_panel .card').css("width", cardWidth + "px");
    $('#silos_panel .card').css("height", cardHeight + "px");
    $('#silos_panel canvas').css("width", canvasWidth + "px");
    $('#silos_panel canvas').css("height", canvasHeight + "px");
    $('#silos_panel canvas').css("height", canvasHeight + "px");
    $('#silos_panel .silo-item').css("font-size", fontsize + "px");
}

function SiloProcess()
{
    var silo_panel = $('#silos_panel');
    $(silo_panel).empty();
    $(silo_panel).append(CreateSilo(1));
    $(silo_panel).append(CreateSilo(2));
    $(silo_panel).append(CreateSilo(3));
    $(silo_panel).append(CreateSilo(4));
    $(silo_panel).append(CreateSilo(5));
    $(silo_panel).append(CreateSilo(6));
    $(silo_panel).append(CreateSilo(7));
    $(silo_panel).append(CreateSilo(8));
    ActivateSilo(1);
    ActivateSilo(2);
    ActivateSilo(3);
    ActivateSilo(4);
    ActivateSilo(5);
    ActivateSilo(6);
    ActivateSilo(7);
    ActivateSilo(8);

}

function AllowedRows(len) {
    $.ajax({
        type: 'POST',
        url: '/SiloWeighing/AllowedRows',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: len }),
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                if (data.data == false)
                    $("#btnNew").hide();
                else
                    $("#btnNew").show();
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function UpdateSiloLog(elem, isload,weight)
{
    $.ajax({
        type: 'POST',
        url: '/SiloWeighing/UpdateLog',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(elem), loading: isload, weight: weight, time: moment().format('DD/MM/YYYY HH:mm:ss') }),
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
         
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function ActivateSilo(id,val)
{
    let silo = new Silo(document.getElementById("canvas_" + id), val);
    silos[id] = silo;
    silo.ID = id;
    silo.LoadStateChanged = function (val) {
        if (val == true) {
            $("#silo_state_" + this.ID).text(dictionary.LoadStart);
            UpdateSiloLog(this.data, true, $("#silo_weight_" + this.ID).text());
        }
        else
            $("#silo_state_" + this.ID).text('');
    };
    silo.UnloadStateChanged = function (val) {
        if (val == true) {
            $("#silo_state_" + this.ID).text(dictionary.UnloadStart);
            UpdateSiloLog(this.data, false, $("#silo_weight_" + this.ID).text());
        }
        else
            $("#silo_state_" + this.ID).text('');
    };

    silo.LogWeight = function (val) {
        UpdateSiloLog(this.data, null, $("#silo_weight_" + this.ID).text());
    };

    silo.animate(silo);
    if (silo.data.Active == true && silo.data.ScaleID > 0)
        silo.start_weight($("#silo_weight_" + id), silo);
    else
        $("#silo_weight_" + id).text(dictionary.InActive);
    /*setInterval(
        function () {
            silo.a = Math.floor(Math.random() * val.MaxCapacity);
            $("#silo_weight_" + id).text(silo.a);
        }
        , 2000);*/
}
function CreateSilo(id, val)
{
    var str = '<div class="m-1 silo-item align-middle" id="' + id + '">';
    str += '<a class="card text-white" style="background-color:#d64a9e">';
    str += '<div class="text-center">';
    str += '<label class="card-title row justify-content-center text-center SiteName" id="site_name_' + id + '">' + val.SiteName + '</label>';
    str += '<label class="card-title  row justify-content-center text-center SiloName" id="silo_name_' + id + '">' + val.Name + '</label>';
    str += '<canvas id="canvas_' + id + '"></canvas>';
    str += '<div class="row justify-content-center text-center">';
    str += '<label class="card-title" id="silo_weight_' + id + '"></label>';
    str += '<label class="card-title Unit m-1" id="silo_unit_' + id + '">' + (val.UnitName == null ? "" : val.UnitName) + '</label>';
    str += '</div>';
    str += '<label class="card-title row justify-content-center text-center  silo-state" id="silo_state_' + id + '"></label>';
    str += '</div>';
    str += '</a>';
    str += '</div>';

    return str;
}


/*
function animate() {

    console.log("****y=" + y);
    console.log("(1-(a/b))*ch=" + (1 - (a / b)) * ch);
    //if (y > (1-(a/b))*ch) {
        requestAnimationFrame(animate);
    //}

    ctx.clearRect(0, 0, cw, ch);
    ctx.save();
    drawContainer(0, null, null);
    drawContainer(15, drawingColor, null);
    drawContainer(7, "white", "white");
    ctx.save();
    ctx.clip();
    drawLiquid();
    ctx.restore();
    ctx.restore();
    if (y > (1-(a/b))*ch)
        y--;
    else
        y++;

}

function drawLiquid() {
    ctx.beginPath();
    ctx.moveTo(0, y);
    for (var x = 0; x < cw; x += 10) {
        ctx.quadraticCurveTo(x + 10, y + 15, x + 20, y);
    }
    ctx.lineTo(cw, ch);
    ctx.lineTo(0, ch);
    ctx.closePath();
    ctx.fillStyle = drawingColor;
    ctx.fill();
}

function drawContainer(linewidth, strokestyle, fillstyle) {
    ctx.beginPath();
    ctx.moveTo(20, 100);
    //ctx.bezierCurveTo(121, 36, 133, 57, 144, 78);
    ctx.bezierCurveTo(20, 20, 175, 20, 180, 100); 
    ctx.rect(20,100,160,190);
    //ctx.bezierCurveTo(160, 109, 176, 141, 188, 175);
    //ctx.bezierCurveTo(206, 226, 174, 272, 133, 284);
    //ctx.bezierCurveTo(79, 300, 24, 259, 25, 202);
    //ctx.bezierCurveTo(25, 188, 30, 174, 35, 161);
    //ctx.bezierCurveTo(53, 115, 76, 73, 100, 31);
    //ctx.bezierCurveTo(103, 26, 106, 21, 109, 15);
    ctx.lineWidth = linewidth;
    ctx.strokeStyle = strokestyle;
    ctx.stroke();
    if (fillstyle) {
        ctx.fillStyle = fillstyle;
        ctx.fill();
    }
}
*/
function LoadSilos()
{
    $.ajax({
        type: 'POST',
        url: '/SiloWeighing/Get',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ data: JSON.stringify(new Filter()) }),
        async: false,
        dataType: 'json',
        success: function (data) {
            if (data.isSucceded == false) {
                alert(data.message);
            }
            else {
                var silos_panel = $("#silos_panel");
                $.each(silos, function (i, val) {
                    val.stop_weight();
                });
                silos = [];
                silos_panel.empty();
                $.each(data.data, function (i, val) {
                    $(silos_panel).append(CreateSilo(i,val));
                    ActivateSilo(i,val);
                });
                SetSilosScale($("#f_silos_views").val());
            }
        },
        error: function (xhr, ajaxOptions, thrownError)
        { console.log(xhr.responseText) }

    });
}

function Filter() {
    this.SiteName = $("#f_site_name").val();
    this.Name = $("#f_name").val();
    //this.MaxCapacity = $("#silo_max_capacity_current").val();
    //this.ScaleID = $("#scales").val();
    //this.LoadInterval = $("#silo_load_interval_current").val();
    //this.LoadIntervalTime = $("#silo_load_interval_time_current").val();
    //this.UnloadInterval = $("#silo_unload_interval_current").val();
    //this.UnloadIntervalTime = $("#silo_unload_interval_time_current").val();
    this.Active = $("#f_active").prop("checked");
    //this.IsLoad = $("#silo_is_load_current").prop("checked");
    //this.IsUnload = $("#silo_is_unload_current").prop("checked");
}

function Scale() {
    this.ScalesTypeID = 1;
    this.Active = true;
    this.Status = true;
}

function CreateFilterEvents() {
    $("#f_name").on('input', function () {
        LoadSilos();
    });

    $("#f_site_name").on('input', function () {
        LoadSilos();
    });

    $("#f_id").on('input', function () {
        LoadSilos();;
    });

    $("#f_active").on('input', function () {
        LoadSilos();
    });

}